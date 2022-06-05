using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class BankForwardingReportViewModel
    {
        public string ContractNo { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? InvoiceFOB { get; set; }
        public int? InvoiceQuantity { get; set; }
        public string EXPNo { get; set; }
        public string BLToBeEndorsedTo { get; set; }
    }
}
