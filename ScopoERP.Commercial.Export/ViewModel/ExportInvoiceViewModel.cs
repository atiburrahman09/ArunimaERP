using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScopoERP.Domain;
using System.ComponentModel.DataAnnotations;
using ScopoERP.Store.ViewModel;

namespace ScopoERP.Commercial.ViewModel
{
    public class ExportInvoiceViewModel
    {
        public int? JobID { get; set; }

        public int InvoiceId { get; set; }
        

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }

        [Display(Name = "Invoice Date")]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "Invoice Value")]
        public Decimal? InvoiceValue { get; set; }

        [Display(Name = "Document Sent Date")]
        public DateTime? DocumentSentDate { get; set; }

        [Display(Name = "Trade CardInPut Date")]
        public DateTime? TradeCardInPutDate { get; set; }
        public string BL { get; set; }

        [Display(Name = "B/L Date")]
        public DateTime? B_LDate { get; set; }

        [Display(Name = "BL To Be Endorsed To")]
        public string BLToBeEndorsedTo { get; set; }

        public string EXP { get; set; }

        [Display(Name = "EXP Date")]
        public DateTime? EXPDate { get; set; }

        public string FCR { get; set; }

        [Display(Name = "FCR Date")]
        public DateTime? FCRDate { get; set; }

        [Display(Name = "Bookibg Factory Date")]
        public DateTime? BookingExFactoryDate { get; set; }

        [Display(Name = "Ex Factory Date")]
        public DateTime? ExFactoryDate { get; set; }

        [Display(Name = "Bank Nego Date")]
        public DateTime? BankNegoDate { get; set; }

        [Display(Name = "FDBP No")]
        public string FDBP_No { get; set; }

        [Display(Name = "On Board Date")]
        public DateTime? OnBoardDate { get; set; }

        [Display(Name = "C/O Date")]
        public DateTime? CODate { get; set; }

        [Display(Name = "I.C Date")]
        public DateTime? ICDate { get; set; }

        [Display(Name = "B/L Release Date")]
        public DateTime? BLRealeaseDate { get; set; }

        [Display(Name = "Doc Despatch Date")]
        public DateTime? DocDespatchDate { get; set; }
        [Display(Name = "Doc Payment App. Date")]
        public DateTime? DocsPaymentApprovalDate { get; set; }
        public string Courier { get; set; }

        [Display(Name = "Payment Receive Date")]
        public DateTime? PaymentReceiveDate { get; set; }

        [Display(Name = "Shipping  Bill")]
        public string ShippingBill { get; set; }

        [Display(Name = "Shipping Bill Date")]
        public DateTime? ShippingBillDate { get; set; }

        public HttpPostedFileBase InvoiceFile { get; set; }

        public string PortOfLoading { get; set; }
        public string FinalDestination { get; set; }
        public string CountryName { get; set; }

        public int? BankForwardingID { get; set; }

        public List<ShipmentViewModel> ShipmentList { get; set; }
    }
}