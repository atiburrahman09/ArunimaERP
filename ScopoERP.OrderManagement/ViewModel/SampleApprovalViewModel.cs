using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.OrderManagement.ViewModel
{
    public class SampleApprovalViewModel
    {
        public int SampleApprovalID { get; set; }
        public int StyleID { get; set; }
        public string UserID { get; set; }

        public List<ApprovalViewModel> ApprovalList { get; set; }
    }

    public class ApprovalViewModel
    {
        public ApprovalViewModel()
        {
            this.ApprovalSerialNo = "1001";
        }

        public int SampleApprovalID { get; set; }
        public int SampleTypeID { get; set; }
        public int Quantity { get; set; }
        public DateTime ApproximateSentDate { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public int ApprovalStatus { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public string Remarks { get; set; }
        public string  CourierName { get; set; }
        public int CourierNo { get; set; }
        public string ApprovalThrough { get; set; }
        public int ValidityTime { get; set; }
        public string ApprovalSerialNo { get; set; }
    }
}
