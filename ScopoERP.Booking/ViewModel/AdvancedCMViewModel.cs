using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.ViewModel
{
    public class AdvancedCMViewModel
    {
        public int AdvancedCMID { get; set; }

        public int JobID { get; set; }
        public string JobNo { get; set; }

        public int PIID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PINo { get; set; }
        public DateTime? PIDate { get; set; }
        public decimal PIValue { get; set; }

        public int SupplierID { get; set; }
        public string SupplierName { get; set; }

        public Boolean UDStatus { get; set; }

        public decimal ConversionRate { get; set; }
        public decimal ReceivableAmount { get; set; }
        public decimal? ReceivedAmount { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public decimal? DifferenceFromReceivable { get; set; }

        public string Remarks { get; set; }

        public string BackToBackLC { get; set; }
        public DateTime? BackToBackLCDate { get; set; }

        public int UserID { get; set; }
        public DateTime SetupDate { get; set; }
    }
}
