using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ProformaInvoiceViewModel
    {
        public string PONo { get; set; }
        public string ItemCategory { get; set; }
        public string ItemDescription { get; set; }
        public string ItemColor { get; set; }
        public string ItemSize { get; set; }
        public decimal ToTalQuantity { get; set; }
        public string ConsumptionUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
