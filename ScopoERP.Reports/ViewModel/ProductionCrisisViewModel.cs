using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ProductionCrisisViewModel
    {
        public string BuyerName { get; set; }
        public string StyleNo { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime ExitDate { get; set; }
        public int ShipmentQuantity { get; set; }
        public DateTime? ShipmentDate { get; set; }
    }
}
