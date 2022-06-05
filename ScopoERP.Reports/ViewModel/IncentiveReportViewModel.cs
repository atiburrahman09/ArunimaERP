using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class IncentiveReportViewModel
    {
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? OnBoardDate { get; set; }

        public string EXP { get; set; }
        public DateTime? EXPDate { get; set; }

        public string BL { get; set; }
        public DateTime? BLRealeaseDate { get; set; }

        public string FDBPNo { get; set; }
        public decimal? RealizationAmount { get; set; }
        public DateTime? RealizationDate { get; set; }

        public int ShipmentQuantity { get; set; }
        public decimal? InvoiceFOB { get; set; }

        public string PortOfLoading { get; set; }
        public string FinalDestination { get; set; }
        public string CountryName { get; set; }
    }
}
