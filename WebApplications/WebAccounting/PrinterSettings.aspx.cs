using System;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAccounting
{
    public partial class PrinterSettings : System.Web.UI.Page
    {
        private SettingsMasterPage settingsMasterPage;

        private PrinterDAO printerDAO;


        protected void Page_Load(object sender, EventArgs e)
        {
            settingsMasterPage = (SettingsMasterPage)Page.Master;
            settingsMasterPage.InitializeMasterPageComponents();

            int printerId;
            Boolean isNumeric = int.TryParse(Request.QueryString["printerId"], out printerId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            Tenant tenant = (Tenant)Session["tenant"];
            printerDAO = new PrinterDAO(settingsMasterPage.dataAccess.GetConnection());
            Printer printer = printerDAO.GetPrinter(tenant.id, printerId);
            // Não permite a criação de impressoras pois o usuário não pode fazer isso diretamente
            if (printer == null)
            {
                EmbedClientScript.ShowErrorMessage(this, "Falha ao obter os dados da impressora.", true);
                return;
            }

            lblTitle.Text = "Dados da impressora " + printer.name;
            SettingsInput settingsInput = new SettingsInput(settingsArea, null);
            settingsInput.AddHidden("txtId", printer.id.ToString());
            settingsInput.AddHidden("txtTenantId", printer.tenantId.ToString());
            settingsInput.AddHidden("txtName", printer.name);
            settingsInput.Add("txtAlias", "Nome amigável", printer.alias);
            settingsInput.Add("txtBwPageCost", "Custo página Pb", String.Format("{0:0.000}", printer.pageCost));
            settingsInput.Add("txtColorPageCost", "Custo página Cor", String.Format("{0:0.000}", printer.pageCost + printer.colorCostDiff));
            settingsInput.AddCheckBox("chkBwPrinter", "Monocromática", printer.bwPrinter);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Printer printer = new Printer();
            Decimal colorPageCost = 0;
            Boolean blackAndWhitePrinter = false;
            try
            {
                foreach (String fieldName in Request.Form)
                {
                    if (fieldName.Contains("txtId"))
                        printer.id = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtTenantId"))
                        printer.tenantId = int.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtName"))
                        printer.name = Request.Form[fieldName];
                    if (fieldName.Contains("txtAlias"))
                    {
                        printer.alias = Request.Form[fieldName];
                        if (String.IsNullOrEmpty(printer.alias))
                            throw new FormatException();
                    }
                    if (fieldName.Contains("txtBwPageCost"))
                        printer.pageCost = Decimal.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("txtColorPageCost"))
                        colorPageCost = Decimal.Parse(Request.Form[fieldName]);
                    if (fieldName.Contains("chkBwPrinter"))
                        blackAndWhitePrinter = true; // o checkbox só é enviado no Post se estiver marcado
                }
                printer.colorCostDiff = colorPageCost - printer.pageCost;
                printer.bwPrinter = blackAndWhitePrinter;
            }
            catch (System.FormatException)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os valores informados não estão em um formato válido!");
                return;
            }
            
            printerDAO.SetPrinter(printer);
            EmbedClientScript.CloseWindow(this);
        }
    }

}
