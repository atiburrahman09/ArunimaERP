using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class PostSheetViewModel
    {
        public string  EmployeeName { get; set; }
        public string EmployeeCardNo { get; set; }
        public int Section { get; set; }
        public string Supervisor { get; set; }
        public double? WABR { get; set; }
        public decimal BaseRate { get; set; }
        public double? OT { get; set; }
        public double? OTTH { get; set; }
        public double? PP { get; set; }
        public double? E { get; set; }
        public double? NP { get; set; }
        public double? TotalPaid { get; set; }
        public double? CSD { get; set; }
        public double? O { get; set; }
        public double? P { get; set; }
        public double? TH { get; set; }
        public double? SH { get; set; }
        public double? SAH { get; set; }
        public double? EM { get; set; }
        public double? PM { get; set; }
        public DateTime? WorkingDate { get; set; }
        public string OperationName { get; set; }
        public int OperationID { get; set; }
        public string NW { get; set; }
        public double? NWEGP { get; set; }
        public double? NWDuration { get; set; }
        public string MT { get; set; }
        public double? MTEGP { get; set; }
        public double? MTDuration { get; set; }
        public string MISC { get; set; }
        public double? MISCEGP { get; set; }
        public double? MISCDuration { get; set; }
        public string BU { get; set; }
        public double? BUEGP { get; set; }
        public double? BUDuration { get; set; }
        public string TB { get; set; }
        public double? TBEGP { get; set; }
        public double? TBDuration { get; set; }


    }
}
