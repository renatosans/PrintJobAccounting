using System;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingInstaller.Util;


namespace AccountingInstaller.DataManipulation
{
    public class ImportController
    {
        private SqlConnection sqlConnection;

        private ImportData tableData;

        private Dictionary<String, ImportData> tableDependencies;

        private IListener listener;

        private List<Object> databaseForeignKeys;

        private XmlNode currentRecord;

        private String currentTableReference;


        public ImportController(SqlConnection sqlConnection, ImportData tableData, IListener listener)
        {
            this.sqlConnection = sqlConnection;
            this.tableData = tableData;
            this.tableDependencies = new Dictionary<String, ImportData>();
            this.listener = listener;
            this.databaseForeignKeys = null;
            this.currentRecord = null;
            this.currentTableReference = null;
        }

        /// <summary>
        /// Verifica se a tabela está sem registros e se o id está zerado
        /// </summary>
        public Boolean TableIsEmpty()
        {
            DBQuery dbQuery = new DBQuery(sqlConnection);
            dbQuery.Query = "use " + tableData.DatabaseName;
            dbQuery.Execute(false);

            // Verifica se a tabela está sem registros
            dbQuery.Query = "SELECT TOP 1 * FROM " + tableData.TableName;
            dbQuery.Execute(true);
            List<Object> rowList = dbQuery.ExtractFromResultset(typeof(Object));
            if (rowList.Count > 0) return false;

            // Verifica se o id está zerado
            dbQuery.Query = "SELECT IDENT_CURRENT('" + tableData.TableName + "')";
            dbQuery.Execute(true);
            int? identity = dbQuery.ExtractFromResultset();
            if ((identity != null) && (identity.Value > 1)) return false;

            return true;
        }

        /// <summary>
        /// Monta as dependências da tabela a partir da lista de arquivos no database e as foreign keys
        /// </summary>
        public void MountDependencies(String[] fileList, List<Object> databaseForeignKeys)
        {
            // Busca as tabelas referenciadas e as foreignKeys com estas referências
            List<String> referencedTableList = new List<String>();
            foreach (Object foreignKey in databaseForeignKeys)
            {
                Object[] foreignKeyProperties = (Object[])foreignKey;
                String parentTable = (String)foreignKeyProperties[0];
                String refTable = (String)foreignKeyProperties[2];
                if (parentTable == tableData.TableName) referencedTableList.Add(refTable);
            }

            // Monta o dicionário com as dependências
            foreach (String filename in fileList)
            {
                String tableName = "tb_" + Path.GetFileNameWithoutExtension(filename);
                if (referencedTableList.Contains(tableName))
                    tableDependencies.Add(tableName, new ImportData(filename));
            }

            this.databaseForeignKeys = databaseForeignKeys;
        }

        /// <summary>
        /// Move o cursor para o próximo registro no XML
        /// </summary>
        public Boolean MoveNextRecord()
        {
            // Move para o primeiro caso não exista registro corrente
            if (currentRecord == null)
            {
                currentRecord = tableData.MainNode.FirstChild;
                return true;
            }

            // Tenta mover para o próximo registro
            currentRecord = currentRecord.NextSibling;
            if (currentRecord == null) return false;

            return true;
        }

        /// <summary>
        /// Obtem o tenant a que pertence o registro corrente
        /// </summary>
        public int? GetRecordOwner()
        {
            // Retorna null caso o registro não esteja posicionado
            if (currentRecord == null) return null;
 
            if (tableData.TableName == "tb_tenant")
                return int.Parse(currentRecord.Attributes["id"].Value);

            XmlAttribute tenantId = currentRecord.Attributes["tenantId"];
            if (tenantId != null)
                return int.Parse(tenantId.Value);

            // Caso seja um registro que não pertence a um tenant retorna null
            return null;
        }

        /// <summary>
        /// Corrige o id do próximo registro de acordo com o id existente na massa de dados
        /// </summary>
        public void FixIdentity(int? lastInsertedId)
        {
            // Aborta caso o registro não esteja posicionado
            if (currentRecord == null) return;

            // Aborta caso o registro não possua a chave primária "id"
            XmlAttribute idAttrib = currentRecord.Attributes["id"];
            if (idAttrib == null) return;
            int xmlCurrent = int.Parse(idAttrib.Value);

            // Aborta caso não seja fornecido o id do último registro inserido
            if (lastInsertedId == null) return;
            int dbCurrent = lastInsertedId.Value + 1;

            // Caso os ids sejam iguais nada precisa ser corrigido
            if (dbCurrent == xmlCurrent) return;

            // Dispara uma exceção caso a quantidade de registros inseridos ultrapasse a quantidade no xml
            // na verdade o id poderá ser maior se houverem falhas de inserção
            if (dbCurrent > xmlCurrent)
                throw new Exception("A numeração dos registros apresentou problemas.");

            DBQuery dbQuery = new DBQuery(sqlConnection);
            dbQuery.Query = "use " + tableData.DatabaseName;
            dbQuery.Execute(false);

            // Faz o "Re Seed" corrigindo o id
            int reseedValue = xmlCurrent - 1;
            if (lastInsertedId.Value == 0) reseedValue = xmlCurrent;
            dbQuery.Query = "DBCC CHECKIDENT (" + tableData.TableName + ", reseed, " + reseedValue + ")";
            dbQuery.Execute(false);
        }

        /// <summary>
        /// Monta a instrução de INSERT a partir do registro corrente do XML(com os dados da tabela)
        /// </summary>
        public String MountInsertStatement()
        {
            // Aborta caso o registro não esteja posicionado
            if (currentRecord == null) return "";

            String statement = "INSERT INTO {0}({1}) VALUES ({2}) SELECT SCOPE_IDENTITY() id";
            String fieldNames = "";
            String fieldValues = "";

            foreach (XmlAttribute attrib in currentRecord.Attributes)
            {
                String attribName = attrib.Name;
                String attribValue = attrib.Value;
                // Verifica se é chave estrangeira, corrige a referência caso necessário
                if ((attribValue != "null") && (IsForeignKey(tableData.TableName, attribName)))
                    attribValue = GetReferencedRecordFixedId(attribValue).ToString();

                // Ignora o atributo "id" ao inserir pois nas tabelas esse campo é "auto increment"
                if (attrib.Name != "id")
                {
                    if (!String.IsNullOrEmpty(fieldNames)) fieldNames += ", ";
                    fieldNames += attribName;
                    if (!String.IsNullOrEmpty(fieldValues)) fieldValues += ", ";
                    fieldValues += attribValue;
                }
            }

            return String.Format(statement, tableData.TableName, fieldNames, fieldValues);
        }

        private Boolean IsForeignKey(String tableName, String fieldName)
        {
            foreach (Object foreignKey in databaseForeignKeys)
            {
                Object[] foreignKeyProperties = (Object[])foreignKey;
                String parentTable = (String)foreignKeyProperties[0];
                String parentField = (String)foreignKeyProperties[1];
                if ((parentTable == tableName) && (parentField == fieldName))
                {
                    // Seta o apontamento corrente 
                    currentTableReference = (String)foreignKeyProperties[2];

                    return true;
                }
            }

            return false;
        }

        private String FixXPath(String text)
        {
            String fixedText;

            if (String.IsNullOrEmpty(text)) return "''"; // retorna String vazia delimitada por apóstrofes
            String[] parts = text.Split(new Char[] {'\''}); // quebra o texto em partes, o separador é o apóstrofe
            if (parts.Length < 2) return "'" + text + "'";
            fixedText = "concat("; // utiliza a função Concat do XLST
            for(int index = 0; index < parts.Length; index++)
            {
                if (fixedText != "concat(") fixedText += ", ";
                fixedText += "'" + parts[index] + "'";
                if (index != (parts.Length-1)) fixedText += ", " + '"' + "'" + '"';
            }
            fixedText += ")";

            return fixedText;
        }

        private XmlNode GetReferencedRecord(String dependency, String recordId)
        {
            ImportData dependencyData = tableDependencies[dependency];

            String mainNodeTag = dependencyData.MainNode.Name;
            String childNodeTag = dependencyData.MainNode.FirstChild.Name;
            String xpathQuery = "//" + mainNodeTag + "/" + childNodeTag + "[@id='" + recordId + "']";

            return dependencyData.MainNode.SelectSingleNode(xpathQuery);
        }

        private int GetRelativeIndex(String dependency, String recordId, NameValueCollection query)
        {
            ImportData dependencyData = tableDependencies[dependency];

            String mainNodeTag = dependencyData.MainNode.Name;
            String childNodeTag = dependencyData.MainNode.FirstChild.Name;
            String xpathQuery = "//" + mainNodeTag + "/" + childNodeTag + "[{0}]";
            String xpathPredicate = "";
            foreach(String fieldName in query)
            {
                String fieldValue = query[fieldName];
                if (!String.IsNullOrEmpty(xpathPredicate)) xpathPredicate += " and ";
                xpathPredicate += "@" + fieldName + "=" + FixXPath(fieldValue);
            }

            // Busca a lista de registros que se enquadram na query
            XmlNodeList recordList = dependencyData.MainNode.SelectNodes(String.Format(xpathQuery, xpathPredicate));
            if (recordList.Count > 1)
            {
                int index = 0;
                foreach (XmlNode record in recordList)
                {
                    index++;
                    if (record.Attributes["id"].Value == recordId) return index;
                }
            }

            return 0;
        }

        private int GetReferencedRecordFixedId(String oldId)
        {
            // Busca o registro referenciado no xml(dependência)
            String dependency = currentTableReference;
            XmlNode referencedRecord = GetReferencedRecord(dependency, oldId);
            if (referencedRecord == null) return 0;

            // Monta a query de seleção do registro
            NameValueCollection query = new NameValueCollection();
            String queryText = "";
            foreach (XmlAttribute attrib in referencedRecord.Attributes)
            {
                // Se não for chave primária nem estrangeira adiciona na query
                if ((attrib.Name != "id") && (!IsForeignKey(dependency, attrib.Name)))
                {
                    query.Add(attrib.Name, attrib.Value);
                    if (!String.IsNullOrEmpty(queryText)) queryText += " AND ";
                    String condition = attrib.Name + "=" + attrib.Value;
                    if (attrib.Value == "null") condition = attrib.Name + " is NULL";
                    queryText += condition;
                }
            }

            // Busca no banco o registro equivalente
            String statement = "SELECT id FROM " + dependency + " WHERE {0}";
            DBQuery dbQuery = new DBQuery(sqlConnection);
            dbQuery.Query = "use " + tableDependencies[dependency].DatabaseName;
            dbQuery.Execute(false);
            dbQuery.Query = String.Format(statement, queryText);
            dbQuery.Execute(true);
            // Busca o indice relativo ( quando existem varios registros que atendem a query )
            int relativeIndex = GetRelativeIndex(dependency, oldId, query);
            if (relativeIndex == 0)
            {
                int? referencedRecordId = dbQuery.ExtractFromResultset();
                if (referencedRecordId == null) return 0;
                return referencedRecordId.Value;
            }
            // A existência de vários registros correspondentes é um indício de que podem existir
            // dados corrompidos, então avisa o usuário e utiliza o indice relativo.
            String importWarning = "A tabela " + tableData.TableName + " apresentou problemas durante a importação. Faça uma verificação.";
            if (listener != null) listener.NotifyObject(importWarning);
            List<Object> resultset = dbQuery.ExtractFromResultset(new String[] {"id"});
            return (int)((Object[])resultset[relativeIndex-1])[0];
        }
    }

}
