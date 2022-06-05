using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class Bundle
    {
        public int BundleID { get; set; }
        public int CuttingPlanID { get; set; }
        public string BundleNo { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }

        public virtual cuttingPlan cuttingplan { get; set; }
        public virtual ICollection<coupon> coupons { get; set; }

    }
}
