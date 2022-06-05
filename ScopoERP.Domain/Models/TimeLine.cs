using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class TimeLine
    {
        public int TimeLineID { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> ProvableDate { get; set; }
        public Nullable<DateTime> ExpectedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> LastModified { get; set; }
        public int PurchaseOrderID { get; set; }
    }
}
