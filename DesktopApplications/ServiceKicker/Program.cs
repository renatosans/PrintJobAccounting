using System;
using System.Threading;
using System.Windows.Forms;
using DocMageFramework.Parsing;
using DocMageFramework.JobExecution;


namespace ServiceKicker
{
    static class Program
    {
        private static void KickService(String serviceName, int waitBeforeStop, int waitBeforeStart)
        {
            String errors = null;
            if (!ServiceHandler.ServiceExists(serviceName)) return;

            try
            {
                Thread.Sleep(waitBeforeStop);
                ServiceHandler.StopService(serviceName, 33000);

                UpdateHandler updateHandler = new UpdateHandler();
                updateHandler.UpdateSoftwareProduct();

                Thread.Sleep(waitBeforeStart);
                ServiceHandler.StartService(serviceName, 33000);
            }
            catch (Exception exc)
            {
                errors = exc.Message;
            }
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)        
        {
            String serviceName = null;
            int waitBeforeStop = 0;
            int waitBeforeStart = 0;

            foreach(String argument in args)
            {
                if (argument.ToUpper().Contains("/SERVICE:"))
                {
                    serviceName = ArgumentParser.GetValue(argument);
                }

                if (argument.ToUpper().Contains("/WAITBEFORESTOP:"))
                {
                    String argumentValue = ArgumentParser.GetValue(argument);
                    Boolean isNumeric = int.TryParse(argumentValue, out waitBeforeStop);
                    if (!isNumeric) waitBeforeStop = 0;
                }

                if (argument.ToUpper().Contains("/WAITBEFORESTART:"))
                {
                    String argumentValue = ArgumentParser.GetValue(argument);
                    Boolean isNumeric = int.TryParse(argumentValue, out waitBeforeStart);
                    if (!isNumeric) waitBeforeStart = 0;
                }
            }

            if (serviceName != null)
            {
                KickService(serviceName, waitBeforeStop, waitBeforeStart);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

}
