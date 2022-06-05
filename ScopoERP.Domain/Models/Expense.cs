using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class Expense
    {
        [Required]
        public int ExpenseID { get; set; }
        [Required]
        public int ChartOfAccountID { get; set; }
        [Required]
        public int ExpenseAmount { get; set; }
        [Required]
        public DateTime ExpenseDate { get; set; }
        [Required]
        public string ExpenseBy { get; set; }
        [Required]
        public string Description { get; set; }
        public bool? IsApproved { get; set; }
        public string ReferenceNo { get; set; }
        
        public virtual ChartOfAccount ChartOfAccount { get; set; }

        
    }
}
