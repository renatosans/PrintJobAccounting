﻿using System;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingInstaller.Util;


namespace AccountingInstaller.DataManipulation
{
    public class ScriptRunner
    {
        private String[] databaseNames;

        private SqlConnection sqlConnection;

        private IListener listener;

        private ContainerHandler containerHandler;

        private ProgressMeter progressMeter;

        private String currentScript;

        private int scriptsExecuted;


        public ScriptRunner(String[] databaseNames, SqlConnection sqlConnection, IListener listener)
        {
            this.databaseNames = databaseNames;
            this.sqlConnection = sqlConnection;
            this.listener = listener;
            this.containerHandler = new ContainerHandler();
        }


        private void ExecuteQuery(String query)
        {
            // Separa a query em partes, executa cada parte separadamente ( comando "GO" )
            String[] subQueries = query.Split(new String[] { "GO\r\n", "Go\r\n", "go\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String subQuery in subQueries)
            {
                String fixedQuery = subQuery;
                foreach (String dbName in databaseNames)
                {
                    if (subQuery.Contains("USE " + dbName))
                    {
                        fixedQuery = fixedQuery.Replace("USE", "-- USE"); // Comenta a query para evitar erros no Azure
                        sqlConnection.ChangeDatabase(dbName); // Muda o database na conexão para evitar erros no Azure
                        listener.NotifyObject("connection.ChangeDatabase()  Database alterado para -> " + sqlConnection.Database);
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
