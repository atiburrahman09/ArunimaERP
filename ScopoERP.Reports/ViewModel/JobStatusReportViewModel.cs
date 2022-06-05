using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class JobStatusReportViewModel
    {
        public string JobNo { get; set; }
        public string ContractNo { get; set; }
        public string StyleNo { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime ExitDate { get; set; }
        public decimal FOB { get; set; }
        public decimal AgreedCM { get; set; }
        public decimal RMCost { get; set; }
        public decimal? ActualRMCost { get; set; }
        public decimal TotalFOB { get; set; }
        public decimal TotalAgreedCM { get; set; }
        public decimal? FabricsValue { get; set; }
        public decimal? TrimsValue { get; set; }
        public decimal? WashValue { get; set; }
        public decimal? EmbPrintValue { get; set; }
        public decimal? OthersValue { get; set; }
    }
}
