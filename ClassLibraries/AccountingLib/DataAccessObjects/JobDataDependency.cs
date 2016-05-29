using System;
using System.Reflection;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;
using AccountingLib.Entities;


namespace AccountingLib.DataAccessObjects
{
    public class JobDataDependency
    {
        private IList jobList;

        private SqlConnection sqlConnection;

        private int tenantId;

        private int? mainCostCenterId;


        /// <summary>
        /// Construtor da classe, ao ser instanciada recebe uma lista de jobs de impressão/cópia
        /// para posteriormente gerar as dependências no BD
        /// </summary>
        public JobDataDependency(IList jobList, SqlConnection sqlConnection, int tenantId)
        {
            this.jobList = jobList;
            this.sqlConnection = sqlConnection;
            this.tenantId = tenantId;
        }

        /// <summary>
        /// Cria as dependências dos job de impressão/cópia (foreign keys para as tabelas de usuário e impressora)
        /// </summary>
        public void CreateDataDependency()
        {
            if (jobList.Count == 0) return;

            Type jobType = jobList[0].GetType();
            FieldInfo userNameField = jobType.GetField("userName");
            FieldInfo printerNameField = jobType.GetField("printerName");

            List<String> userNames = new List<String>();
            List<String> printerNames = new List<String>();
            foreach(Object job in jobList)
            {
                String userName = (String)userNameField.GetValue(job);
                String printerName = (String)printerNameField.GetValue(job);

                if (!userNames.Contains(userName)) userNames.Add(userName); // evita duplicações
                if (!printerNames.Contains(printerName)) printerNames.Add(printerName); // evita duplicações
            }

            CostCenterDAO costCenterDAO = new CostCenterDAO(sqlConnection);
            CostCenter mainCostCenter = costCenterDAO.GetMainCostCenter(tenantId);
            mainCostCenterId = null;
            if (mainCostCenter != null) mainCostCenterId = mainCostCenter.id;

            UserDAO userDAO = new UserDAO(sqlConnection);
            List<Object> users = userDAO.GetAllUsers(tenantId);
            foreach (String userName in userNames)
            {
                CreateUser(userName, userDAO, users);
            }

            PrinterDAO printerDAO = new PrinterDAO(sqlConnection);
            List<Object> printers = printerDAO.GetAllPrinters(tenantId);
            foreach (String printerName in printerNames)
            {
                CreatePrinter(printerName, printerDAO, printers);
            }
        }

        private void CreateUser(String userName, UserDAO userDAO, List<Object> users)
        {
            // Sai do método caso o usuário já exista no banco de dados
            foreach (User user in users)
            {
                if (user.name.ToUpper() == userName.ToUpper()) return;
            }

            User newUser = new User(tenantId, userName, userName);
            int? userId = userDAO.SetUser(newUser);

            // Tenta associar o usuário ao centro de custo principal (centro de custo raiz)
            if ((mainCostCenterId == null) || (userId == null)) return;
            CostCenterAssociate associate = new CostCenterAssociate(tenantId, mainCostCenterId.Value, userId.Value);
            CostCenterAssociateDAO associateDAO = new CostCenterAssociateDAO(sqlConnection);
            associateDAO.SetAssociate(associate);
        }

        private void CreatePrinter(String printerName, PrinterDAO printerDAO, List<Object> printers)
        {
            // Sai do método caso a impressora já exista no banco de dados
            foreach (Printer printer in printers)
            {
                if (printer.name.ToUpper() == printerName.ToUpper()) return;
            }

            Printer newPrinter = new Printer(tenantId, printerName, printerName);
            printerDAO.SetPrinter(newPrinter);
        }
    }

}
