namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("subcontract")]
    public partial class subcontract
    {
        public int SubContractID { get; set; }

        [Required]
        [StringLength(200)]
        public string SubContractNo { get; set; }

        public int FactoryID { get; set; }

        public int PurchaseOrderID { get; set; }

        public int SubContractQuantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime SubContractExitDate { get; set; }

        public decimal SubContractRate { get; set; }

        public decimal? CommercialPercentage { get; set; }

        [Required]
        [StringLength(500)]
        public string Remarks { get; set; }

        public int UserID { get; set; }

        public DateTime SetupDate { get; set; }

        public virtual factory factory { get; set; }

        public virtual postyle postyle { get; set; }
    }
}
