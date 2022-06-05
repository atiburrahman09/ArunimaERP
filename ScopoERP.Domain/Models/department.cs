namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("department")]
    public partial class department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public department()
        {
            purchaserequisition = new HashSet<purchaserequisition>();
        }

        public int DepartmentID { get; set; }

        [Required]
        [StringLength(200)]
        public string DepartmentName { get; set; }

        public int UserID { get; set; }

        public DateTime SetupDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<purchaserequisition> purchaserequisition { get; set; }
    }
}
