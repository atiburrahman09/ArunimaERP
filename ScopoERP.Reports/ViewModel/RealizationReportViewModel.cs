using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class RealizationReportViewModel
    {
        public int BankForwardingID { get; set; }
        public string FDBPNo { get; set; }
        public decimal? InvoiceValue { get; set; }
        public decimal? RealizationValue { get; set; }
        public decimal? Difference { get; set; }
        public DateTime RealizationDate { get; set; }

        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public decimal? CurrencyRate { get; set; }
    }
}
