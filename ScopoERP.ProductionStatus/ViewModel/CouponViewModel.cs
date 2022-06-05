using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class CouponViewModel
    {
        public int CouponID { get; set; }
        public int SerialNo { get; set; }
        public int StyleOperationID { get; set; }
        public string EmployeeCardNo { get; set; }
        public int OperationID { get; set; }
        public int? SpecID { get; set; }
        public int StyleID { get; set; }
        public string OperationNo { get; set; }
        public int StandardOperationID { get; set; }
        public string OperationName { get; set; }
        public string PurchaseOrderNo { get; set; }
        public decimal Sam { get; set; }
        public decimal? AuxSam { get; set; }
        public string BundleNo { get; set; }
        public string Size { get; set; }
        public string JobClassName { get; set; }
        public decimal Time { get; set; }//in minute; sam+auxsam
        public decimal? BaseRate { get; set; }
        public decimal? Value { get; set; }
        public int? Bundlesize { get; set; }
        public DateTime CompletedDate { get; set; }
        public int CuttingPlanID { get; set; }
        public int Quantity { get; set; }
        public string  SpecNo { get; set; }
        public int BundleID { get; set; }
        public int CutNo { get; set; }
        public string type { get; set; }
        public int SectionNo { get; set; }
        public int? SupervisorID { get; set; }
        public int OperationCategory { get; set; }
        public string SpecName { get; set; }
        public int JobClassID { get; set; }
        public int PoStyleId { get; set; }
        public int? EmployeeID { get; set; }
    }
}
