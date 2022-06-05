using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class JobSummaryReportViewModel
    {
        public int CurrentStatus { get; set; }
        public string JobNo { get; set; }
        public string StyleNo { get; set; }
        public string Remarks { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime ExitDate { get; set; }
        public decimal FOB { get; set; }
        public decimal TotalFOB { get; set; }
        public decimal AgreedCM { get; set; }
        public decimal TotalAgreedCM { get; set; }
        public int? ChalanQuantity { get; set; }
        public string ChalanNo { get; set; }
        public DateTime? ChalanDate { get; set; }
        public decimal? TotalInvoiceFOB { get; set; }
        public string InvoiceNo { get; set; }
    }
}
