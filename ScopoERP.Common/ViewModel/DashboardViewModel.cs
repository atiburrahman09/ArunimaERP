using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.ViewModel
{
   public class DashboardViewModel
    {
        public int TotalOrder { get; set; }
        public int TotalInvoice { get; set; }
        public int TotalPI { get; set; }
        public int TotalContract { get; set; }
        public int CurrentShipment { get; set; }
        public int UpcomingShipment { get; set; }
    }
}
