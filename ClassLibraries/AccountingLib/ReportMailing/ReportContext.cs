using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using DocMageFramework.Reporting;
using DocMageFramework.FileUtils;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.ReportMailing
{
    public static class ReportContext
    {
        /// <summary>
        /// Verifica se o horário corrente (DateTime.Now) bate com o horário agendado (conhecido
        /// pela frequência/periodicidade de envio)
        /// </summary>
        public static Boolean IsScheduledTime(ReportFrequencyEnum reportFrequency, int periodEndDate)
        {
            DateTime now = DateTime.Now;

            switch (reportFrequency)
            {
                // Os relatórios são gerados no inicio do dia ( 01:00 horas )
                // não exatamente a meia noite pois o serviço importador precisa importar
                // os logs primeiro

                case ReportFrequencyEnum.Daily:
                    // 01:00 horas (inicio do dia)
                    return (now.Hour == 1);

                case ReportFrequencyEnum.Weekly:
                    // primeiro dia da semana (domingo)
                    return ((now.DayOfWeek == DayOfWeek.Sunday) && (now.Hour == 1));

                default:
                    // relatório mensal por default, gerado no dia do mês definido por periodEndDate
                    return ((now.Day == periodEndDate) && (now.Hour == 1));
            }
        }

        /// <summary>
        /// Obtem a faixa de datas de acordo com a frequência do relatório
        /// </summary>
        public static DateRange GetDateRange(ReportFrequencyEnum reportFrequency)
        {
            DateTime startDate;
            DateTime endDate = DateTime.Now.Date;

            switch (reportFrequency)
            {
                case ReportFrequencyEnum.Daily:
                    startDate = endDate.AddDays(-1);
                    break;
                case ReportFrequencyEnum.Weekly:
                    startDate = endDate.AddDays(-7);
                    break;
                default:
                    startDate = endDate.AddMonths(-1);
                    break;
            }

            DateRange dateRange = new DateRange(true);
            dateRange.SetRange(startDate, endDate);

            return dateRange;
        }

        /// <summary>
        /// Tenta remover os arquivos temporários de relatório
        /// </summary>
        public static void TryRemoveTempFiles()
        {
            String reportDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            DirectoryInfo dirInfo = new DirectoryInfo(reportDir);

            List<FileInfo> filesToRemove = new List<FileInfo>();
            filesToRemove.AddRange(dirInfo.GetFiles("Report*.pdf"));
            filesToRemove.AddRange(dirInfo.GetFiles("Report*.xls"));
            filesToRemove.AddRange(dirInfo.GetFiles("Report*.csv"));

            foreach (FileInfo fileInfo in filesToRemove)
            {
                // Tenta remover o arquivo, igonora erros caso esteja aberto/em uso
                String filePath = PathFormat.Adjust(reportDir) + fileInfo.Name;
                FileResource.TryDelete(filePath);
            }
        }

        /// <summary>
        /// Obtem o texto associado a frequência de envio do relatório
        /// </summary>
        public static String GetFrequencyCaption(ReportFrequencyEnum reportFrequency)
        {
            String frequency = reportFrequency.ToString();
            frequency = AssociatedText.GetFieldDescription(typeof(ReportFrequencyEnum), frequency);

            return frequency;
        }
    }

}
