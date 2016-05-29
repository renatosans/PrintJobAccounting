using System;
using System.IO;
using System.Xml;


namespace AccountingInstaller.DataManipulation
{
    public class ImportData
    {
        private XmlNode mainNode;

        private String tableName;

        private String databaseName;

        public XmlNode MainNode
        {
            get { return mainNode; }
        }

        public String TableName
        {
            get { return tableName; }
        }

        public String DatabaseName
        {
            get { return databaseName; }
        }

        private String lastError;


        public ImportData(String filename)
        {
            // Obtem o nome da tag raiz do XML, que é o nome da entidade seguido de "Table"
            String dataTable = Path.GetFileNameWithoutExtension(filename) + "Table";
            try // Tenta abrir o xml para leitura
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                mainNode = xmlDoc.SelectSingleNode("//" + dataTable);
                tableName = mainNode.Attributes["name"].Value;
                databaseName = mainNode.Attributes["database"].Value;
            }
            catch (Exception exc)
            {
                lastError = "Erro ao ler o XML de dados " + dataTable + ". " + exc.Message;
                mainNode = null;
                tableName = null;
                databaseName = null;
                return;
            }
        }

        public String GetLastError()
        {
            return lastError;
        }
    }

}
