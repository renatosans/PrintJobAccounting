using System;
using System.Data;
using AccountingLib.Entities;


namespace AccountingLib.ServerCopyLog
{
    public class CopyLogDeviceHandler
    {
        private CopyLogDevice device;


        public CopyLogDeviceHandler(CopyLogDevice device)
        {
            this.device = device;
        }

        public Boolean ProcessCopyLog(DataTable logData, CopyLogSender copyLogSender, int tenantId)
        {
            int logType;
            Boolean isNumeric = int.TryParse(device.logType, out logType);
            if (!isNumeric) return false;

            switch (logType)
            {
                case 1:
                    // Gera uma view do log apenas com as cópias
                    String rowFilter = "Type='Copy'";
                    DataView view = new DataView(logData, rowFilter, null, DataViewRowState.Added);
                    DataTable copiedDocumentTable = view.ToTable();

                    // Define uma faixa de horário para filtrar os registros mais recentes
                    DateTime startHour = DateTime.Now.AddHours(-1);
                    DateTime endHour = DateTime.Now.AddHours(+1);

                    CopiedDocument copiedDocument;
                    foreach (DataRow row in copiedDocumentTable.Rows)
                    {
                        copiedDocument = new CopiedDocument();
                        copiedDocument.tenantId = tenantId;
                        copiedDocument.jobTime = DateTime.Parse(row["Date"] + " " + row["Time"]);
                        copiedDocument.userName = row["User Name"].ToString();
                        copiedDocument.printerName = device.printerName;
                        copiedDocument.pageCount = int.Parse(row["Print Pages"].ToString());
                        copiedDocument.duplex = false;
                        copiedDocument.color = false;

                        if ((copiedDocument.jobTime > startHour) && (copiedDocument.jobTime < endHour))
                            copyLogSender.AddCopiedDocument(copiedDocument);
                    }
                    break;
                case 2:
                    break;
                default:
                    return false;
            }

            return true;
        }
    }

}
