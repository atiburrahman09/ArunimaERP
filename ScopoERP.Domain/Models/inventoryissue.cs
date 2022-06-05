namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("inventoryissue")]
    public partial class inventoryissue
    {
        public int InventoryIssueID { get; set; }

        public int SRID { get; set; }

        public int ItemID { get; set; }

        public decimal? IssuedQuantity { get; set; }
        public decimal? RequestedQuantity { get; set; }

        public short Status { get; set; }

        public int PoStyleId { get; set; }

        public virtual sr sr { get; set; }
        public virtual postyle ps { get; set; }
    }
}
