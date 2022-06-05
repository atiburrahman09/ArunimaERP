namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sampletype")]
    public partial class sampletype
    {
        public int SampleTypeID { get; set; }

        [Required]
        [StringLength(200)]
        public string SampleTypeName { get; set; }

        public string UserID { get; set; }

        public DateTime SetDate { get; set; }
    }
}
