using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class AssignCouponViewModel
    {
        public string EmployeeCardNo { get; set; }
        public DateTime CompletedDate { get; set; }
        public List<CouponAssignViewModel> CouponList { get; set; }
    }
    public class CouponAssignViewModel
    {
        public int CouponNo { get; set; }
        public int CouponID { get; set; }
    }
}
