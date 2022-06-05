using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Accounts.ViewModel
{
    public class PurchaseRequisitionInstallmentViewModel
    {
        public int PurchaseRequisitionInstallmentID { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public string RequisitionNo { get; set; }

        public decimal Amount { get; set; }
        public DateTime InstallmentDate { get; set; }

        public decimal? PayableAmount { get; set; }
        public DateTime? PayableDate { get; set; }

        public int UserID { get; set; }
        public DateTime SetDate { get; set; }
    }
}
