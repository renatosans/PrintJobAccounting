using System;


namespace AccountingLib.Spool
{
    public class JobNotification
    {
        public JobNotificationTypeEnum NotificationType;

        public String JobName;


        public JobNotification(JobNotificationTypeEnum notificationType, String jobName)
        {
            this.NotificationType = notificationType;
            this.JobName = jobName;
        }
    }

}
