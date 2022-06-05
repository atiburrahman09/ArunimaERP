using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ImportStatusViewModel
    {
        public string BackToBackLCNo { get; set; }
        public string ExportLCNo { get; set; }
        public string JobNo { get; set; }
        public string Buyer { get; set; }
        public string Benificiary { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceValue { get; set; }
        public string InvoiceQuantity { get; set; }
        public string Unit { get; set; }
        public string BLNo { get; set; }
        public DateTime? BLDate { get; set; }
        public string PortName { get; set; }
        public DateTime? CopyDocuSentToCnf { get; set; }
        public string NegoOriginal { get; set; }
        public string ETA { get; set; }
        public string PositionOfVessel { get; set; }
        public DateTime? DeliveryByCnf { get; set; }
        public string Stuffing { get; set; }
        public string Container { get; set; }
        public string Remarks { get; set; } 
    }
}
