using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.AppUtils;
using DocMageFramework.Reporting;


namespace AccountingLib.ServerCopyLog
{
    public class CopyLogPersistence
    {
        private int tenantId;

        private SqlConnection sqlConnection;

        private IListener listener;


        public CopyLogPersistence(int tenantId, SqlConnection sqlConnection, IListener listener)
        {
            this.tenantId = tenantId;
            this.sqlConnection = sqlConnection;
            this.listener = listener;
        }

        /// <summary>
        /// Verifica se registros de log correspondentes ao período já foram inseridos no
        /// banco de dados
        /// </summary>
        public Boolean FileImported(DateRange dateRange)
        {
            Boolean imported = false;

            // Verifica se a faixa de datas foi fornecida
            if (dateRange == null) return false;

            // Verifica se existe alguma impressão no intervalo de datas
            CopiedDocumentDAO copiedDocumentDAO = new CopiedDocumentDAO(sqlConnection);
            List<Object> copiedDocuments = copiedDocumentDAO.GetCopiedDocuments(tenantId, dateRange.GetFirstDay(), dateRange.GetLastDay(), null, null);
            if (copiedDocuments.Count > 0)
            {
                NotifyListener("Já existiam registros inseridos anteriormente para a data especificada.");
                imported = true;
            }

            return imported;
        }

        /// <summary>
        /// Importa os registros do arquivo de log(.CSV) e insere no banco de dados, importa apenas
        /// os registros da data especificada em "logDate" (ignora a hora, minutos e segundos em logDate)
        /// </summary>
        public Boolean ImportFile(String fileName, DateTime logDate)
        {
            // Informações de trace são enviadas ao listener através de NotifyListener()
            // O listener grava essas informações em log de arquivo
            CSVReader reader = new CSVReader(fileName, listener);
            NotifyListener("Fazendo a leitura do CSV.");
            DataTable fullTable = reader.Read(0);
            int rowCount = fullTable.Rows.Count;

            // Verifica se existem registros no CSV
            if (rowCount < 1)
            {
                NotifyListener("CSV inválido. Nenhum registro encontrado.");
                return false;
            }

            // Informa a quantidade de registros no CSV e uma amostra de seu conteúdo
            NotifyListener("Quantidade de registros no CSV - " + rowCount);
            String sampleData = fullTable.Rows[0]["Date"].ToString() + " " +
                                fullTable.Rows[0]["Time"].ToString() + " - " +
                                fullTable.Rows[0]["User Name"].ToString() + " " +
                                fullTable.Rows[0]["Print Pages"].ToString() + " página(s)";
            NotifyListener("Amostra dos dados - " + sampleData);

            // Gera uma view do log apenas com as cópias
            DataView view = new DataView(fullTable, "Type='Copy'", null, DataViewRowState.Added);
            DataTable copiedDocumentTable = view.ToTable();

            // Obtem o nome da impressora a partir do nome do arquivo
            String printerName = Path.GetFileNameWithoutExtension(fileName);

            CopiedDocumentDAO copiedDocumentDAO = new CopiedDocumentDAO(sqlConnection);
            CopiedDocument copiedDocument;
            foreach (DataRow row in copiedDocumentTable.Rows)
            {
                copiedDocument = new CopiedDocument();
                copiedDocument.tenantId = tenantId;
                copiedDocument.jobTime = DateTime.Parse(row["Date"] + " " + row["Time"]);
                copiedDocument.userName = row["User Name"].ToString();
                copiedDocument.printerName = printerName;
                copiedDocument.pageCount = int.Parse(row["Print Pages"].ToString());
                copiedDocument.duplex = false;
                copiedDocument.color = false;

                // Insere no BD apenas se a data do registro for aquela que se deseja importar
                if (copiedDocument.jobTime.Date.CompareTo(logDate.Date) == 0)
                    copiedDocumentDAO.InsertCopiedDocument(copiedDocument);
            }

            return true;
        }

        private void NotifyListener(Object obj)
        {
            if (listener != null)
                listener.NotifyObject(obj);
        }
    }

}
