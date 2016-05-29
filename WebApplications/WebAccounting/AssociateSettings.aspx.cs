using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using AccountingLib.Entities;
using AccountingLib.DataAccessObjects;
using DocMageFramework.WebUtils;


namespace WebAccounting
{
    public partial class AssociateSettings : System.Web.UI.Page
    {
        private SettingsMasterPage settingsMasterPage;

        private Tenant tenant;

        private int comparisonValue;

        private String action;

        private int costCenterId;


        private Boolean CheckAssociateMethod(Object associate)
        {
            CostCenterAssociate objToCheck = (CostCenterAssociate)associate;
            if (objToCheck.userId != comparisonValue)
                return false;

            return true;
        }

        private List<Object> GetAllUsers()
        {
            UserDAO userDAO = new UserDAO(settingsMasterPage.dataAccess.GetConnection());
            List<Object> userList = userDAO.GetAllUsers(tenant.id);

            return userList;
        }

        private ListItem[] GetAvailableUsers(List<Object> userList)
        {
            SqlConnection sqlConnection = settingsMasterPage.dataAccess.GetConnection();
            CostCenterAssociateDAO associateDAO = new CostCenterAssociateDAO(sqlConnection);
            List<Object> associates = associateDAO.GetAllAssociates(tenant.id);

            SortedList<String, ListItem> availableUsers = new SortedList<String, ListItem>();
            foreach (User user in userList)
            {
                comparisonValue = user.id;

                if (associates.Find(CheckAssociateMethod) == null)
                {
                    ListItem availableUser = new ListItem();
                    availableUser.Text = user.alias;
                    availableUser.Value = user.id.ToString();

                    availableUsers.Add(availableUser.Text, availableUser);
                }
            }
            
            ListItem[] returnList = new ListItem[availableUsers.Count];
            availableUsers.Values.CopyTo(returnList, 0);
            
            return returnList;
        }

        private ListItem[] GetAssociates(int costCenterId)
        {
            SqlConnection sqlConnection = settingsMasterPage.dataAccess.GetConnection();
            CostCenterAssociateDAO associateDAO = new CostCenterAssociateDAO(sqlConnection);
            List<Object> associateList = associateDAO.GetAssociates(tenant.id, costCenterId);

            SortedList<String, ListItem> associates = new SortedList<String, ListItem>();
            foreach(CostCenterAssociate associate in associateList)
            {
                ListItem newItem = new ListItem();
                newItem.Text = associate.userName;
                newItem.Value = associate.id.ToString();

                associates.Add(newItem.Text, newItem);
            }
            
            ListItem[] returnList = new ListItem[associates.Count];
            associates.Values.CopyTo(returnList, 0);
            
            return returnList;
        }

        protected void Page_Load(Object sender, EventArgs e)
        {
            settingsMasterPage = (SettingsMasterPage)Page.Master;
            settingsMasterPage.InitializeMasterPageComponents();
            tenant = (Tenant)Session["tenant"];

            if (String.IsNullOrEmpty(Request.QueryString["action"]))
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }
            action = Request.QueryString["action"];

            Boolean isNumeric = int.TryParse(Request.QueryString["costCenterId"], out costCenterId);
            if (!isNumeric)
            {
                EmbedClientScript.ShowErrorMessage(this, "Os parâmetros passados para a página não estão em um formato válido.", true);
                return;
            }

            if (Page.IsPostBack)
            {
                ProcessSubmitClick();
                return;
            }


            ListItem[] items = null;
            if (action == "Associate")
            {
                lblTitle.Text = "Associação";
                lblPageInfo.Text = lblPageInfo.Text + "Será criada a associação entre usuário e centro de custo.";
                List<Object> userList = GetAllUsers();
                if (userList.Count < 1)
                {
                    EmbedClientScript.ShowErrorMessage(this, "Nenhum usuário existente para associação.", true);
                    return;
                }
                items = GetAvailableUsers(userList);
                if (items.Length < 1)
                {
                    EmbedClientScript.ShowErrorMessage(this, "Todos os usuários já estão associados a centros de custo.", true);
                    return;
                }
            }
            if (action == "Disassociate")
            {
                lblTitle.Text = "Disassociação";
                lblPageInfo.Text = lblPageInfo.Text + "Será removida a associação entre usuário e centro de custo.";
                items = GetAssociates(costCenterId);
                if (items.Length < 1)
                {
                    EmbedClientScript.ShowErrorMessage(this, "Nenhuma associação existente para remoção.", true);
                    return;
                }
            }

            // Monta a lista com o nome dos usuários. O valor dos items na lista pode ser o "id do usuário"
            // ou o "id da associação" dependendo da ação (action=Associate  ou  action=Disassociate)
            listAvailable.Items.AddRange(items);
        }

        private CostCenterAssociate NewAssociate(int userId)
        {
            CostCenterAssociate associate = new CostCenterAssociate();
            associate.tenantId = tenant.id;
            associate.costCenterId = costCenterId;
            associate.userId = userId;

            return associate;
        }

        private void ProcessSubmitClick()
        {
            SqlConnection sqlConnection = settingsMasterPage.dataAccess.GetConnection();
            CostCenterAssociateDAO associateDAO = new CostCenterAssociateDAO(sqlConnection);

            foreach (String fieldName in Request.Form)
            {
                if (fieldName.Contains("transferedItem"))
                {
                    int idPos = fieldName.IndexOf("transferedItem") + "transferedItem".Length;
                    int itemId = int.Parse(fieldName.Substring(idPos));
                    String itemValue = Request.Form[fieldName];

                    //O valor dos items na lista pode ser o "id do usuário" ou o "id da associação"
                    // dependendo da ação (action=Associate  ou  action=Disassociate)
                    if (action == "Associate")
                        associateDAO.SetAssociate(NewAssociate(itemId));
                    if (action == "Disassociate")
                        associateDAO.RemoveAssociate(itemId);
                }
            }

            EmbedClientScript.CloseWindow(this);
        }
    }

}
