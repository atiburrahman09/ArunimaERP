namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("buyerinfo")]
    public partial class buyerinfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public buyerinfo()
        {
            styleinfo = new HashSet<styleinfo>();
        }

        [Key]
        public int BuyerId { get; set; }

        [Required]
        [StringLength(100)]
        public string BuyerName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<styleinfo> styleinfo { get; set; }
    }
}
