using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ComparativeStatementViewModel
    {
        public string JobNo { get; set; }
        public string ContractNo { get; set; }
        public decimal? ContractValue { get; set; }
        public Nullable<DateTime> ApprxReceiveDate { get; set; }
        public string BackToBackLCNo { get; set; }
        public decimal? BackToBackLCValue { get; set; }
        public Nullable<DateTime> MaturityDate { get; set; }
    }
}
