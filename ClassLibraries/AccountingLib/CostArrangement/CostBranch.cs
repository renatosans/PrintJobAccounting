using System;
using System.Collections.Generic;
using AccountingLib.Entities;


namespace AccountingLib.CostArrangement
{
    public class CostBranch
    {
        private CostCenter costCenter;

        public CostBranch Parent;

        public List<CostBranch> Children;

        public List<Object> Associates;

        public String Name
        {
            get { return costCenter.name; }
        }

        public int Id
        {
            get { return costCenter.id; }
        }

        public Boolean IsRoot()
        {
            return (costCenter.parentId == null);
        }

        public CostBranch(CostCenter costCenter)
        {
            this.costCenter = costCenter;
        }
    }

}
