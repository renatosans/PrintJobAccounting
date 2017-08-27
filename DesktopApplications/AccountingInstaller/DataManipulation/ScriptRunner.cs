using System;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingInstaller.Util;


namespace AccountingInstaller.DataManipulation
{
    public class ScriptRunner
    {
        private SqlConnection sqlConnection;

        private IListener listener;

        private String targetDatabase;

        private ContainerHandler containerHandler;

        private ProgressMeter progressMeter;

        private String currentScript;

        private int scriptsExecuted;


        public ScriptRunner(SqlConnection sqlConnection, IListener listener, String targetDatabase)
        {
            this.sqlConnection = sqlConnection;
            this.listener = listener;
            this.targetDatabase = targetDatabase;
            this.containerHandler = new ContainerHandler();
        }


        private void ExecuteQuery(String query)
        {
            // Separa a query em partes, executa cada parte separadamente ( comando "GO" )
            String[] subQueries = query.Split(new String[] { "GO\r\n", "Go\r\n", "go\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String subQuery in subQueries)
            {
                String fixedQuery = subQuery;
                if (subQuery.Contains("USE "))
                {
                    // Comenta a query para evitar erros no Azure
                    fixedQuery = fixedQuery.Replace("USE ", "-- USE "); // Manter o espaço para não substituir USER

                    String dbName = subQuery.Replace("USE ", "").Trim(); // Manter o espaço para não substituir USER

                    // Muda o database na conexão para evitar erros no Azure
                    if (dbName != sqlConnection.Database)
                    {
                        Relocate relocate = new Relocate(dbName);
                        listener.NotifyObject(relocate);
                        listener.NotifyObject("Relocate()  Database alterado para -> " + dbName);
                        this.sqlConnection = relocate.sqlConnection;
                    }
                }

                DBQuery dbQuery = new DBQuery(fixedQuery, sqlConnection);
                dbQuery.Execute(false);
            }
        }


        public void RunAll(String scriptsDirectory, IProgressListener progressListener)
        {
            listener.NotifyObject("Origem(caminho): " + scriptsDirectory);
            listener.NotifyObject("Destino(database): " + sqlConnection.Database);

            String path = PathFormat.Adjust(scriptsDirectory);
            List<String> scriptList = new List<String>();

            TextReader textReader = new StreamReader(path + "ExecutionOrder.txt");
            String line = "";
            while (line != null)
            {
                line = textReader.ReadLine();
                if (line != null)
                    scriptList.Add(line);
            }
            textReader.Close();

            scriptsExecuted = 0;
            progressMeter = new ProgressMeter(scriptList.Count, progressListener);
            foreach (String script in scriptList)
            {
                currentScript = script;
                ContainedFile file = containerHandler.ExtractFromContainer(path + "ScriptFiles.xml", script);
                if (file == null)
                    throw new Exception("Script não encontrado no XML: " + script);

                ExecuteQuery(file.fileContent);
                scriptsExecuted++;
                progressMeter.IncreaseProgress(1);
            }
        }

        public String GetCurrentScript()
        {
            return currentScript;
        }

        public int GetFileCount()
        {
            return scriptsExecuted;
        }
    }

}
