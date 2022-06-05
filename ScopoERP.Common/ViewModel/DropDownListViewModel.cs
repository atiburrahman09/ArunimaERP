using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.ViewModel
{
    public class DropDownListViewModel
    {
        public int Value { get; set; }
        public string ValueString { get; set; }
        public int ValueInt { get; set; }
        public string Text { get; set; }
        public decimal CouponTime { get; set; }
        public decimal? CouponValue { get; set; }
    }
}
