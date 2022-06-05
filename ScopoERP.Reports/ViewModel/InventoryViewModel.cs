using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class InventoryViewModel
    {
        public string UserName { get; set; }
        //public int? CLNo { get; set; }
        //public DateTime? CLDate { get; set; }
        public string BLNo { get; set; }
        public DateTime? BLDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Item { get; set; }
        public decimal? TotalBookingQty { get; set; }
        public decimal? TotalReceivedQty { get; set; }
        public decimal? InvoiceQuantity { get; set; }
        //public decimal? TotalRequisitionQty { get; set; }
        public decimal? TotalIssueQty { get; set; }
        //public decimal? TotalBalanceQty { get; set; }
        //public string IssueFloorName { get; set; }
        public string BalanceQtyInHand { get; set; }
        public string ItemColor { get; set; }
        public string ItemSize { get; set; }
    }
}
