namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bldetails
    {
        public int BLDetailsID { get; set; }

        public int BLID { get; set; }

        public int BookingID { get; set; }

        public decimal? InvoiceQuantity { get; set; }
        public string ActualItemDescription { get; set; }

        public decimal? ReceivedQuantity { get; set; }

        public int UserID { get; set; }

        public DateTime SetupDate { get; set; }

        public virtual bl bl { get; set; }
        
    }
}
