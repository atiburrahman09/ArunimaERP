using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.LC.ViewModel
{
    public class JobViewModel
    {
        public int JobId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string JobNo { get; set; }
        public string ContractNo { get; set; }
        public DateTime? ContractDate { get; set; }
        public string ExtraContractNo { get; set; }
        public int? BankID { get; set; }
        public string BankName { get; set; }

        public decimal? BudgetPercentage { get; set; }
        public decimal? AdvancedCMPercentage { get; set; }
        public decimal? Limit { get; set; }
        public int? SightDays { get; set; }

        public string UDNo { get; set; }
        public DateTime? UDDate { get; set; }

        public bool IsClose { get; set; }


        public string ShippedTo { get; set; }
        public string NotifyParty { get; set; }
        public string AlsoNotifyParty { get; set; }
        public string CATNO { get; set; }

        public bool? IsClosed { get; set; }
    }
}
