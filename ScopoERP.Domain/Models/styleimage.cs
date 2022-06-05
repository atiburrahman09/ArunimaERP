namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("styleimage")]
    public partial class styleimage
    {
        public int StyleImageID { get; set; }

        public int StyleID { get; set; }

        [Required]
        [StringLength(200)]
        public string ImageUrl { get; set; }

        public int UserID { get; set; }

        public DateTime SetupDate { get; set; }

        public virtual styleinfo styleinfo { get; set; }
    }
}
