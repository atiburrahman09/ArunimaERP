using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class JobItemReportViewModel
    {
        public string StyleNo { get; set; }
        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public decimal TotalFOB { get; set; }
        public decimal TotalAgreedCM { get; set; }
        public decimal TotalRM { get; set; }

        public int ItemID { get; set; }
        public string ItemCategory { get; set; }
        public string ItemDescription { get; set; }

        public decimal? CostsheetConsumption { get; set; }
        public decimal? CostsheetUnitPrice { get; set; }
        public string CostsheetUnitName { get; set; }
        public decimal? CostsheetWastage { get; set; }
        public decimal? CostsheetTotalConsumption { get; set; }
        public decimal? CostsheetTotalPrice { get; set; }

        public decimal? BookingConsumption { get; set; }
        public decimal? BookingUnitPrice { get; set; }
        public string BookingUnitName { get; set; }
        public decimal? BookingWastage { get; set; }
        public decimal? BookingTotalConsumption { get; set; }
        public decimal? BookingTotalPrice { get; set; }
        public string SupplierName { get; set; }
        public string PINo { get; set; }
        public string ReferenceNo { get; set; }
    }
}
