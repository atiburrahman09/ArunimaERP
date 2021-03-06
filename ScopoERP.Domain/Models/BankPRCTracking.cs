using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("bankPrcTracking")]
    public partial class BankPRCTracking
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string TrackingNo { get; set; }
        [Column(TypeName = "date")]
        public DateTime TrackingDate { get; set; }                
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
