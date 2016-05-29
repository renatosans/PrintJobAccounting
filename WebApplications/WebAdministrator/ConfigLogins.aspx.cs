using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;
using DocMageFramework.Reflection;
using DocMageFramework.DataManipulation;


namespace WebAdministrator
{
    public partial class ConfigLogins : System.Web.UI.Page
    {
        AdministratorMasterPage administratorMasterPage;

        DataAccess dataAccess;


        protected void Page_Load(Object sender, EventArgs e)
        {
            administratorMasterPage = (AdministratorMasterPage)Page.Master;
            administratorMasterPage.AddMenuItems();
            administratorMasterPage.InitializeMasterPageComponents();
            dataAccess = administratorMasterPage.dataAccess;

            AdministratorLoginDAO administratorLoginDAO = new AdministratorLoginDAO(dataAccess.GetConnection());
            List<Object> loginList = administratorLoginDAO.GetAllLogins();
            String[] columnNames = new String[] { "Login" };
            String alterScript = "window.open('LoginSettings.aspx?loginId=' + {0}, 'Settings', 'width=540,height=600');";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                new EditableListButton("Editar", alterScript, ButtonTypeEnum.Edit),
            };
            EditableList editableList = new EditableList(configurationArea, columnNames, buttons);
            foreach (AdministratorLogin login in loginList)
            {
                editableList.InsertItem(login.id, false, new String[1] { login.username });
            }
            editableList.DrawList();

            // O clique do botão chama o script de alteração passando "id = 0", a tela de alteração
            // interpreta "id = 0" como "criar um novo", o id é então gerado no banco de dados.
            btnNovo.Attributes.Add("onClick", String.Format(alterScript, 0));
        }
    }

}
