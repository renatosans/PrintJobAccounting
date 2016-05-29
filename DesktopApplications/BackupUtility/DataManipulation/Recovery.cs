using System;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Collections.Generic;
using Util;


namespace DataManipulation
{
    /// <summary>
    /// Classe responsável pela importação/exportação da massa de dados do sistema, permite recuperar os dados
    /// do sistema durante uma migração de servidor ou um crash. São limitações desta classe:
    /// - os nomes das tabelas devem ter o prefixo "tb_"
    /// - as chaves primárias das tabelas devem se chamar "id"
    /// - a filtragem de registros é feita pela foregin key "tenantId", ela deve ter exatamente esse nome
    /// </summary>
    public class Recovery: IListener
    {
        private DBAccess saAccess;

        private SqlConnection sqlConnection;

        private String dataDirectory;

        private String currentDatabase;

        private List<Object> databaseForeignKeys;

        private String[] filesToImport;

        private Boolean decimalSeparatorIsComma;

        private ProgressMeter progressMeter;

        private String warnings;

        private String lastError;


        /// <summary>
        /// Construtor da classe recovery, recebe ao ser instanciada o login de system admin e o diretório
        /// de importação/exportação dos arquivos xml
        /// </summary>
        public Recovery(DBAccess saAccess, String dataDirectory)
        {
            this.saAccess = saAccess;
            this.sqlConnection = new SqlConnection();
            this.dataDirectory = dataDirectory;
        }

        private Boolean OpenConnection()
        {
            if (saAccess == null) return false;

            try
            {
                sqlConnection.ConnectionString = @"Data Source=" + saAccess.server + ";User=" + saAccess.saLogin.username + "; password=" + saAccess.saLogin.password;
                sqlConnection.Open();
            }
            catch
            {
                return false;
            }

            // Se não houve nenhuma falha retorna status de sucesso
            return true;
        }

        private void CloseConnection()
        {
            sqlConnection.Close();
        }

        private String AdjustNumber(String number)
        {
            String result = number;

            if (decimalSeparatorIsComma) // No T-SQL o separador decimal é o ponto então é necessário a conversão
            {
                result = result.Replace(".", ""); // retira pontuação de milhar
                result = result.Replace(",", "."); // substitui virgula por ponto
            }

            return result;
        }

        private String AdjustText(String text)
        {
            String result = text;

            // O usuário pode utilizar livremente  >  <  & ' "  no texto pois é feito o tratamento a seguir
            // São sequências de caracteres reservadas para o XML:   &lt;   &gt;   &amp;   &apos;   &quot;
            // O usuário não deveria utilizar estas sequências em textos pois elas tem significado especial
            result = result.Replace("<", "&lt;");
            result = result.Replace(">", "&gt;");
            result = result.Replace("&", "&amp;");
            result = result.Replace("'", "&apos;&apos;"); // respeita a sintaxe do XML e do T-SQL
            result = result.Replace("\"", "&quot;");

            return result;
        }

        private String ConvertFieldValue(Object fieldValue)
        {
            if (fieldValue == null) return "\"null\"";

            // Converte o objeto para String para gravar o valor no XML
            String result = "" + fieldValue;

            // Verifica o tipo do objeto e faz tratamentos adicionais
            if (fieldValue.GetType() == typeof(String)) result = "&apos;" + AdjustText(result) + "&apos;";
            if (fieldValue.GetType() == typeof(DateTime)) result = "&apos;" + ((DateTime)fieldValue).ToString("yyyy-MM-ddTHH:mm:ss") + "&apos;";
            if (fieldValue.GetType() == typeof(Boolean)) result = (Boolean)fieldValue ? "1" : "0";
            if (fieldValue.GetType() == typeof(Double)) result = AdjustNumber(result);
            if (fieldValue.GetType() == typeof(Decimal)) result = AdjustNumber(result);

            // Adiciona aspas ao valor do objeto, isso é necessário independente do tipo
            return "\"" + result + "\"";
        }

        private Boolean ExportTable(String tableName, String[] fieldNames, List<Object> rowList)
        {
            // Verifica se o diretório de saída está disponível
            if (!Directory.Exists(dataDirectory)) return false;

            // Remove o prefixo do nome da tabela e acrescenta a extensão do arquivo
            String entityName = tableName.Remove(0, 3);
            String outputFile = entityName + ".xml";

            // Abre o xml de saída para gravação
            StreamWriter streamWriter = File.CreateText(PathFormat.Adjust(dataDirectory) + outputFile);
            streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            streamWriter.WriteLine("<" + entityName + "Table name=\"" + tableName + "\" database=\"" + currentDatabase + "\" server=\"" + saAccess.server + "\" >");
            foreach(Object row in rowList)
            {
                String tag = entityName;
                Object[] fieldValues = (Object[])row;
                for (int index = 0; index < fieldValues.Length; index++)
                {
                    String attribute = fieldNames[index] + "=" + ConvertFieldValue(fieldValues[index]);
                    tag = tag + " " + attribute;
                }
                streamWriter.WriteLine("   <" + tag + " />");
            }
            streamWriter.WriteLine("</" + entityName + "Table>");
            streamWriter.Close();

            return true;
        }

        /// <summary>
        /// Exporta as tabelas existentes no database para arquivos XML, os dados são gravados no XML em
        /// formato pronto para INSERT (sem informação de tipo porém no formato esperado pelo BD)
        /// </summary>
        public Boolean DBExport(String databaseName)
        {
            if (!OpenConnection()) return false;

            currentDatabase = databaseName;
            decimalSeparatorIsComma = FieldParser.IsCommaDecimalSeparator();
            DBQuery dbQuery = new DBQuery(sqlConnection);
            dbQuery.Query = "use " + currentDatabase;
            dbQuery.Execute(false);

            dbQuery.Query = "SELECT name, id FROM sysObjects WHERE xtype = 'U'";
            dbQuery.Execute(true);
            List<Object> tableList = dbQuery.ExtractFromResultset(typeof(DBObject));
            if (tableList.Count < 1)
            {
                CloseConnection();
                return false;
            }

            foreach (DBObject table in tableList)
            {
                dbQuery.Query = "SELECT name FROM sysColumns WHERE id = " + table.id;
                dbQuery.Execute(true);
                List<Object> fieldList = dbQuery.ExtractFromResultset(new String[] { "name" });
                String[] fieldNames = new String[fieldList.Count];
                for (int index = 0; index < fieldList.Count; index++)
                    fieldNames[index] = (String)((Object[])fieldList[index])[0];

                dbQuery.Query = "SELECT * FROM " + table.name;
                dbQuery.Execute(true);
                List<Object> rowList = dbQuery.ExtractFromResultset(fieldNames);

                Boolean tableExported = ExportTable(table.name, fieldNames, rowList);
                if (!tableExported)
                {
                    CloseConnection();
                    return false;
                }
            }

            CloseConnection();
            return true;
        }

        private Boolean ImportFile(String filename, List<int> tenantsToImport, Boolean preserveRecordId)
        {
            /*
            String tableName = "tb_" + Path.GetFileNameWithoutExtension(filename);
            XmlNode rowCollection;

            ImportData importData = new ImportData(filename);
            currentDatabase = importData.DatabaseName;
            rowCollection = importData.MainNode;
            if ((currentDatabase == null) || (rowCollection == null))
            {
                lastError = importData.GetLastError();
                return false;
            }

            // Se o XML não possui registros considera como se tivesse importado e retorna
            if (rowCollection.FirstChild == null) return true;

            DBQuery dbQuery = new DBQuery(sqlConnection);
            // Altera o banco de dados para o banco onde a tabela se encontra
            dbQuery.Query = "use " + currentDatabase;
            dbQuery.Execute(false);

            // Verifica se a tabela já está populada, se estiver aborta
            ImportController controller = new ImportController(sqlConnection, importData, this);
            if (!controller.TableIsEmpty())
            {
                lastError = "A importação foi cancelada pois a tabela " + tableName + " já possui dados.";
                return false;
            }

            try // tenta fazer a inserção dos dados na tabela
            {
                controller.MountDependencies(filesToImport, databaseForeignKeys);
                int? lastInsertedId = 0;
                while (controller.MoveNextRecord())
                {
                    int? owner = controller.GetRecordOwner();
                    if ((owner == null) || (tenantsToImport.Contains(owner.Value)))
                    {
                        // Conserta o identity para que seja mantida a numeração original
                        if (preserveRecordId) controller.FixIdentity(lastInsertedId);

                        dbQuery.Query = controller.MountInsertStatement();
                        dbQuery.Execute(true);
                        lastInsertedId = dbQuery.ExtractFromResultset();
                    }
                    progressMeter.IncreaseProgress(1);
                }
            }
            catch (Exception exc)
            {
                lastError = exc.Message;
                return false;
            }
            */
            return true;
        }

        /// <summary>
        /// Importa arquivos de dados XML para o BD
        /// </summary>
        public Boolean DBImport(String databaseName, List<int> tenantsToImport, Boolean preserveRecordId, IProgressListener progressListener)
        {
            /*
            if (!OpenConnection()) return false;

            // Verifica se existem arquivos XML com a massa de dados
            DirectoryInfo dirInfo = new DirectoryInfo(dataDirectory);
            FileInfo[] dataFiles = dirInfo.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            if (dataFiles.Length < 1)
            {
                lastError = "Nenhum arquivo XML encontrado no diretório";
                CloseConnection();
                return false;
            }

            // Verifica quantos registros precisam ser importados e cria o medidor de progresso
            progressMeter = new ProgressMeter(ImportContext.GetRecordAmount(dataFiles), progressListener);

            // Busca as chaves estrangeiras existentes no database
            databaseForeignKeys = ImportContext.GetForeignKeys(sqlConnection, databaseName);

            // Ordena os arquivos encontrados no diretório e importa apenas os que pertencem ao database
            filesToImport = ImportContext.SortImportFiles(sqlConnection, databaseName, dataFiles);
            foreach (String filename in filesToImport)
            {
                if (!ImportFile(filename, tenantsToImport, preserveRecordId))
                {
                    CloseConnection();
                    return false; // lastError foi preenchido no método "ImportFile"
                }
            }

            CloseConnection();
            */
            return true;
        }

        public void NotifyObject(Object obj)
        {
            if (obj is String)
            {
                // aborta se a string já está presente em warnings
                if ((warnings != null) && (warnings.Contains((String)obj))) return;

                warnings += (String)obj + Environment.NewLine;
            }
        }

        public String GetWarnings()
        {
            return warnings;
        }

        public String GetLastError()
        {
            return lastError;
        }
    }

}
