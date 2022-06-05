namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("inventoryreceive")]
    public partial class inventoryreceive
    {
        public int InventoryReceiveID { get; set; }

        public int ChalanID { get; set; }

        public int WorkSheetID { get; set; }

        public decimal ReceivedQuantity { get; set; }

        public short Status { get; set; }

        public virtual chalan chalan { get; set; }
    }
}
