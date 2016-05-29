using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Entities
{
    public class CostCenter
    {
        [ItemId]
        public int id;

        public int tenantId;

        [ItemName]
        public String name;

        public int? parentId = null;


        public CostCenter()
        {
        }

        public override String ToString()
        {
            return this.name;
        }
    }

}
