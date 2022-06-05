using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoERP.Common.ViewModel;

namespace ScopoERP.OrderManagement.ViewModel
{
    public class SplitViewModel
    {
        public int MasterPOID { get; set; }
        public List<SplitPONo> SplitList { get; set; }
    }

    public class SplitPONo
    {
        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime ExitDate { get; set; }
        public bool IsChecked { get; set; }
        public decimal? FOB { get; set; }
        public decimal? AgreedCM { get; set; }
    }
}
