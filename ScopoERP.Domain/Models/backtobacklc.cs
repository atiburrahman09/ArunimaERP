namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("backtobacklc")]
    public partial class backtobacklc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public backtobacklc()
        {
            bl = new HashSet<bl>();
            piinfo = new HashSet<piinfo>();
        }

        public int BackToBackLCID { get; set; }

        [Column("BackToBackLC")]
        [Required]
        [StringLength(250)]
        public string BackToBackLC1 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BackToBackLCDate { get; set; }

        public int? SightDays { get; set; }

        public int? MaturityDateCalculation { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SpecificDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MaturityDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BackToBackShippedDate { get; set; }

        public int? JobID { get; set; }

        public bool Status { get; set; }

        public decimal? BackToBackLCValue { get; set; }

        public int? LCTypeID { get; set; }

        public virtual jobinfo jobinfo { get; set; }

        public virtual paymenttype paymenttype { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bl> bl { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<piinfo> piinfo { get; set; }
    }
}
