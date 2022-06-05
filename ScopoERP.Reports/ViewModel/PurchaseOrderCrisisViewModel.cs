using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class PurchaseOrderCrisisViewModel
    {
        public string JobNo { get; set; }

        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime ExitDate { get; set; }

        public string StyleNo { get; set; }

        public bool IsSizeColorExists { get; set; }
        public bool IsCostsheetExists { get; set; }
        public bool IsWorksheetExists { get; set; }
    }
}
