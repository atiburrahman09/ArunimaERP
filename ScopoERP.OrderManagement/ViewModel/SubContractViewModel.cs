using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.OrderManagement.ViewModel
{
    public class SubContractViewModel
    {
        public int SubContractID { get; set; }
        public string SubContractNo { get; set; }
        public int SubFactoryID { get; set; }
        public int FactoryID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int SubContractQuantity { get; set; }
        public DateTime SubContractExitDate { get; set; }
        public decimal SubContractRate { get; set; }
        public decimal? CommercialPercentage { get; set; }
        public string Remarks { get; set; }
        public int UserID { get; set; }
        public DateTime SetupDate { get; set; }
    }
}
