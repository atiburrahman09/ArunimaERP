using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class VATReportViewModel
    {
        public string JobNo { get; set; }
        public string InvoiceNo { get; set; }
        public string ShippingBill { get; set; }
        public DateTime? ShippingBillDate { get; set; }
        public int? ShippedQuantity { get; set; }
        public decimal? InvoiceFOB { get; set; }
    }
}
