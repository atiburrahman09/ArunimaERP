using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class SewingShipmentSummaryReportViewModel
    {
        public string Floor { get; set; }
        public string Buyer { get; set; }
        public int BuyerID { get; set; }
        public string JobNo { get; set; }
        public string StyleNo { get; set; }
        public string PONo { get; set; }
        public int PurchaseOrderID { get; set; }
        public decimal FOB { get; set; }
        public decimal AgreedCM { get; set; }
        public DateTime ExitDate { get; set; }

        public int OrderQuantity { get; set; }
        public long? SewingQuantity { get; set; }
        public int? ShippedQuantity { get; set; }

        public decimal? OrderFOB { get; set; }
        public decimal? SewingFOB { get; set; }
        public decimal? ShippedFOB { get; set; }

        public decimal? OrderCM { get; set; }
        public decimal? SewingCM { get; set; }
        public decimal? ShippedCM { get; set; }
    }
}
