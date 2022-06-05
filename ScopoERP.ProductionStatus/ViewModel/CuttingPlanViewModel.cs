using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class CuttingPlanViewModel
    {
        public int CuttingPlanID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int CuttingQuantity { get; set; }
        public int BundlePerQuantity { get; set; }
        public DateTime CuttingDate { get; set; }
        public int CuttingNo { get; set; }
        public int NoOfBundle { get; set; }
        public int LoopPattern { get; set; }
        public bool IsPrepack { get; set; }
        public string Shade { get; set; }
    }
}
