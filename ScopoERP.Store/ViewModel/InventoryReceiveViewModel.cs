using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Store.ViewModel
{
    public class InventoryReceiveViewModel
    {
        public int BLDetailsID { get; set; }
        public int BLID { get; set; }
        public int BookingID { get; set; }
        public decimal? InvoiceQuantity { get; set; }
        public decimal? ReceivedQuantity { get; set; }
        public int UserID { get; set; }
        public DateTime SetupDate { get; set; }

        public string PONo { get; set; }
        public string ItemDescription { get; set; }
        public string ItemColor { get; set; }
        public string ItemSize { get; set; }
        public decimal BookingQuantity { get; set; }
        public string ConsumpsionUnit { get; set; }
    }
}
