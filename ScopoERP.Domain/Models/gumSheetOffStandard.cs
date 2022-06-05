using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class gumSheetOffStandard
    {
        public int GumSheetOffStandardID { get; set; }
        public string EmployeeCardNo { get; set; }
        public int SpecID { get; set; }
        public int OperationID { get; set; }
        public int Section { get; set; }
        public DateTime WorkingDate { get; set; }
        public string NonStandCode { get; set; }
        public float Duration { get; set; }
        public float EGP { get; set; }
        public string Remark { get; set; }
    }
}
