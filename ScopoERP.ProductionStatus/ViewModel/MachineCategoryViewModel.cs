using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Production.ViewModel
{
    public class MachineCategoryViewModel
    {
        public int MachineCategoryID { get; set; }
        public string Name { get; set; }

        public int? ParentCategoryID { get; set; }
        public string ParentCategoryName { get; set; }

        public int UserID { get; set; }
        public DateTime SetupDate { get; set; }
    }
}
