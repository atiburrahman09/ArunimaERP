using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ContractPaperViewModel
    {
        public string StyleNo { get; set; }
        public string PONo { get; set; }
        public string Item { get; set; }
        public int OrderQuantity { get; set; }
        public decimal FOB { get; set; }
        public DateTime ExitDate { get; set; }
        public decimal? FactoryCM { get; set; }
    }
}
