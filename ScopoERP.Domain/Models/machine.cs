namespace ScopoERP.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("machine")]
    public partial class machine
    {
        public int MachineID { get; set; }

        [Required]
        [StringLength(200)]
        public string MachineCode { get; set; }

        public int AG { get; set; }
        public int Unit { get; set; }
        public string BrandName { get; set; }
        public string MobileNo { get; set; }
        public int? MachineCondition { get; set; }
        public string LoanTo { get; set; }
        public string LoanFrom { get; set; }
        public string MCValue { get; set; }
        public string BookValue { get; set; }
        public string Remarks { get; set; }

        public int MachineCategoryID { get; set; }

        public int UserID { get; set; }

        public DateTime SetupDate { get; set; }

        public virtual machinecategory machinecategory { get; set; }

      
    }
}
