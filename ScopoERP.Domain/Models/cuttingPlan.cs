using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class cuttingPlan
    {
        [Key]
        public int CuttingPlanID { get; set; }

        public int PurchaseOrderID { get; set; }
        [ForeignKey("PurchaseOrderID")]
        public virtual postyle PurchaseOrders { get; set; }

        public int CuttingQuantity { get; set; }
        public int BundlePerQuantity { get; set; }
        public DateTime CuttingDate { get; set; }
        public int CuttingNo { get; set; }
        public int NoOfBundle { get; set; }
        public string Shade { get; set; }
        public bool IsPrepack { get; set; }
        public int LoopPattern { get; set; }



        //public virtual ICollection<RawMaterialRequisition> rawmaterialrequisitiondetails { get; set; }
        public virtual ICollection<Bundle> bundles { get; set; }
        public virtual ICollection<coupon> coupons { get; set; }
    }
}
