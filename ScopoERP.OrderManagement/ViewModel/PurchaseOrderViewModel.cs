using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.OrderManagement.ViewModel
{
    public class PurchaseOrderViewModel
    {
        public int? JobID { get; set; }
        public string JobNo { get; set; }

        [Required(AllowEmptyStrings = false)]
        public int StyleID { get; set; }
        public string StyleNo { get; set; }

        public int PurchaseOrderID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PurchaseOrderNo { get; set; }

        public decimal FOB { get; set; }
        public decimal AgreedCM { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime ExitDate { get; set; }
        public DateTime? OriginalCRD { get; set; }

        public int? ShipMode { get; set; }
        public string ShipModeName { get; set; }
        public string DCCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        public int FactoryID { get; set; }
        public string FactoryName { get; set; }

        public decimal? SubContractRate { get; set; }
        public decimal? FactoryCM { get; set; }

        [Required(AllowEmptyStrings = false)]
        public int SeasonID { get; set; }
        public string SeasonName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public int? CurrentStatus { get; set; }

        public string CostSheetNo { get; set; }

        public string Remarks { get; set; }
    }
}
