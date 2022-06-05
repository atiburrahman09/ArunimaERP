using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Stackholder.ViewModel
{
    public class SupplierViewModel
    {
        public int SupplierID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SupplierName { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
    }
}
