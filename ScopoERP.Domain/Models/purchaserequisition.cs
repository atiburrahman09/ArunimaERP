namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("purchaserequisition")]
    public partial class purchaserequisition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public purchaserequisition()
        {
            purchaserequisitiondetails = new HashSet<purchaserequisitiondetails>();
            purchaserequisitioninstallment = new HashSet<purchaserequisitioninstallment>();
        }

        public int PurchaseRequisitionID { get; set; }

        [Required]
        [StringLength(50)]
        public string RequisitionNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime RequisitionDate { get; set; }

        public string Remarks { get; set; }

        [StringLength(30)]
        public string Sector { get; set; }

        public int DepartmentID { get; set; }

        public int UserID { get; set; }

        public DateTime SetDate { get; set; }

        public virtual department department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<purchaserequisitiondetails> purchaserequisitiondetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<purchaserequisitioninstallment> purchaserequisitioninstallment { get; set; }
    }
}
