using System;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.Generic;
using AccountingLib.Printers;
using DocMageFramework.AppUtils;


namespace AccountingLib.Spool
{
    /// <summary>
    /// Classe que monitora os trabalhos de impressão. Ela intercepta a criação, modificação e
    /// exclusão de arquivos no spool. Para identificar unicamente cada job utiliza seu nome
    /// pois o jobId é único apenas dentro de uma fila, com várias filas rodando em paralelo
    /// um jobId pode não ser único. O jobName por sua vez é composto "PrinterName, JobId" o
    /// que dificulta sua repetição.
    /// </summary>
    public class SpoolMonitor
    {
        // Spool de jobs de impressão, dicionário com pares < JobName , SpooledJob >
        // o monitor atualiza a lista quando um novo trabalho de impressão é submetido
        private Dictionary<String, SpooledJob> spool = new Dictionary<String, SpooledJob>();

        // Mapa do caminho dos Shadow files e os respectivos Job names
        private Dictionary<String, String> fileMap = new Dictionary<String, String>();

        // Objeto que será notificado sobre os arquivos de spool
        private IListener listener;

        /// <summary>
        /// Constroi uma instancia da classe SpoolMonitor. Inicia o monitoramento ( intercepta
        /// arquivos Shadow (*.SHD) criados/modificados/excluidos no diretório de spool )
        /// </summary>
        public SpoolMonitor(IListener listener)
        {
            this.listener = listener;

            FileSystemWatcher fileWatcher = new FileSystemWatcher(DeviceHandler.GetSpoolDirectory(), "*.shd");
            fileWatcher.IncludeSubdirectories = false;
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            fileWatcher.Created += new FileSystemEventHandler(SpoolFileCreated);
            fileWatcher.Changed += new FileSystemEventHandler(SpoolFileChanged);
            fileWatcher.Deleted += new FileSystemEventHandler(SpoolFileDeleted);
            fileWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Busca um job no dicionário/spool, caso não encontre retorna null
        /// </summary>
        public SpooledJob FindSpooledJob(String jobName)
        {
            if (!spool.ContainsKey(jobName)) return null;
            return spool[jobName];
        }

        private void SpoolFileCreated(Object sender, FileSystemEventArgs e)
        {
            // todo: abrir thread (BackgroundWorker) para tratamento da ação

            SpooledJob spooledJob = new SpooledJob(e.FullPath, listener);
            if (spooledJob.ShadowFile == null) return;

            String jobName = spooledJob.ShadowFile.PrinterName + ", " + spooledJob.ShadowFile.JobId.ToString();
            if (!spool.ContainsKey(jobName))
            {
                fileMap.Add(e.FullPath, jobName);
                spool.Add(jobName, spooledJob);

                // Notifica a inserção de um novo job no spool
                if (listener != null)
                    listener.NotifyObject(new JobNotification(JobNotificationTypeEnum.JobCreated, jobName));
            }
        }

        private void SpoolFileChanged(Object sender, FileSystemEventArgs e)
        {
            // todo: abrir thread (BackgroundWorker) para tratamento da ação

            if (fileMap.ContainsKey(e.FullPath))
            {
                String jobName = fileMap[e.FullPath];
                SpooledJob spooledJob = spool[jobName];
                // Reseta a instancia para obter dados atualizados
                spooledJob.ResetInstance();

                // Notifica a alteração de um job no spool
                if (listener != null)
                    listener.NotifyObject(new JobNotification(JobNotificationTypeEnum.JobChanged, jobName));
            }
        }

        private void SpoolFileDeleted(Object sender, FileSystemEventArgs e)
        {
            // todo: abrir thread (BackgroundWorker) para tratamento da ação

            if (fileMap.ContainsKey(e.FullPath))
            {
                String jobName = fileMap[e.FullPath];
                spool.Remove(jobName);

                // Notifica a remoção de um job do spool
                if (listener != null)
                    listener.NotifyObject(new JobNotification(JobNotificationTypeEnum.JobDeleted, jobName));
            }
        }
    }

}
