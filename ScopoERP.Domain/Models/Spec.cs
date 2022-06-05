using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class Spec
    {
        public int SpecID { get; set; }
        public string SpecNo { get; set; }
        public string SpecName { get; set; }
        public int OperationID { get; set; }

       
        public virtual Operation Operations { get; set; }
    }
}
