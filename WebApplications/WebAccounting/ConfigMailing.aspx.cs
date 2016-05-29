using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.ReportMailing;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;
using DocMageFramework.DataManipulation;
using DocMageFramework.CustomAttributes;


namespace WebAccounting
{
    public partial class ConfigMailing : System.Web.UI.Page
    {
        private AccountingMasterPage accountingMasterPage;

        private DataAccess dataAccess;

        private MailingDAO mailingDAO;


        protected void Page_Load(object sender, EventArgs e)
        {
            accountingMasterPage = (AccountingMasterPage)Page.Master;
            accountingMasterPage.InitializeMasterPageComponents();
            dataAccess = accountingMasterPage.dataAccess;


            // action:
            //    null -  Sem ação, apenas lista os mailings
            //    0    -  Excluir mailing, lista os restantes
            //    1    -  Teste execução do mailing
            int? action = null;
            int? mailingId = null;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["action"]))
                    action = int.Parse(Request.QueryString["action"]);

                if (!String.IsNullOrEmpty(Request.QueryString["mailingId"]))
                    mailingId = int.Parse(Request.QueryString["mailingId"]);
            }
            catch (System.FormatException)
            {
                // Remove todos os controles da página
                configurationArea.Controls.Clear();
                controlArea.Controls.Clear();

                // Mostra aviso de inconsistência nos parâmetros
                WarningMessage.Show(controlArea, ArgumentBuilder.GetWarning());
                return;
            }

            Tenant tenant = (Tenant)Session["tenant"];
            mailingDAO = new MailingDAO(dataAccess.GetConnection());

            if (mailingId != null)
                switch (action)
                {
                    case 0:
                        mailingDAO.RemoveMailing(mailingId.Value);
                        Response.Redirect("ConfigMailing.aspx"); // Limpa a QueryString para evitar erros
                        break;
                    case 1:
                        Mailing mailing = mailingDAO.GetMailing(tenant.id, mailingId);
                        TestMailing(mailing);
                        break;
                    default:
                        break;
                }

            List<Object> mailingList = mailingDAO.GetAllMailings(tenant.id);

            String[] columnNames = new String[] { "Frequência de envio", "Relatório", "Destinatários" };
            //String testScript = "window.location='ConfigMailing.aspx?action=1&mailingId=' + {0};";
            String alterScript = "window.open('MailingSettings.aspx?mailingId=' + {0}, 'Settings', 'width=540,height=600');";
            String removeScript = "var confirmed = confirm('Deseja realmente excluir este item?'); if (confirmed) window.location='ConfigMailing.aspx?action=0&mailingId=' + {0};";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                //new EditableListButton("Testar", testScript, ButtonTypeEnum.Execute),
                new EditableListButton("Editar", alterScript, ButtonTypeEnum.Edit),
                new EditableListButton("Excluir", removeScript, ButtonTypeEnum.Remove)
            };
            EditableList editableList = new EditableList(configurationArea, columnNames, buttons);
            foreach (Mailing mailing in mailingList)
            {
                ReportFrequencyEnum frequency = (ReportFrequencyEnum)mailing.frequency;
                ReportTypeEnum reportType = (ReportTypeEnum)mailing.reportType;

                String[] mailingProperties = new String[]
                {
                    AssociatedText.GetFieldDescription(typeof(ReportFrequencyEnum), frequency.ToString()),
                    AssociatedText.GetFieldDescription(typeof(ReportTypeEnum), reportType.ToString()),
                    mailing.recipients
                };
                // A lista de mailings não possui item default, isDefaultItem é sempre "false"
                editableList.InsertItem(mailing.id, false, mailingProperties);
            }
            editableList.DrawList();

            // O clique do botão chama o script de alteração passando "id = 0", a tela de alteração
            // interpreta "id = 0" como "criar um novo", o id é então gerado no banco de dados.
            btnNovo.Attributes.Add("onClick", String.Format(alterScript, 0));
        }


        private void TestMailing(Mailing mailing)
        {
            // not implemented yet
        }
    }

}
