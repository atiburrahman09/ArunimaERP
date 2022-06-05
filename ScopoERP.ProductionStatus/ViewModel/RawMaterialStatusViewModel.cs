using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class RawMaterialStatusViewModel
    {
        public int ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public int ItemID { get; set; }
        public string ItemDescription { get; set; }
        public decimal TotalQuantity { get; set; }
        public string CurrentStatus { get; set; }
    }
}
