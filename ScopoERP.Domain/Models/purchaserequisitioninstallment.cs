namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("purchaserequisitioninstallment")]
    public partial class purchaserequisitioninstallment
    {
        public int PurchaseRequisitionInstallmentID { get; set; }

        public int PurchaseRequisitionID { get; set; }

        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime InstallmentDate { get; set; }

        public decimal? PayableAmount { get; set; }

        public DateTime? PayableDate { get; set; }

        public int UserID { get; set; }

        public DateTime SetDate { get; set; }

        public virtual purchaserequisition purchaserequisition { get; set; }
    }
}
