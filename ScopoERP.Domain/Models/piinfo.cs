namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("piinfo")]
    public partial class piinfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public piinfo()
        {
            advancedcm = new HashSet<advancedcm>();
            booking = new HashSet<booking>();
            excessbooking = new HashSet<excessbooking>();
            worksheets = new HashSet<worksheets>();
        }

        [Key]
        public int PIID { get; set; }

        [Required]
        [StringLength(200)]
        public string ReferenceNo { get; set; }

        [StringLength(200)]
        public string PINo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PIDate { get; set; }

        public int? SupplierID { get; set; }

        public int? BackToBackLCID { get; set; }

        public int? RequisitionID { get; set; }

        public int? LoanFromJobID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApproximateInhouseDate { get; set; }

        public int Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<advancedcm> advancedcm { get; set; }

        public virtual backtobacklc backtobacklc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<booking> booking { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<excessbooking> excessbooking { get; set; }

        public virtual jobinfo jobinfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<worksheets> worksheets { get; set; }
    }
}
