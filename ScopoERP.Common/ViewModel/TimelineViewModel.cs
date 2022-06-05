using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.ViewModel
{
    public class TimelineViewModel
    {
        public int TimeLineID { get; set; }
        [Required]
        public string Description { get; set; }
        public Nullable<DateTime> ProvableDate { get; set; }
        public Nullable<DateTime> ExpectedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> LastModified { get; set; }
        [Required]
        public int PurchaseOrderID { get; set; }
    }
}
