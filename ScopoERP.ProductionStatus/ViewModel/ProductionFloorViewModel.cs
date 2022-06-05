using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Production.ViewModel
{
    public class ProductionFloorViewModel
    {
        public int ProductionFloorID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Floor { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Line { get; set; }

        public string Division { get; set; }
        public int Status { get; set; }
    }
}
