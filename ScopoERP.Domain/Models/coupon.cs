using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class coupon
    {
        [Key]
        public int CouponID { get; set; }
        public int SerialNo { get; set; }  
        public int BundleID { get; set; }
        public int OperationID { get; set; }
        public int? SpecID { get; set; }
        public int CuttingPlanID { get; set; }
        public int OperationCategoryID { get; set; }
        public int StyleOperationID { get; set; }
        public int PoStyleId { get; set; }
        public int JobClassID { get; set; }
        public int? SupervisorID { get; set; }

        public int Quantity { get; set; }
        public decimal Time { get; set; }
        public string Size { get; set; }

        public decimal BaseRate { get; set; }
        public decimal? Value { get; set; }

        public string EmployeeCardNo { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int SectionNo { get; set; }

        [ForeignKey("BundleID")]
        public virtual Bundle Bundles { get; set; }

        [ForeignKey("OperationID")]
        public virtual Operation Operations { get; set; }
        [ForeignKey("StyleOperationID")]
        public virtual StyleOperation StyleOperations { get; set; }

        [ForeignKey("CuttingPlanID")]
        public virtual cuttingPlan CuttingPlans { get; set; }
    }
}
