using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAccounting
{
    public partial class PageCounterHistory : System.Web.UI.Page
    {
        private SettingsMasterPage settingsMasterPage;

        private PrintingDeviceDAO printingDeviceDAO;


        protected void Page_Load(object sender, EventArgs e)
        {
            settingsMasterPage = (SettingsMasterPage)Page.Master;
            settingsMasterPage.InitializeMasterPageComponents();

            int deviceId;
            Boolean isNumeric = int.TryParse(Request.QueryString["deviceId"], out deviceId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            printingDeviceDAO = new PrintingDeviceDAO(settingsMasterPage.dataAccess.GetConnection());
            List<Object> counterHistory = printingDeviceDAO.GetCounterHistory(deviceId);

            String[] columnNames = new String[] { "Contador", "Data" };
            EditableList editableList = new EditableList(displayArea, columnNames, null);
            foreach(Object counter in counterHistory)
            {
                PageCounter pageCounter = (PageCounter)counter;
                String[] counterProperties = new String[]
                {
                    pageCounter.counter.ToString(),
                    String.Format("{0:dd/MM/yyyy HH:mm}", pageCounter.date)
                };
                editableList.InsertItem(pageCounter.id, false, counterProperties);
            }
            editableList.DrawList();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            EmbedClientScript.CloseWindow(this);
        }
    }

}
