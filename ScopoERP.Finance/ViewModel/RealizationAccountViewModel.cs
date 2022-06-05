using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Finance.ViewModel
{
    public class RealizationAccountViewModel
    {
        public int RealizationAccountID { get; set; }
        public string RealizationAccountNo { get; set; }
        public string RealizationAccountName { get; set; }
        public int RealizationAccountType { get; set; }
        public int UserID { get; set; }
        public DateTime SetDate { get; set; }
    }
}
