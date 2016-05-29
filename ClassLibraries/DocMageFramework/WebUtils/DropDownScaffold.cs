using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocMageFramework.Reflection;
using DocMageFramework.DataManipulation;


namespace DocMageFramework.WebUtils
{
    public static class DropDownScaffold
    {
        // Convenções:
        // A procedure deve retornar registros compatíveis com objectType
        // objectType deve possuir campos com os atributos [ItemId] e [ItemName]
        // Estes campos devem ser publicos e de instancia (não estáticos)
        public static ListItem[] RetrieveStrict(String procedureName, SqlConnection sqlConnection, Type objectType)
        {
            return GetItems(procedureName, sqlConnection, objectType, true);
        }

        public static ListItem[] Retrieve(String procedureName, SqlConnection sqlConnection, Type objectType)
        {
            return GetItems(procedureName, sqlConnection, objectType, false);
        }

        private static ListItem[] GetItems(String procedureName, SqlConnection sqlConnection, Type objectType, Boolean strict)
        {
            ProcedureCall procedureCall = new ProcedureCall(procedureName, sqlConnection);
            Object tenant = HttpContext.Current.Session["tenant"];
            NameValueCollection tenantInfo = HttpUtility.ParseQueryString(tenant.ToString());
            int tenantId = 0;
            int.TryParse(tenantInfo["id"], out tenantId);
            procedureCall.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, tenantId));
            procedureCall.Execute(true);
            List<Object> returnList = procedureCall.ExtractFromResultset(objectType);

            ItemScaffolding itemScaffolding = new ItemScaffolding();
            itemScaffolding.FindItemFields(objectType);

            ListItem[] itemArray;
            int ndx;
            if (strict)
            {
                itemArray = new ListItem[returnList.Count];
                ndx = 0;
            }
            else
            {
                itemArray = new ListItem[returnList.Count + 1];
                itemArray[0] = new ListItem();
                itemArray[0].Value = "0";
                itemArray[0].Text = "";
                ndx = 1;
            }
            foreach (Object obj in returnList)
            {
                itemArray[ndx] = new ListItem();
                itemArray[ndx].Value = itemScaffolding.GetItemId(obj).ToString();
                itemArray[ndx].Text = itemScaffolding.GetItemName(obj);
                ndx++;
            }

            return itemArray;
        }


        private static int? ParseId(String idValue)
        {
            int? id;

            id = int.Parse(idValue);
            if (id == 0) id = null;

            return id;
        }


        public static int? GetSelectedItemId(DropDownList dropDownList)
        {
            String idValue = dropDownList.SelectedItem.Value;
            int? id = ParseId(idValue);

            return id;
        }
    }

}
