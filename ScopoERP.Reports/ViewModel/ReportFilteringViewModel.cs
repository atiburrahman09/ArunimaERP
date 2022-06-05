using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ReportFilteringViewModel
    {
        public int? BuyerID { get; set; }
        public string BuyerName { get; set; }

        public int JobID { get; set; }
        public string JobNo { get; set; }

        public int PIID { get; set; }
        public string PINo { get; set; }
        public DateTime? PIDate { get; set; }
        
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime AsOnDate { get; set; }

        public int RequisitionID { get; set; }
        public string RequisitionNo { get; set; }

        public string Factory { get; set; }
        public string Floor { get; set; }
        public int Decision { get; set; }
        public int FactoryMode { get; set; }

        // Extras
        public decimal? ContractValue { get; set; }
        public decimal? OpenedB2BLCValue { get; set; }

        // Contract Paper
        public string ContractNo { get; set; }
        public DateTime? ContractDate { get; set; }
        public string AmendmentNo { get; set; }
        public DateTime? AmendmentDate { get; set; }
        public int? PreviousQuantity { get; set; }
        public decimal? PreviousValue { get; set; }
        public string ChangeText { get; set; }
        public string AmountInWords { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Destination { get; set; }
        public string DestinationChangedText { get; set; }

        public Boolean IsShow { get; set; }

        // Bank Forwarding
        public int BankForwardingID { get; set; }
        public string BankForwardingNo { get; set; }
        public DateTime BankForwardingDate { get; set; }
        public int ShipmentMode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }

        public Boolean InBDT { get; set; }

        public int PurchaseRequisitionID { get; set; }

        public int PurchaseOrderID { get; set; }
        public int InvoiceId { get; set; }

        public string MembershipNo { get; set; }
        public string BondLicenseNo { get; set; }
        public DateTime BondLicenseDate { get; set; }
        public string ReferenceNo { get; set; }
        public int StyleID { get; set; }
        public int CuttingPlanID { get; set; }
        public int OperationCategoryID { get; set; }
    }
}
