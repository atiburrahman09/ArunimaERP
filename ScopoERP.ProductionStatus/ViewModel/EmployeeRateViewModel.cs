using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class EmployeeRateViewModel
    {
        public int EmployeeRateID { get; set; }
        [Required]
        public string EmployeeCardNo { get; set; }
        [Required]
       // public int OperationID { get; set; }
        public string Curve { get; set; }
        public int? Stage { get; set; }
        public string RTorNHorFL { get; set; }
        [Required]
        public int Section { get; set; }
        public string SpecNo { get; set; }
    }
}
