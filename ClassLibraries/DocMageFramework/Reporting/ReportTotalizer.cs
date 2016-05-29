using System;
using System.Drawing;
using System.Collections.Generic;


namespace DocMageFramework.Reporting
{
    public class ReportTotalizer
    {
        private Object[] totalizerArray;


        public ReportTotalizer(int columnCount)
        {
            totalizerArray = new Object[columnCount];
        }

        public void SetTotalAsZero(int totalizerIndex, ReportCellType cellType)
        {
            switch (cellType)
            {
                case ReportCellType.Money:
                    totalizerArray[totalizerIndex] = (decimal)0;
                    break;
                case ReportCellType.Percentage:
                    totalizerArray[totalizerIndex] = (double)0;
                    break;
                default: // por defalut considera o conteudo da célula como número
                    totalizerArray[totalizerIndex] = (int)0;
                    break;
            }
        }

        public void IncTotal(int totalizerIndex, Object value, ReportCellType cellType)
        {
            if (totalizerArray[totalizerIndex] == null)
            {
                totalizerArray[totalizerIndex] = value;
                return;
            }

            switch (cellType)
            {
                case ReportCellType.Money:
                    totalizerArray[totalizerIndex] = (decimal)totalizerArray[totalizerIndex] + (decimal)value;
                    return;
                case ReportCellType.Percentage:
                    totalizerArray[totalizerIndex] = (double)totalizerArray[totalizerIndex] + (double)value;
                    return;
                default: // por defalut considera o conteudo da célula como número
                    totalizerArray[totalizerIndex] = (int)totalizerArray[totalizerIndex] + (int)value;
                    return;
            }
        }

        public String GetTotal(int totalizerIndex, ReportCellType cellType)
        {
            if (totalizerArray[totalizerIndex] == null)
            {
                // Seta zero como total pois não há nenhuma linha na tabela
                SetTotalAsZero(totalizerIndex, cellType);
            }

            switch (cellType)
            {
                case ReportCellType.Money:
                    return String.Format("R$ {0:0.000}", totalizerArray[totalizerIndex]);
                case ReportCellType.Percentage:
                    return String.Format("{0:0.##}%", (double)totalizerArray[totalizerIndex] * 100);
                default:
                    // por defalut considera o conteudo da célula como número
                    return String.Format("{0}", totalizerArray[totalizerIndex]);
            }
        }

        private static ReportCellType[] GetFieldTypes(Type recordType, String[] fieldNames)
        {
            ReportCellType[] fieldTypes = new ReportCellType[fieldNames.Length];
            Object recordSample = Activator.CreateInstance(recordType);
            for (int index = 0; index < fieldNames.Length; index++)
            {
                Object fieldValue = recordType.GetField(fieldNames[index]).GetValue(recordSample);
                ReportCellType fieldType = ReportCellType.Text;
                if (fieldValue is decimal)
                    fieldType = ReportCellType.Money;
                if (fieldValue is double)
                    fieldType = ReportCellType.Percentage;
                if (fieldValue is int)
                    fieldType = ReportCellType.Number;

                fieldTypes[index] = fieldType;
            }

            return fieldTypes;
        }

        public static ReportCell[] GetSummary(List<Object> reportRecords, Type recordType, String[] fieldNames)
        {
            ReportCell[] summary = new ReportCell[fieldNames.Length];
            summary[0] = new ReportCell("TOTAL", Color.Red);
            ReportTotalizer reportTotalizer = new ReportTotalizer(fieldNames.Length);
            ReportCellType[] fieldTypes = GetFieldTypes(recordType, fieldNames);

            foreach (Object record in reportRecords)
            {
                for (int index = 0; index < fieldNames.Length; index++)
                {
                    Object fieldValue = recordType.GetField(fieldNames[index]).GetValue(record);
                    ReportCellType fieldType = fieldTypes[index];

                    if (fieldType != ReportCellType.Text)
                        reportTotalizer.IncTotal(index, fieldValue, fieldType);
                }
            }

            // O índice "0" já foi preenchido com o rótulo "TOTAL"
            for (int index = 1; index < fieldNames.Length; index++)
            {
                if (fieldTypes[index] == ReportCellType.Text)
                    summary[index] = new ReportCell("", Color.Red);
                else
                    summary[index] = new ReportCell(reportTotalizer.GetTotal(index, fieldTypes[index]), Color.Red);
            }

            return summary;
        }
    }

}
