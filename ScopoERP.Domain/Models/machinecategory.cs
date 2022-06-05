namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("machinecategory")]
    public partial class machinecategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public machinecategory()
        {
            machine = new HashSet<machine>();
            machinecategory1 = new HashSet<machinecategory>();
        }

        public int MachineCategoryID { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public int? ParentCategoryID { get; set; }

        public int UserID { get; set; }

        public DateTime SetupDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<machine> machine { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<machinecategory> machinecategory1 { get; set; }

        public virtual machinecategory machinecategory2 { get; set; }
    }
}
