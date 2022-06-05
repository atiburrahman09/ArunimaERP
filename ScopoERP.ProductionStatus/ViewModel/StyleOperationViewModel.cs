using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class StyleOperationViewModel
    {
        public int StyleOperationID { get; set; }
        public int StyleID { get; set; }
        public int OperationID { get; set; }
        public int? SpecID { get; set; }
        public string StandardOperationName { get; set; }
        public decimal Sam { get; set; }
        public decimal? AuxSam { get; set; }
        public int MachineID { get; set; }
        public int SectionNo { get; set; }
        public int SupervisorID { get; set; }
        public string Size { get; set; }

        public List<SizeListViewModel> SizeListVM { get; set; }
    }


    public class SizeListViewModel
    {
        public string Size { get; set; }
        public decimal Sam { get; set; }
    }
}
