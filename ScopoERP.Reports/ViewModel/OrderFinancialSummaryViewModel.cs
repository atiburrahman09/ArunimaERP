using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class OrderFinancialSummaryViewModel
    {
        public string JobNo { get; set; }
        public string ContractNo { get; set; }
        public int OrderQuantity { get; set; }
        public decimal TotalShipment { get; set; }
        public decimal TotalFOB { get; set; }
        public decimal TotalInvoiceFOB { get; set; }
    }
}
