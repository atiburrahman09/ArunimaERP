using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
   public class StandardOperationViewModel
    {   
        public int OperationID { get; set; }
        [Required]
        public string OperationName { get; set; }
        [Required]
        public string OperationCodeNo { get; set; }
        public string SpecNo { get; set; }
        public int? JobClassID { get; set; }
        [Required]
        public int OperationCategoryID { get; set; }
    }
}
