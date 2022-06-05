using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.ViewModel
{
    public class ShipmentPerDay
    {
        public ShipmentPerDay()
        {
            Dates = new List<int>();
            Amounts = new List<int>();
        }
        public List<int> Dates { get; set; }
        public List<int> Amounts { get; set; }
    }
}
