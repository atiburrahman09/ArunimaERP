namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("approval")]
    public partial class approval
    {
        public int ApprovalId { get; set; }

        public int StyleId { get; set; }

        [Required]
        [StringLength(200)]
        public string SampleType { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int Status { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public virtual styleinfo styleinfo { get; set; }
    }
}
