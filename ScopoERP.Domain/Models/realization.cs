namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("realization")]
    public partial class realization
    {
        public int RealizationID { get; set; }

        public int BankForwardingID { get; set; }

        [Column(TypeName = "date")]
        public DateTime RealizationDate { get; set; }

        public int AccountID { get; set; }

        public decimal Amount { get; set; }

        public decimal? CurrencyRate { get; set; }

        public int UserID { get; set; }

        public DateTime SetDate { get; set; }
    }
}
