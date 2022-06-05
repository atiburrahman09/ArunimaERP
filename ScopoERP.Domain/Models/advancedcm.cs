namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("advancedcm")]
    public partial class advancedcm
    {
        public int AdvancedCMID { get; set; }

        public int JobID { get; set; }

        public int PIID { get; set; }

        public decimal PIValue { get; set; }

        public bool UDStatus { get; set; }

        public decimal ConversionRate { get; set; }

        public decimal? ReceivedAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReceivedDate { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int UserID { get; set; }

        public DateTime SetupDate { get; set; }

        public virtual jobinfo jobinfo { get; set; }

        public virtual piinfo piinfo { get; set; }
    }
}
