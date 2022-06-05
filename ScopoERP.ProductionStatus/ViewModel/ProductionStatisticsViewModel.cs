using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class ProductionStatisticsViewModel
    {
        public int StyleID { get; set; }
        public string StyleNo { get; set; }
        public int Capacity { get; set; }

        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime ExitDate { get; set; }
    }
}
