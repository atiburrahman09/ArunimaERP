using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class SubContractReportViewModel
    {
        public string FactoryName { get; set; }
        public string SubContractFactory { get; set; }
        public string BuyerName { get; set; }
        public string PONo { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal FOB { get; set; }
        public decimal AgreedCM { get; set; }
        public decimal? CommercialPercentage { get; set; }
        public decimal? SubContractRate { get; set; }
        public int OrderQuantity { get; set; }
        public decimal? InvoiceFOB { get; set; }
        public int? TotalShipment { get; set; }
    }
}
