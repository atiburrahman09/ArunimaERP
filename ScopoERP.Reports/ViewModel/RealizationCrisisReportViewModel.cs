using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class RealizationCrisisReportViewModel
    {
        public int BankForwardingID { get; set; }
        public string BankForwardingNo { get; set; }
        public DateTime BankForwardingDate { get; set; }
        public string FDBPNo { get; set; }
    }
}
