using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Finance.ViewModel
{
    public class RealizationViewModel
    {
        public int? RealizationID { get; set; }
        public int? BankForwardingID { get; set; }
        public string BankForwardingNo { get; set; }
        public string FDBPNo { get; set; }

        public DateTime? RealizationDate { get; set; }
        
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }

        public decimal? Amount { get; set; }
        public decimal? CurrencyRate { get; set; }

        public int? UserID { get; set; }
        public DateTime? SetDate { get; set; }

        public decimal? InvoiceValue { get; set; }
        public decimal? TotalRealizedValue { get; set; }
    }
}
