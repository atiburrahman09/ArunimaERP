using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class Budget
    {
        public int BudgetID { get; set; }
        public int ChartOfAccountID { get; set; }
        public int Month { get; set; }
        public int BudgetAmount { get; set; }

        public virtual ChartOfAccount ChartOfAccount { get; set; }
    }
}
