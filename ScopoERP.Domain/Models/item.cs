namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("item")]
    public partial class item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public item()
        {
            booking = new HashSet<booking>();
            initialcostsheet = new HashSet<initialcostsheet>();
            worksheets = new HashSet<worksheets>();
        }

        public int ItemId { get; set; }

        [StringLength(100)]
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public int ItemCategoryId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<booking> booking { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<initialcostsheet> initialcostsheet { get; set; }

        public virtual itemcategory itemcategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<worksheets> worksheets { get; set; }
    }
}
