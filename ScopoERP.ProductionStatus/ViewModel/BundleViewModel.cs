using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class BundleViewModel
    {
        public int BundleID { get; set; }
        public int CuttingPlanID { get; set; }
        public string BundleNo { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
    }
}
