using System;


namespace AccountingLib.ServerPrintLog
{
    public class StopNotification
    {
        private String stopReason;

        public String Reason
        {
            get { return stopReason; }
        }


        public StopNotification(String stopReason)
        {
            this.stopReason = stopReason;
        }
    }

}
