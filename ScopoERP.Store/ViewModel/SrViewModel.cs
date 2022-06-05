using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Store.ViewModel
{
    public class SrViewModel
    {
        public int SRID { get; set; }
       
        public string SRNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime IssuedDate { get; set; }

        [StringLength(200)]
        public string IssuedBy { get; set; }

        [Required]
        [StringLength(200)]
        public string ReceiverName { get; set; }

        public int FloorLineID { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        [StringLength(400)]
        public string Remarks { get; set; }

        public short Status { get; set; }

        public List<InventoryIssueViewModel> Inventories { get; set; }
    }
}
