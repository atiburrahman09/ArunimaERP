using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.ViewModel
{
    public class DivisionViewModel
    {
        public int DivisionID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string DivisionName { get; set; }
    }
}
