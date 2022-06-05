using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public  class EmployeeCapabilityViewModel
    {
        //public int EmployeeCapabilityID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCardNo { get; set; }
        public string EmployeeName { get; set; }
    }
}
