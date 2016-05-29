using System;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAdministrator
{
    public partial class ConfigLicenses : System.Web.UI.Page
    {
        private AdministratorMasterPage administratorMasterPage;


        protected void Page_Load(Object sender, EventArgs e)
        {
            administratorMasterPage = (AdministratorMasterPage)Page.Master;
            administratorMasterPage.AddMenuItems();
            administratorMasterPage.InitializeMasterPageComponents();

            TenantDAO tenantDAO = new TenantDAO(administratorMasterPage.dataAccess.GetConnection());
            LicenseDAO licenseDAO = new LicenseDAO(administratorMasterPage.dataAccess.GetConnection());

            String[] columnNames = new String[] { "Empresa", "Quantidade de Licenças" };
            String addScript = "window.open('AddLicenses.aspx?tenantId=' + {0}, 'AddLicenses', 'width=540,height=600');";
            EditableListButton[] buttons = new EditableListButton[]
            {
                // Botões que devem aparecer para os items da lista
                new EditableListButton("Adicionar", addScript, ButtonTypeEnum.Edit)
            };

            EditableList editableList = new EditableList(configurationArea, columnNames, buttons);
            List<Object> tenantList = tenantDAO.GetAllTenants();
            foreach(Tenant tenant in tenantList)
            {
                String[] itemProperties = new String[]
                {
                    tenant.alias,
                    licenseDAO.GetAllLicenses(tenant.id).Count.ToString()
                };
                editableList.InsertItem(tenant.id, false, itemProperties);
            }
            editableList.DrawList();
        }
    }

}
