using System;
using System.Collections.Specialized;
using DocMageFramework.DataManipulation;


namespace DocMageFramework.JobExecution
{
    public interface IPeriodicTask
    {
        void InitializeTaskState(NameValueCollection taskParams, DataAccess dataAccess);
        void Execute();
    }

}
