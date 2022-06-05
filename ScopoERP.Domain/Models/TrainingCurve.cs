using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class TrainingCurve
    {
        public int TrainingCurveID { get; set; }
        public int Stage { get; set; }
        public int Curve { get; set; }
        public decimal? Curve_1 { get; set; }
        public decimal? Curve_1A { get; set; }
        public decimal? Curve_2 { get; set; }
        public decimal? Curve_3 { get; set; }
        public decimal? Curve_4 { get; set; }
        public decimal? Curve_5 { get; set; }
        public decimal? Curve_6 { get; set; }
        public decimal? Curve_New { get; set; }
    }
}
