using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class SpecViewModel
    {
        public int SpecID { get; set; }
        [Required]
        public string SpecNo { get; set; }
        [Required]
        public string SpecName { get; set; }
        [Required]
        public int JobClassID { get; set; }
        [Required]
        public int OperationID { get; set; }
    }
}
