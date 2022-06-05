using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
   public class SupervisorViewModel
    {
        public int SupervisorID { get; set; }
        public string SupervisorName { get; set; }
        public int CardNo { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
    }
}
