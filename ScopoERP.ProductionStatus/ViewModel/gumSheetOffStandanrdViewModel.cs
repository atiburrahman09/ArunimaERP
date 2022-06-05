using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class gumSheetOffStandanrdViewModel
    {
        public int GumSheetOffStandardID { get; set; }
        public string EmployeeCardNo { get; set; }
        public int SpecID { get; set; }
        public string SpecNo { get; set; }
        public int OperationID { get; set; }
        public int Section { get; set; }
        public DateTime CompletedDate { get; set; }
        public List<OffStandardViewModel> OffStandardVm { get; set; }
      

    }

    public class OffStandardViewModel
    {
        public string OffStandardText { get; set; }
        public float offStandanrdDuration { get; set; }
        public float Value { get; set; }
        public string Remark { get; set; }
    }
}
