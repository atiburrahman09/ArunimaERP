using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class POEmployeeMapping
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int ProductionPlanningID { get; set; }
    }
}
