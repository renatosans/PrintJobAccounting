using System;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAdministrator
{
    public partial class AddLicenses : System.Web.UI.Page
    {
        private AdministratorMasterPage administratorMasterPage;


        protected void Page_Load(Object sender, EventArgs e)
        {
            administratorMasterPage = (AdministratorMasterPage)Page.Master;
            administratorMasterPage.InitializeMasterPageComponents();

            int tenantId;
            Boolean isNumeric = int.TryParse(Request.QueryString["tenantId"], out tenantId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.AddHidden("txtId", tenantId.ToString());
            settingsInput.Add("txtAmmount", "Quantidade", "1");
        }

        protected void btnAdd_Click(Object sender, EventArgs e)
        {
            LicenseDAO linceseDAO = new LicenseDAO(administratorMasterPage.dataAccess.GetConnection());
            License license = new License();
            license.id = 0; // adicionar uma nova licença
            String ammount = "";

            foreach (String fieldName in Request.Form)
            {
                if (fieldName.Contains("txtId"))
                    license.tenantId = int.Parse(Request.Form[fieldName]);
                if (fieldName.Contains("txtAmmount"))
                    ammount = Request.Form[fieldName];
            }

            int licenseAmmount = 0;
            Boolean isNumeric = int.TryParse(ammount, out licenseAmmount);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }

            if (licenseAmmount < 1)
            {
                EmbedClientScript.ShowErrorMessage(this, "Quantidade inválida!");
                return;
            }

            for (int index = 0; index < licenseAmmount; index++)
            {
                linceseDAO.SetLicense(license);
            }

            EmbedClientScript.CloseWindow(this);
        }
    }

}
