using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class PurchaseRequisitionReportViewModel
    {
        public string RequisitionNo { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string DepartmentName { get; set; }

        public string ProductDescription { get; set; }
        public decimal Quantity { get; set; }
        public string UnitName { get; set; }
        public decimal UnitPrice { get; set; }
        public string Remarks { get; set; }
        public string Sector { get; set; }
    }
}
