using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class CommercialInvoiceReportVM
    {
        public string InvoiceNo { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        public string ExportNo { get; set; }
        public Nullable<DateTime> ExportDate { get; set; }
        public string Department { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string Terms { get; set; }
        public string FOBNo { get; set; }
        public Nullable<DateTime> FOBDate { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public Nullable<DateTime> DateOfExport { get; set; }
        public string FOBPoint { get; set; }
        public string Destination { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string SwiftCode { get; set; }
        public string AccountNo { get; set; }
        public string BuyerName { get; set; }
        public string BuyerAddress { get; set; }
        public string AlsoNotifyParty { get; set; }
        public string NotifyParty { get; set; }
        public string ContractNo { get; set; }
        public Nullable<DateTime> ContractDate { get; set; }
        public string PortOfLoading { get; set; }
        public string Remarks { get; set; }
        public string CATNO { get; set; }
    }
    public class CommercialInvoiceDescriptionVM
    {
        public int QTY { get; set; }
        public string PONo { get; set; }
        public string TargetPO { get; set; }
        public string StyleNo { get; set; }
        public string DPCIItem { get; set; }
        public string Color { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? CTNQuantity { get; set; }
    }
}
