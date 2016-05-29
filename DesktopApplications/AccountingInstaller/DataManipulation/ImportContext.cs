using System;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingInstaller.Util;


namespace AccountingInstaller.DataManipulation
{
    public static class ImportContext
    {
        /// <summary>
        /// Verifica quantos registros existem na massa de dados
        /// </summary>
        public static long GetRecordAmount(FileInfo[] dataFiles)
        {
            long recordAmount = 0;

            foreach (FileInfo dataFile in dataFiles)
            {
                String rootNodeName = Path.GetFileNameWithoutExtension(dataFile.Name) + "Table";
                XmlTextReader xmlReader = new XmlTextReader(dataFile.FullName);
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                        if (xmlReader.Name != rootNodeName) recordAmount++;
                }
                xmlReader.Close();
            }

            return recordAmount;
        }

        /// <summary>
        /// Busca as foreign keys do database
        /// </summary>
        public static List<Object> GetForeignKeys(SqlConnection sqlConnection, String databaseName)
        {
            List<Object> foreignKeys = null;

            DBQuery dbQuery = new DBQuery(sqlConnection);
            dbQuery.Query = "use " + databaseName;
            dbQuery.Execute(false);
            dbQuery.Query = "SELECT OBJECT_NAME(PARENT_OBJECT_ID)         SrcTable," + Environment.NewLine +
                            "       SRC.NAME                              SrcField," + Environment.NewLine +
                            "       OBJECT_NAME(REFERENCED_OBJECT_ID)     RefTable," + Environment.NewLine +
                            "       REF.NAME                              RefField " + Environment.NewLine +
                            "FROM   SYS.FOREIGN_KEY_COLUMNS FKC                    " + Environment.NewLine +
                            "       JOIN SYS.COLUMNS SRC                           " + Environment.NewLine +
                            "         ON FKC.PARENT_OBJECT_ID = SRC.OBJECT_ID      " + Environment.NewLine +
                            "            AND FKC.PARENT_COLUMN_ID = SRC.COLUMN_ID  " + Environment.NewLine +
                            "       JOIN SYS.COLUMNS REF                           " + Environment.NewLine +
                            "         ON FKC.REFERENCED_OBJECT_ID = REF.OBJECT_ID  " + Environment.NewLine +
                            "            AND FKC.REFERENCED_COLUMN_ID = REF.COLUMN_ID";
            dbQuery.Execute(true);
            foreignKeys = dbQuery.ExtractFromResultset(new String[] { "SrcTable", "SrcField", "RefTable", "RefField" });

            return foreignKeys;
        }

        /// <summary>
        /// Checa quais são os tenants existentes na na massa de dados
        /// </summary>
        public static List<DBObject> GetTenantsFromImportData(String dataDirectory)
        {
            List<DBObject> tenantList = new List<DBObject>();

            // Retorna uma lista vazia caso não encontre o diretório de dados
            Boolean dirExists = Directory.Exists(dataDirectory);
            if (!dirExists) return tenantList;

            // Retorna uma lista vazia caso não encontre o arquivo de dados 
            String filename = PathFormat.Adjust(dataDirectory) + "tenant.xml";
            Boolean fileExists = File.Exists(filename);
            if (!fileExists) return tenantList;

            // Retorna uma lista vazia caso não consiga carregar os dados
            XmlNode tenantTable = (new ImportData(filename)).MainNode;
            if (tenantTable == null) return tenantList;

            foreach (XmlNode tenant in tenantTable)
            {
                int id = int.Parse(tenant.Attributes["id"].Value);
                String name = tenant.Attributes["alias"].Value;
                // Remove os apóstrofes que delimitam a String
                name = name.Remove(0, 1);
                name = name.Remove(name.Length - 1, 1);

                tenantList.Add(new DBObject(id, name));
            }

            return tenantList;
        }

        /// <summary>
        /// Monta a ordem de importação das tabelas de acordo com as foreign keys existentes no banco,
        /// tabelas referenciadas devem ser importadas antes das tabelas não referenciadas.
        /// Recebe como parâmetro uma lista de XMLs/dados, filtra apenas os pertencentes ao banco
        /// </summary>
        public static String[] SortImportFiles(SqlConnection sqlConnection, String databaseName, FileInfo[] dataFiles)
        {
            // Verifica quais arquivos de dados pertencem ao database
            List<String> databaseContents = new List<String>();
            foreach (FileInfo dataFile in dataFiles)
            {
                ImportData importData = new ImportData(dataFile.FullName);
                if (importData.DatabaseName == databaseName) databaseContents.Add(dataFile.FullName);
            }

            // Empilha os arquivos de acordo com sua prioridade de importação
            Stack<String> fileStack = new Stack<String>();
            List<String> filesLeft = new List<String>();
            filesLeft.AddRange(databaseContents);
            List<Object> foreignKeys = GetForeignKeys(sqlConnection, databaseName);
            while (fileStack.Count != databaseContents.Count)
            {
                // Empilha as tabelas não referenciadas primeiro (fica embaixo na pilha), remove
                // as foreignkeys quebradas, repete o processo
                PushNotReferenced(fileStack, filesLeft, foreignKeys);
                RemoveBroken(foreignKeys, fileStack);
            }

            // Monta o array com os arquivos ordenados
            String[] orderedFiles = new String[databaseContents.Count];
            int fileIndex = 0;
            while (fileStack.Count > 0)
            {
                String filename = fileStack.Pop();
                orderedFiles[fileIndex] = filename;
                fileIndex++;
            }

            return orderedFiles;
        }

        // Empilha os os arquivos das tabelas que não são referenciadas
        private static void PushNotReferenced(Stack<String> fileStack, List<String> filesLeft, List<Object> foreignKeys)
        {
            foreach (String filename in filesLeft)
            {
                String tableName = "tb_" + Path.GetFileNameWithoutExtension(filename);
                Boolean isReferenced = TableIsReferenced(tableName, foreignKeys);
                Boolean isOnlySelfReferenced = TableIsOnlySelfReferenced(tableName, foreignKeys);
                if ((!isReferenced) || (isOnlySelfReferenced)) fileStack.Push(filename);
            }

            // Atualiza a lista de arquivos restantes
            foreach (String filename in fileStack)
                if (filesLeft.Contains(filename)) filesLeft.Remove(filename);
        }

        // Verifica se a tabela é referenciada por alguma foreignkey do database
        private static Boolean TableIsReferenced(String tableName, List<Object> foreignKeys)
        {
            foreach (Object foreignKey in foreignKeys)
            {
                Object[] foreignKeyProperties = (Object[])foreignKey;
                String refTable = (String)foreignKeyProperties[2];
                if (refTable == tableName) return true;
            }
            return false;
        }

        // Verifica se a tabela é referenciada somente por si mesma
        private static Boolean TableIsOnlySelfReferenced(String tableName, List<Object> foreignKeys)
        {
            int referenceCount = 0;
            Boolean selfReferenced = false;

            foreach (Object foreignKey in foreignKeys)
            {
                Object[] foreignKeyProperties = (Object[])foreignKey;
                String parentTable = (String)foreignKeyProperties[0];
                String refTable = (String)foreignKeyProperties[2];
                if (refTable == tableName) referenceCount++;
                if (refTable == parentTable) selfReferenced = true;
            }
            // Caso a tabela seja referenciada apenas por si mesma retorna "true"
            if ((referenceCount == 1) && (selfReferenced)) return true;

            return false;
        }

        // Remove as foreign keys quebradas
        private static void RemoveBroken(List<Object> foreignKeys, Stack<String> fileStack)
        {
            // Obtem os nomes das tabelas que já estão em fileStack
            List<String> stackedTables = new List<String>();
            foreach (String filename in fileStack)
            {
                String tableName = "tb_" + Path.GetFileNameWithoutExtension(filename);
                stackedTables.Add(tableName.ToLower());
            }

            // Monta a lista de foreign keys que devem ser removidas
            List<Object> removeList = new List<Object>();
            foreach (Object foreignKey in foreignKeys)
            {
                Object[] foreignKeyProperties = (Object[])foreignKey;
                String parentTable = (String)foreignKeyProperties[0];
                if (stackedTables.Contains(parentTable.ToLower())) removeList.Add(foreignKey);
            }

            // Executa a remoção
            foreach (Object foreignKey in removeList) foreignKeys.Remove(foreignKey);
        }
    }

}
