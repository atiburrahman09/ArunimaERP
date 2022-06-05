namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sampleapproval")]
    public partial class sampleapproval
    {
        public int SampleApprovalID { get; set; }

        public int StyleID { get; set; }

        [Required]
        [StringLength(20)]
        public string ApprovalSerialNo { get; set; }

        public int SampleTypeID { get; set; }

        public int Quantity { get; set; }

        
        public DateTime ApproximateSentDate { get; set; }

        
        public DateTime? SentDate { get; set; }

        
        public DateTime? ApproveDate { get; set; }

        public int ApprovalStatus { get; set; }

        public string UserID { get; set; }

        public DateTime SetDate { get; set; }

        public int Validity { get; set; }

        public string Color { get; set; }

        public int Size { get; set; }

        public string CourierName { get; set; }

        public int CourierNo { get; set; }

        public string ApprovalThrough { get; set; }

        public string Remarks { get; set; }
    }
}
