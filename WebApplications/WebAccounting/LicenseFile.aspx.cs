using System;
using AccountingLib.Security;
using AccountingLib.Management;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    public partial class LicenseFile : System.Web.UI.Page
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!Authentication.IsAuthenticated(Session))
                Response.Redirect("LoginPage.aspx");

            Tenant tenant = (Tenant)Session["tenant"];
            if (tenant == null)
            {
                EmbedClientScript.ShowErrorMessage(this, "Sessão inválida.", true);
                return;
            }

            int licenseId;
            Boolean isNumeric = int.TryParse(Request.QueryString["licenseId"], out licenseId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            // Abre a conexão com o banco
            DataAccess dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(this.Page.Server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();

            ApplicationParamDAO appParamDAO = new ApplicationParamDAO(dataAccess.GetConnection());
            ApplicationParam urlParam = appParamDAO.GetParam("url", "webAccounting");

            // Fecha a conexão com o banco
            dataAccess.CloseConnection();
            dataAccess = null;

            if (urlParam == null)
            {
                EmbedClientScript.ShowErrorMessage(this, "Falha ao buscar url do sistema no banco.", true);
                return;
            }

            String serviceUrl = urlParam.value + "/JobRoutingService.aspx";

            DateTime oneYearFromNow = DateTime.Now.AddYears(1);
            DateTime expirationDate = new DateTime(oneYearFromNow.Year, oneYearFromNow.Month, oneYearFromNow.Day, 0, 0, 0);


            this.Response.Clear();
            this.Response.ContentType = "application/octet-stream";
            this.Response.AddHeader("content-disposition", "attachment; filename=ProductKey.bin");

            String licenseKey = LicenseKeyMaker.GenerateKey(serviceUrl, tenant.id, licenseId, expirationDate);
            Response.Write(licenseKey);

            this.Response.End();
        }
    }

}
