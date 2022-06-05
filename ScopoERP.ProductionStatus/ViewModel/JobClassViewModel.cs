using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class JobClassViewModel
    {
        public int JobClassID { get; set; }
        public string JobClassName { get; set; }
        public decimal BaseRate { get; set; }
        public decimal? MaxPaid { get; set; }
    }
}
