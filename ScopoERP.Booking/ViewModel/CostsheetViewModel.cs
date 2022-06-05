using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.ViewModel
{
    public class CostsheetViewModel
    {
        public int CostsheetID { get; set; }

        [Required]
        public int StyleID { get; set; }
        public string CostSheetNo { get; set; }

        [Required]
        public int ItemCategoryID { get; set; }
        public string ItemCategory { get; set; }

        [Required]
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }

        [Required]
        public decimal Consumption { get; set; }

        [Required]
        public int ConsumptionUnitID { get; set; }
        public string ConsumptionUnit { get; set; }

        [Required]
        public decimal? ConversionQuantity { get; set; }

        [Required]
        public int ConversionUnitID { get; set; }
        public string ConversionUnit { get; set; }

        [Required]
        public decimal Wastage { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public decimal? ActualConsumption
        {
            get { return Math.Round((decimal)(Consumption / ConversionQuantity), 10); }
        }

        public decimal? TotalRawMaterials
        {
            get { return Math.Round((decimal)(ActualConsumption + (Consumption * Wastage / (100 * ConversionQuantity))), 10); }
        }

        public decimal? TotalActualCost
        {
            get { return Math.Round((decimal)(TotalRawMaterials * UnitPrice), 4); }
        }
    }


    public class CostSheetSummaryViewModel
    {
        public string StyleNo { get; set; }
        public string CostSheetNo { get; set; }
    }
}
