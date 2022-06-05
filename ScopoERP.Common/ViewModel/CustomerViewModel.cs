using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Stackholder.ViewModel
{
    public class CustomerViewModel
    {
        public int CustomerID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CustomerName { get; set; }
    }
}
