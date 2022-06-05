using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Domain.Models
{
    public class JobClass
    {
        public int JobClassID { get; set; }
        public string JobClassName { get; set; }
        public decimal BaseRate { get; set; }
        public decimal? MaxPaid { get; set; }

        public ICollection<JobClass> JobClasses;
    }
}
