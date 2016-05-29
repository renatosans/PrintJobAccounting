using System;
using System.ServiceProcess;


namespace CopyLogImporter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new CopyLogImport() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }

}
