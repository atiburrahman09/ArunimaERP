using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Production.ViewModel
{
    public class MachineViewModel
    {
        public int MachineID { get; set; }
        public string MachineCode { get; set; }

        public int MachineCategoryID { get; set; }
        public string MachineCategoryName { get; set; }
        public bool isChecked { get; set; }

        public int UserID { get; set; }
        public DateTime SetupDate { get; set; }

        public int? AG { get; set; }
        public int? Unit { get; set; }
        public string BrandName { get; set; }
        public string MobileNo { get; set; }
        public int? MachineCondition { get; set; }
        public string LoanTo { get; set; }
        public string LoanFrom { get; set; }
        public string MCValue { get; set; }
        public string BookValue { get; set; }
        public string Remarks { get; set; }
    }
}
