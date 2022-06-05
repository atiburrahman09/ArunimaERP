using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ShipmentSummaryViewModel
    {
        public string FactoryName { get; set; }
        public int TotalOrderQuantity { get; set; }
        public int TotalShipmentQuantity { get; set; }
        public decimal TotalAgreedCM { get; set; }
        public decimal TotalShippedAgreedCM { get; set; }
        public decimal? TotalAdvanceCM { get; set; }
        public decimal? TotalRemainCM { get; set; }
        public decimal TotalFOB { get; set; }
        public decimal? TotalInvoiceFOB { get; set; }
    }
}
