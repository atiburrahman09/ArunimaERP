using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Commercial.ViewModel
{
    public class BankForwardingViewModel
    {
        public int BankForwardingID { get; set; }
        public string BankForwardingNo { get; set; }
        public DateTime BankForwardingDate { get; set; }
        public string FDBPNo { get; set; }
        public int JobID { get; set; }
        public string JobNo { get; set; }
        public bool Status { get; set; }
        public int UserID { get; set; }
        public DateTime SetupDate { get; set; }
        public string Courier { get; set; }

        public List<InvoiceSummary> InvoiceList { get; set; }
    }

    public class InvoiceSummary
    {
        public int InvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
    }
}
