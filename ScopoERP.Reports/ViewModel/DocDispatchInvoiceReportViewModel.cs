using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class DocDispatchInvoiceReportViewModel
    {
        public string JobNo { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string BL { get; set; }
        public DateTime? OnBoardDate { get; set; }
        public DateTime? BLReleaseDate { get; set; }
        public string EXP { get; set; }
        public DateTime? EXPDate { get; set; }
        public string FDBPNo { get; set; }
        public int? TotalShipmentQuantity { get; set; }
        public decimal? InvoiceFOB { get; set; }
        public decimal RealizedAmount { get; set; }
        public DateTime? RealizedDate { get; set; }
    }

    public class DocDispatchInvoiceSummaryReportViewModel
    {
        public string BuyerName { get; set; }
        public string InvoiceNo { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal? InvoiceFOB { get; set; }
        public string DispatchStatus { get; set; }
        
    }
}
