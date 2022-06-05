using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ShipmentDetailsReportViewModel
    {
        public string BuyerName { get; set; }
        public string StyleNo { get; set; }
        public string StyleDescription { get; set; }
        public string PoNo { get; set; }
        public int OrderQuantity { get; set; }
        public string Remarks { get; set; }

        public string FactoryName { get; set; }
        
        public long? TotalShipMent { get; set; }
        public DateTime? ShippedDate { get; set; }
        public long? BalanceShipQty { get; set; }
        
        public decimal TotalAgreedCM { get; set; }
        public decimal TotalFob { get; set; }
        public decimal? TotalShippedValue { get; set; }
        public decimal? TotalShippedAgreedCM { get; set; }

        public string JobNo { get; set; }
        public string ContractNo { get; set; }

        public string UDNo { get; set; }
        public DateTime? UDDate { get; set; }

        public string InvoiceNo { get; set; }
        public decimal? InvoiceFOB { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? InvoiceValue { get; set; }
        public DateTime? DocumentSentDate { get; set; }
        public DateTime? TradeCardInPutDate { get; set; }
        public string BL { get; set; }
        public DateTime? B_LDate { get; set; }
        public string EXP { get; set; }
        public DateTime? EXPDate { get; set; }
        public string FCR { get; set; }
        public DateTime? FCRDate { get; set; }
        public DateTime? BookingExFactoryDate { get; set; }
        public DateTime? ExFactoryDate { get; set; }
        public DateTime? BankNegoDate { get; set; }
        public string FDBP_No { get; set; }
        public string BankForwardingNo { get; set; }
        public DateTime? BankForwardingDate { get; set; }

        public DateTime? RealizationDate { get; set; }
        public decimal? TotalRealizationValue { get; set; }

        public DateTime? OnBoardDate { get; set; }
        public DateTime? CODate { get; set; }
        public DateTime? ICDate { get; set; }
        public DateTime? BLRealeaseDate { get; set; }
        public DateTime? DocDespatchDate { get; set; }
        public string Courier { get; set; }
        public DateTime? PaymentReceiveDate { get; set; }
        public string ShippingBill { get; set; }
        public DateTime? ShippingBillDate { get; set; }

        public string PortOfLoading { get; set; }
        public string FinalDestination { get; set; }
        public string CountryName { get; set; }

        public decimal? TotalSewing { get; set; }
        public decimal? TotalFinishing { get; set; }
    }
}
