using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class ChartOfAccount
    {
        public int ChartOfAccountID { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
