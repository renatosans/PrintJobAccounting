using System;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAccounting
{
    public partial class CostCenterSettings : System.Web.UI.Page
    {
        private SettingsMasterPage settingsMasterPage;

        private int? costCenterId = null; // recebe na QueryString

        private int? parentId = null; // recebe na QueryString


        // Constroi o centro de custo com os parametros recebidos na QueryString
        private CostCenter GetCostCenter()
        {
            CostCenter costCenter = null;
            Tenant tenant = (Tenant)Session["tenant"];
            
            // Alteração de um centro de custo existente
            if (costCenterId != null)
            {
                CostCenterDAO costCenterDAO = new CostCenterDAO(settingsMasterPage.dataAccess.GetConnection());
                costCenter = costCenterDAO.GetCostCenter(tenant.id, costCenterId.Value);
            }
            
            // Inclusão de um novo centro de custo
            if (parentId != null)
            {
                costCenter = new CostCenter();
                costCenter.parentId = parentId.Value;
                costCenter.tenantId = tenant.id;
            }


            return costCenter;
        }

        protected void Page_Load(Object sender, EventArgs e)
        {
            settingsMasterPage = (SettingsMasterPage)Page.Master;
            settingsMasterPage.InitializeMasterPageComponents();

            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["costCenterId"]))
                    costCenterId = int.Parse(Request.QueryString["costCenterId"]);

                if (!String.IsNullOrEmpty(Request.QueryString["parentId"]))
                    parentId = int.Parse(Request.QueryString["parentId"]);
            }
            catch (System.FormatException)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.Add("txtName", "Nome", GetCostCenter().name);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            String costCenterName = null;
            foreach (String fieldName in Request.Form)
            {
                if (fieldName.Contains("txtName"))
                    costCenterName = Request.Form[fieldName];
            }
            if (String.IsNullOrEmpty(costCenterName))
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }

            CostCenter costCenter = GetCostCenter();
            costCenter.name = costCenterName;

            try
            {
                CostCenterDAO costCenterDAO = new CostCenterDAO(settingsMasterPage.dataAccess.GetConnection());
                costCenterDAO.SetCostCenter(costCenter);
            }
            catch (Exception genericException)
            {
                if (genericException.Message.Contains("Violation of UNIQUE KEY"))
                {
                    EmbedClientScript.ShowErrorMessage(this, "Este centro de custo já existe!");
                    return;
                }

                EmbedClientScript.ShowErrorMessage(this, genericException.Message);
            }

            EmbedClientScript.CloseWindow(this);
        }
    }

}
