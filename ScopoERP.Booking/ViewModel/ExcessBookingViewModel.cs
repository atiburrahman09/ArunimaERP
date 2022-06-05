using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.ViewModel
{
    public class ExcessBookingViewModel
    {
        public int ExcessBookingID { get; set; }
        public int JobID { get; set; }
        public string JobNo { get; set; }

        public int ProformaInvoiceID { get; set; }
        public string ProformaInvoiceNo { get; set; }

        public int ItemID { get; set; }
        public string ItemDescription { get; set; }

        public string ItemColor { get; set; }
        public string ItemSize { get; set; }

        public decimal TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }

        public bool Status { get; set; }
    }
}
