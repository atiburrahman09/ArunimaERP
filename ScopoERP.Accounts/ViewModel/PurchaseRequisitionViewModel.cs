using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Accounts.ViewModel
{
    public class PurchaseRequisitionViewModel
    {
        public int PurchaseRequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Sector { get; set; }
        public string Remarks { get; set; }

        public IEnumerable<PurchaseRequisitionDetailsViewModel> RequisitionDetails { get; set; }

        public decimal? RequisitionAmount { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public DateTime? InstallmentDate { get; set; }
        public decimal? PayableAmount { get; set; }
        public DateTime? PayableDate { get; set; }

        public int UserID { get; set; }
        public DateTime SetDate { get; set; }
    }

    public class PurchaseRequisitionDetailsViewModel
    {
        public int PurchaseRequisitionDetailsID { get; set; }
        public string ProductDescription { get; set; }
        public decimal Quantity { get; set; }
        public int UnitID { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
