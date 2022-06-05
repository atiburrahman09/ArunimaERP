using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ExpenseBudgetReportViewModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public int YearlyBudget { get; set; }
        public int MonthlyBudget { get; set; }
        public int CummulativeBudget { get; set; }
        public int ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseBy { get; set; }
        public string ReferenceNo { get; set; }
    }
}
