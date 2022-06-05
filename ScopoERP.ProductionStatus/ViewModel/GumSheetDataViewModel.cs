using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class GumSheetDataViewModel
    {
        public decimal BaseRate { get; set; }
        public double ProductionTotalDuration { get; set; }
        public double ProductionEarn { get; set; }
        public double OffStandardTotalDuration { get; set; }
        public double OffStandardEarn { get; set; }
        public double ClockedTime { get; set; }
    }
}
