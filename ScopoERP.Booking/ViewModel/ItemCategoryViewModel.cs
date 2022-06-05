using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.ViewModel
{
    public class ItemCategoryViewModel
    {
        public int ItemCategoryID { get; set; }

        [Required(ErrorMessage="Item category name is required")]
        public string Name { get; set; }
        public Nullable<int> ParentCategoryID { get; set; }
    }
}
