using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class AdvancedCMReportViewModel
    {
        public string JobNo { get; set; }
        public string PONo { get; set; }
        public int? Month { get; set; }
        public string Period { get; set; }
        public decimal? TotalFOB { get; set; }
        public decimal? AvailableCM { get; set; }
        public decimal? PIValue { get; set; }
        public DateTime? ReceivedDate { get; set; }
    }
}
