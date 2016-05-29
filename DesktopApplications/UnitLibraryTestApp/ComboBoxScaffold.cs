using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using DocMageFramework.Reflection;
using DocMageFramework.DataManipulation;


namespace UnitLibraryTestApp
{
    public static class ComboBoxScaffold
    {
        public static Object[] Retrieve(String procedureName, SqlConnection sqlConnection, Type objectType)
        {
            // Convenções:
            // A procedure deve retornar registros compatíveis com objectType
            // objectType deve possuir campos com os atributos [ItemId] e [ItemName]
            // Estes campos devem ser publicos e de instancia (não estáticos)

            ProcedureCall procedureCall = new ProcedureCall(procedureName, sqlConnection);
            procedureCall.parameters.Add(new ProcedureParam("@tenantId", SqlDbType.Int, 4, 4));
            procedureCall.Execute(true);
            List<Object> returnList = procedureCall.ExtractFromResultset(objectType);

            ItemScaffolding itemScaffolding = new ItemScaffolding();
            itemScaffolding.FindItemFields(objectType);

            Object[] itemArray = new Object[returnList.Count + 1];
            itemArray[0] = Activator.CreateInstance(objectType);
            itemScaffolding.SetItemId(itemArray[0], 0);
            itemScaffolding.SetItemName(itemArray[0], "");
            int ndx = 1;
            foreach (Object obj in returnList)
            {
                itemArray[ndx] = Activator.CreateInstance(objectType);
                itemScaffolding.SetItemId(itemArray[ndx], itemScaffolding.GetItemId(obj));
                itemScaffolding.SetItemName(itemArray[ndx], itemScaffolding.GetItemName(obj));
                ndx++;
            }

            return itemArray;
        }
    }

}
