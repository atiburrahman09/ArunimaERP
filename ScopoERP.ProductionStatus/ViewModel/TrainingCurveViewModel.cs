using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class TrainingCurveViewModel
    {
        public int TrainingCurveID { get; set; }
        [Required]
        public int Stage { get; set; }
        [Required]
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
