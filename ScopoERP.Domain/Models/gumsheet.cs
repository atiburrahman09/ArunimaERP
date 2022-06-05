using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class gumsheet
    {
        [Key]
        public int GumSheetID { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? EmployeeCardNo { get; set; }
        public string BundleNo { get; set; }
        public decimal Duration { get; set; }
        public decimal? EGP { get; set; }
        public int? SpecID { get; set; }
        public decimal ClockedTime { get; set; }
        public int Section { get; set; }
        public bool MachineTrouble { get; set; }
        public bool PayMethod { get; set; }


        public double? LearningCurve { get; set; } 
        public double? Allowance { get; set; }

    }
}
