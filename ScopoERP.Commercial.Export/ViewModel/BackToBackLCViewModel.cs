using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Commercial.ViewModel
{
    public class BackToBackLCViewModel
    {
        public int BackToBackLCID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string BackToBackLCNo { get; set; }

        public DateTime? BackToBackLCDate { get; set; }
        public decimal? BackToBackLCValue { get; set; }
        public DateTime? BackToBackShippedDate { get; set; }

        public int? SightDays { get; set; }
        public DateTime? MaturityDate { get; set; }

        [Required]
        public int? JobID { get; set; }
        public string JobNo { get; set; }

        public bool Status { get; set; }

        [Required]
        public int? LCTypeID { get; set; }
        public string LCTypeTitle { get; set; }

        public List<PISummary> PIList { get; set; }
    }

    public class PISummary
    {
        public int? PIID { get; set; }
        public string PINo { get; set; }
        public decimal PIValue { get; set; }
    }
}
