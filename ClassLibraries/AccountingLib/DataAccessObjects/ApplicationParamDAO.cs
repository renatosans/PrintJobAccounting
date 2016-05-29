using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.Specialized;
using AccountingLib.Entities;
using DocMageFramework.DataManipulation;


namespace AccountingLib.DataAccessObjects
{
    public class ApplicationParamDAO
    {
        private SqlConnection sqlConnection;

        private String currentParamName;

        private String currentOwnerTask;


        public ApplicationParamDAO(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }


        private Boolean CheckParamMethod(Object param)
        {
            ApplicationParam paramToCheck = (ApplicationParam)param;
            // Verifica se o nome do parametro bate
            if (paramToCheck.name.ToUpper() != currentParamName.ToUpper())
                return false;
            // Verifica se a tarefa a que ele pertence bate
            if (paramToCheck.ownerTask.ToUpper() != currentOwnerTask.ToUpper())
                return false;

            return true;
        }


        public ApplicationParam GetParam(String paramName, String ownerTask)
        {
            // Para identificar unicamente um parametro são necessários três campos:
            // [name], [applicationID], [ownerTask]
            // a procedure utilizada retorna somente parametros onde [applicationId] = ID("Print Accounting")
            // [name] e [ownerTask] são passados na chamada deste método
            ApplicationParam accountingParam;

            ProcedureCall retrieveAccountingParams = new ProcedureCall("pr_retrieveAccountingParams", sqlConnection);
            retrieveAccountingParams.Execute(true);
            List<Object> paramList = retrieveAccountingParams.ExtractFromResultset(typeof(ApplicationParam));
            this.currentParamName = paramName;
            this.currentOwnerTask = ownerTask;
            accountingParam = (ApplicationParam)paramList.Find(CheckParamMethod);

            return accountingParam;
        }


        public List<Object> GetAllParams()
        {
            List<Object> accountingParams;

            ProcedureCall retrieveAccountingParams = new ProcedureCall("pr_retrieveAccountingParams", sqlConnection);
            retrieveAccountingParams.Execute(true);
            accountingParams = retrieveAccountingParams.ExtractFromResultset(typeof(ApplicationParam));

            return accountingParams;
        }


        public NameValueCollection GetTaskParams(String task)
        {
            NameValueCollection paramCollection = new NameValueCollection();

            List<Object> paramList = GetAllParams();
            foreach (ApplicationParam param in paramList)
            {
                String ownerTask = param.ownerTask.ToUpper();
                if (task.ToUpper() == ownerTask)
                    paramCollection.Add(param.name, param.value);
            }

            return paramCollection;
        }


        public Dictionary<String, NameValueCollection> GetParamsGroupByTask()
        {
            Dictionary<String, NameValueCollection> paramCollectionDictionary = new Dictionary<String, NameValueCollection>();

            List<Object> paramList = GetAllParams();
            String ownerTask;
            NameValueCollection paramCollection;
            foreach (ApplicationParam param in paramList)
            {
                ownerTask = param.ownerTask;
                if (paramCollectionDictionary.ContainsKey(ownerTask))
                {
                    paramCollection = paramCollectionDictionary[ownerTask];
                }
                else
                {
                    paramCollection = new NameValueCollection();
                    paramCollectionDictionary.Add(ownerTask, paramCollection);
                }

                paramCollection.Add(param.name, param.value);
            }

            return paramCollectionDictionary;
        }


        public void SetParam(ApplicationParam param)
        {
            ProcedureCall updateAccountingParam = new ProcedureCall("pr_storeAccountingParam", sqlConnection);
            updateAccountingParam.parameters.Add(new ProcedureParam("@paramId", SqlDbType.Int, 4, param.id));
            updateAccountingParam.parameters.Add(new ProcedureParam("@name", SqlDbType.VarChar, 100, param.name));
            updateAccountingParam.parameters.Add(new ProcedureParam("@value", SqlDbType.VarChar, 255, param.value));
            updateAccountingParam.parameters.Add(new ProcedureParam("@ownerTask", SqlDbType.VarChar, 100, param.ownerTask));
            updateAccountingParam.Execute(false);
        }
    }

}
