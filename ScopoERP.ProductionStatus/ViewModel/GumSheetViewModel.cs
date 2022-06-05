using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class GumSheetViewModel
    {
        public int GumSheetID { get; set; }
        public DateTime CompletedDate { get; set; }
        [Required]
        public int? EmployeeCardNo { get; set; }
        [Required]
        public string SpecNo { get; set; }
        [Required]
        public decimal ClockedTime { get; set; }
        public int Section { get; set; }
        public bool MachineTrouble { get; set; }
        public bool PayMethod { get; set; }
        public double LearningCurve { get; set; }
        public double Allowance { get; set; }
        public decimal BaseRate { get; set; }
        public decimal ProductionTotalDuration { get; set; }
        public decimal ProductionEarn { get; set; }
        public decimal OffStandardTotalDuration { get; set; }
        public decimal OffStandardEarn { get; set; }
    }
}
