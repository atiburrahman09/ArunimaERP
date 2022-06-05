using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.ViewModel
{
    public class BookingViewModel
    {
        public int BookingID { get; set; }

        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }

        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }

        public string ItemSize { get; set; }
        public string ItemColor { get; set; }

        public int ConsumptionUnitID { get; set; }
        public string ConsumptionUnitName { get; set; }
        
        public decimal TotalQuantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public int? PIID { get; set; }
        public string PINo { get; set; }
        public string ReferenceNo { get; set; }

        public int SupplierID { get; set; }
        public string SupplierName { get; set; }

        public int RevisionNo { get; set; }

        public decimal? Ratio { get; set; }

        public int UserID { get; set; }

        public DateTime SetDate { get; set; }
    }

    public class BookingSelectionViewModel
    {
        public bool IsChecked { get; set; }
        public string StyleNo { get; set; }
        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }
        public int ItemID { get; set; }
        public string ItemDescription { get; set; }
    }
}
