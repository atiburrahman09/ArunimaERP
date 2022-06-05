using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.LC.ViewModel
{
    public class BankViewModel
    {
        public int BankID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string BankName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string BankAddress { get; set; }

        public int UserID { get; set; }
        public DateTime SetDate { get; set; }
    }
}
