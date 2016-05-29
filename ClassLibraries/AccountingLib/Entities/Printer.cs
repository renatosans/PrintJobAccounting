using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Entities
{
    public class Printer
    {
        [ItemId]
        public int id;

        public int tenantId;

        public String name;

        [ItemName]
        public String alias;

        public Decimal pageCost;

        public Decimal colorCostDiff;

        public Decimal duplexCostDiff;

        public Boolean bwPrinter = true; // default - impressoras monocromáticas


        public Printer()
        {
        }

        public Printer(int tenantId, String name, String alias)
        {
            this.id = 0;
            this.tenantId = tenantId;
            this.name = name;
            this.alias = alias;
        }

        public override string ToString()
        {
            return this.alias;
        }
    }

}
