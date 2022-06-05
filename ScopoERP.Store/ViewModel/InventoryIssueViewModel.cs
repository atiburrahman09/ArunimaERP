using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Store.ViewModel
{
    public class InventoryIssueViewModel
    {
        public int InventoryIssueID { get; set; }
        public int SRID { get; set; }
        public int ItemID { get; set; }
        public decimal? IssuedQuantity { get; set; }
        public decimal? RequestedQuantity { get; set; }
        public short Status { get; set; }
        public int PoStyleId { get; set; }        
    }
}
