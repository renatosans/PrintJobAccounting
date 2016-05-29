﻿using System;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingInstaller.Util;


namespace AccountingInstaller.DataManipulation
{
    public class ScriptRunner
    {
        private String databaseName;

        private SqlConnection sqlConnection;

        private ContainerHandler containerHandler;

        private ProgressMeter progressMeter;

        private String currentScript;

        private int scriptsExecuted;


        public ScriptRunner(String databaseName, SqlConnection sqlConnection)
        {
            this.databaseName = databaseName;
            this.sqlConnection = sqlConnection;
            this.containerHandler = new ContainerHandler();
        }


        private void ExecuteQuery(String query, Boolean replaceTargetDatabase)
        {
            // Separa a query em partes, executa cada parte separadamente ( comando "GO" )
            String[] subQueries = query.Split(new String[] { "GO\r\n", "Go\r\n", "go\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String subQuery in subQueries)
            {
                String fixedQuery = subQuery;
                if (replaceTargetDatabase && fixedQuery.Contains("[targetDatabase]"))
                    fixedQuery = fixedQuery.Replace("[targetDatabase]", databaseName);

                DBQuery dbQuery = new DBQuery(fixedQuery, sqlConnection);
                dbQuery.Execute(false);
            }
        }


        public void RunAll(String scriptsDirectory, IProgressListener progressListener)
        {
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

                ExecuteQuery(file.fileContent, true);
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
