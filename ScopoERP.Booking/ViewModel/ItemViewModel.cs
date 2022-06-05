using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.ViewModel
{
    public class ItemViewModel
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }

        public int ItemCategoryID { get; set; }
        public string ItemCategoryName { get; set; }
    }
}
