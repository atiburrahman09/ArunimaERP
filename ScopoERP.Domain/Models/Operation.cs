using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class Operation
    {
        [Key]
        public int OperationID { get; set; }
        public string OperationName { get; set; }
        public string OperationCodeNo { get; set; }
        public int OperationCategoryID { get; set; }
        public int? JobClassID { get; set; }

        public virtual OperationCategory OperationCategories { get; set; }

        public ICollection<Operation> Operations;
        public virtual JobClass JobClasses { get; set; }
        //public virtual ICollection<coupon> coupons { get; set; }

    }
}
