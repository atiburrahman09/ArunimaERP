using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class EmployeeRate
    {
        public int EmployeeRateID { get; set; }
        public string EmployeeCardNo { get; set; }
        //public int OperationID { get; set; }
        public string Curve { get; set; }
        public int? Stage { get; set; }
        public string RTorNHorFL { get; set; }
        public int Section { get; set; }
        public string SpecNo { get; set; }
    }
}
