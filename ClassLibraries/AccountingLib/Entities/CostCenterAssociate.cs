using System;


namespace AccountingLib.Entities
{
    public class CostCenterAssociate
    {
        public int id;

        public int tenantId;

        public int costCenterId;

        public int userId;

        public String userName;


        public CostCenterAssociate()
        {
        }

        public CostCenterAssociate(int tenantId, int costCenterId, int userId)
        {
            this.id = 0;
            this.tenantId = tenantId;
            this.costCenterId = costCenterId;
            this.userId = userId;
        }
    }

}
