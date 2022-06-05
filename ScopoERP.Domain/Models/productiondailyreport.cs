namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("productiondailyreport")]
    public partial class productiondailyreport
    {
        public int ProductionDailyReportId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int? Hour { get; set; }

        public int PoStyleId { get; set; }

        [Required]
        [StringLength(20)]
        public string Floor { get; set; }

        [Required]
        [StringLength(15)]
        public string Line { get; set; }

        [StringLength(200)]
        public string Color { get; set; }

        public long? Cutting { get; set; }

        public long? SentPrintEmb { get; set; }

        public long? ReceivedPrintEmb { get; set; }

        public long? SewingInput { get; set; }

        public long? TodaySewing { get; set; }

        public long? SentWash { get; set; }

        public long? ReceivedWash { get; set; }

        public long? TodayFinish { get; set; }

        public virtual postyle postyle { get; set; }
    }
}
