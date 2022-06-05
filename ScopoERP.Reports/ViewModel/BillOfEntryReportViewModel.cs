using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class BillOfEntryReportViewModel
    {
        public string JobNo { get; set; }
        public string ContractNo { get; set; }
        public DateTime? ContractDate { get; set; }
        public decimal ContractValue { get; set; }
        public int OrderQty { get; set; }
        public string BankName { get; set; }
        public string UDNo { get; set; }
        public DateTime? UDDate { get; set; }
        public string BillOfEntryDesc { get; set; }
        public int GoodsQTY { get; set; }
        public string Unit { get; set; }
        public decimal InvoiceValue { get; set; }
        public string BillOfEntryNo { get; set; }
        public DateTime? BillOfEntryDate { get; set; }
        public string LCType { get; set; }
    }
}
