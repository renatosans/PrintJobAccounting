using System;
using System.IO;
using System.Collections.Generic;


namespace AccountingLib.ServerPrintLog
{
    public class PrintLogSorter
    {
        private SortedList<int, String> fileList = new SortedList<int, String>();

        public PrintLogSorter(String logPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(logPath);
            foreach (FileInfo fileInfo in dirInfo.GetFiles())
            {
                String fileName = fileInfo.Name;
                int? fileID = PrintLogFile.GetToken(fileName);

                if (fileID != null)
                    fileList.Add(fileID.Value, fileName);
            }
        }

        public String[] GetOrderedFiles()
        {
            int fileCount = fileList.Values.Count;
            String[] orderedFiles = new String[fileCount];

            for (int ndx = 0; ndx < fileCount; ndx++)
            {
                orderedFiles[ndx] = fileList.Values[ndx];
            }

            return orderedFiles;
        }
    }

}
