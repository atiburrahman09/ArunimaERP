using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class RequisitionReportViewModel
    {
        public int JobID { get; set; }
        public string JobNo { get; set; }
        public string ContractNo { get; set; }
        public decimal? ContractValue { get; set; }
        public decimal? BudgetValue { get; set; }
        public decimal? AgreedCM { get; set; }
        public decimal? BackToBackLCValue { get; set; }
        public decimal? PendingB2BLCValue { get; set; }
        public decimal? AppliedForB2BLCValue { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? AvailableBudget { get; set; }
        public string RequisitionNo { get; set; }
        public int? RequisitionSLNo { get; set; }
        public DateTime? RequisitionDate { get; set; }
        public int OrderQuantity { get; set; }

        public DateTime? FirstShipmentDate { get; set; }
        public DateTime? LastShipmentDate { get; set; }
    }

    public class RequisitionPO
    {
        public string PONo { get; set; }
        public DateTime? ExitDate { get; set; }
        public decimal? TotalFOB { get; set; }
        public decimal? TotalBudget { get; set; }
        public decimal? TotalB2BLCValue { get; set; }
    }
}
