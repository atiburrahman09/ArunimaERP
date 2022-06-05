using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class MonthlyFinancialReportViewModel
    {
        public string StyleNo { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public int SewingQuantity { get; set; }
        public decimal? MaterialCost { get; set; }
        public decimal? ImposedMaterialCost { get; set; }
        public int? ShippedQuantity { get; set; }
        public decimal? MaterialCostOfShippedQuantity { get; set; }
        public decimal? MaterialCostOfIrregular { get; set; }
    }
}
