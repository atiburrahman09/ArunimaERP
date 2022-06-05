using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.ViewModel
{
    public class PIViewModel
    {
        public string JobNo { get; set; }

        public int PIID { get; set; }
        public string PINo { get; set; }
        public string ReferenceNo { get; set; }

        public string RequisitionNo { get; set; }
        public DateTime? RequisitionDate { get; set; }

        public DateTime? PIDate { get; set; }
        public decimal PIValue { get; set; }

        [Required]
        public int? SupplierID { get; set; }
        public string SupplierName { get; set; }

        public int LCTypeID { get; set; }
        public string LCTypeTitle { get; set; }

        //[Required]
        public DateTime? DeliveryDate { get; set; }
        ///[Required]
        public DateTime? ApproximateInHouseDate { get; set; }

        public int? BackToBackLCID { get; set; }
        public string BackToBackLCNo { get; set; }

        public int? LoanFromJobID { get; set; }
        public string LoanFromJobNo { get; set; }

        public int Status { get; set; }
        public int AccountID { get; set; }
    }
}
