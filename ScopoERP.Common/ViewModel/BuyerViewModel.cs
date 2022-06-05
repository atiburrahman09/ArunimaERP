using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Stackholder.ViewModel
{
    public class BuyerViewModel
    {
        public int BuyerID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string BuyerName { get; set; }
    }
}
