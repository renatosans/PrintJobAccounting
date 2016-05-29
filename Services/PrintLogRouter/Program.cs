using System;
using System.Threading;
using System.Reflection;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.ServerPrintLog;
using DocMageFramework.JobExecution;


namespace PrintLogRouter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(String[] args)
        {
            String serviceName = "Print Log Router";
            String serviceLocation = Assembly.GetExecutingAssembly().Location;

            foreach (String argument in args)
            {
                if (argument.ToUpper().Contains("/RUNONCE"))
                {
                    // Busca os parâmetros de execução no XML, adicionar XML ao diretório bin/Debug para depurar
                    NameValueCollection taskParams = PrintLogContext.GetTaskParams();
                    double interval = 59000;

                    List<IPeriodicTask> taskList = new List<IPeriodicTask>();
                    taskList.Add(new DeviceDiscoverer());
                    taskList.Add(new PrintLogRoutingTask());
                    JobController jobController = new JobController(taskList, taskParams, null, interval);
                    jobController.Start();

                    Thread.Sleep(119000);
                    return;
                }

                if (argument.ToUpper().Contains("/INSTALL"))
                {
                    // Se não estiver instalado na máquina faz a instalação do serviço
                    if (!ServiceHandler.ServiceExists(serviceName))
                        ServiceHandler.InstallService(serviceLocation);

                    // Se não estiver em execução, inicia o serviço
                    ServiceController serviceController = new ServiceController(serviceName);
                    if (serviceController.Status != ServiceControllerStatus.Running)
                        ServiceHandler.StartService(serviceName, 33000);

                    return;
                }

                if (argument.ToUpper().Contains("/UNINSTALL"))
                {
                    // Se não estiver instalado na máquina não é preciso fazer nada
                    if (!ServiceHandler.ServiceExists(serviceName)) return;

                    // Se estiver em execução, para o serviço
                    ServiceController serviceController = new ServiceController(serviceName);
                    if (serviceController.Status == ServiceControllerStatus.Running)
                        ServiceHandler.StopService(serviceName, 33000);

                    // Faz a remoção do serviço
                    ServiceHandler.UninstallService(serviceLocation);

                    return;
                }
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
			{ 
				new PrintLogRouting() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }

}
