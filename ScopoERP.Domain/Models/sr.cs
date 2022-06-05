namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sr")]
    public partial class sr
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sr()
        {
            inventoryissue = new HashSet<inventoryissue>();
        }

        public int SRID { get; set; }

        [Required]
        [StringLength(200)]
        public string SRNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime IssuedDate { get; set; }

        [StringLength(200)]
        public string IssuedBy { get; set; }

        [Required]
        [StringLength(200)]
        public string ReceiverName { get; set; }

        public int FloorLineID { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        [StringLength(400)]
        public string Remarks { get; set; }

        public short Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventoryissue> inventoryissue { get; set; }
    }
}
