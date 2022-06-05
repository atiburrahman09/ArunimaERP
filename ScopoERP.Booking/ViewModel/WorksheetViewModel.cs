using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.ViewModel
{
    public class WorksheetViewModel
    {
        public int WorksheetId { get; set; }
        public int PoStyleId { get; set; }

        public int ItemCategoryID { get; set; }
        public string ItemCategoryName { get; set; }
        public string ItemCategory { get; set; }
        public string ItemDescription { get; set; }

        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public string Size { get; set; }
        public string Color { get; set; }

        public string ItemSize { get; set; }
        public string ItemColor { get; set; }

        public decimal Consumption { get; set; }
        public int ConsumptionUnitId { get; set; }

        public string ConsumptionUnitName { get; set; }
        public string ConsumptionUnit { get; set; }

        public decimal Wastage { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalQuantity { get; set; }

        public int Formula { get; set; }
        public string FormulaText { get; set; }

        public int Status { get; set; }

        public decimal ItemQuantity
        {
            get { 
                return this.TotalQuantity / (this.Consumption + ((this.Consumption * this.Wastage) / 100)); 
            }
        }
    }
}
