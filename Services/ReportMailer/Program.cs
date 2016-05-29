using System;
using System.Reflection;
using System.ServiceProcess;
using DocMageFramework.JobExecution;


namespace ReportMailer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(String[] args)
        {
            String serviceName = "Report Mailer";
            String serviceLocation = Assembly.GetExecutingAssembly().Location;

            foreach (String argument in args)
            {
                if (argument.ToUpper().Contains("/INSTALL"))
                {
                    // Se não estiver instalado na máquina faz sua instalação
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
				new ReportMailing() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }

}
