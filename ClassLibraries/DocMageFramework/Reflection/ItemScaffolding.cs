using System;
using System.Reflection;
using DocMageFramework.CustomAttributes;


namespace DocMageFramework.Reflection
{
    /// <summary>
    /// Classe utilizada para obtenção/alteração de campos em objetos "item de lista", atua
    /// como uma armação ao redor do objeto e permite a utilização de objetos que não se
    /// sabe nada a respeito
    /// </summary>
    public class ItemScaffolding
    {
        private FieldInfo itemId;

        private FieldInfo itemName;


        public ItemScaffolding()
        {
            this.itemId = null;
            this.itemName = null;
        }


        public void FindItemFields(Type objectType)
        {
            // Convenções:
            // ObjectType deve possuir campos com os atributos [ItemID] e [ItemName]
            // Estes campos devem ser publicos e de instancia (não estáticos)

            itemId = null;
            itemName = null;
            FieldInfo[] fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fields)
            {
                foreach (object attrib in fieldInfo.GetCustomAttributes(true))
                {
                    if (attrib.GetType() == typeof(ItemId)) itemId = fieldInfo;
                    if (attrib.GetType() == typeof(ItemName)) itemName = fieldInfo;
                }
            }
        }


        public int GetItemId(Object item)
        {
            return (int) itemId.GetValue(item);
        }


        public void SetItemId(Object item, Object value)
        {
            itemId.SetValue(item, value);
        }


        public String GetItemName(Object item)
        {
            if (itemName == null) return null;
            return (String) itemName.GetValue(item);
        }


        public void SetItemName(Object item, Object value)
        {
            itemName.SetValue(item, value);
        }
    }

}
