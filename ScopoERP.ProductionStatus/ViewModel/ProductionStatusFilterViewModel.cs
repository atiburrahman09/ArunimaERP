using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class ProductionStatusFilterViewModel
    {
        public int BuyerID{ get; set; }
        public int StyleID { get; set; }
        public string ProductionFloor { get; set; }
        public string ProductionLine { get; set; }
        public DateTime ProductionDate { get; set; }
    }
}
