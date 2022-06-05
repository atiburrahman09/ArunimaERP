namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class worksheets
    {
        [Key]
        public int WorksheetId { get; set; }

        public int PoStyleId { get; set; }

        public int ItemId { get; set; }

        [StringLength(100)]
        public string Size { get; set; }

        [StringLength(100)]
        public string Color { get; set; }

        [StringLength(100)]
        public string ItemSize { get; set; }

        [StringLength(100)]
        public string ItemColor { get; set; }

        public decimal Consumption { get; set; }

        public int ConsumptionUnitId { get; set; }

        public decimal Wastage { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalQuantity { get; set; }

        public int? SupplierId { get; set; }

        public int Formula { get; set; }

        public int? PIId { get; set; }

        public int Status { get; set; }

        public virtual consumptionunit consumptionunit { get; set; }

        public virtual item item { get; set; }

        public virtual piinfo piinfo { get; set; }

        public virtual postyle postyle { get; set; }

        public virtual supplier supplier { get; set; }
    }
}
