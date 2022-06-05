using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class OperationCategory
    {
        public int OperationCategoryID { get; set; }
        public string OperationCategoryName { get; set; }

        public ICollection<OperationCategory> OperationCategories;
    }
}
