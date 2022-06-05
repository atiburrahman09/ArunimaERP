using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class StyleOperation
    {
        [Key]
        public int StyleOperationID { get; set; }
        public int StyleID { get; set; }
        public int OperationID { get; set; }
        public int? SpecID { get; set; }
        public int SectionNo { get; set; }
        public decimal sam { get; set; }
        public Nullable<decimal> AuxSam { get; set; }
        public int MachineID { get; set; }
        public int SupervisorID { get; set; }
        public string Size { get; set; }

        public ICollection<StyleOperation> StyleOperations;

    }
}
