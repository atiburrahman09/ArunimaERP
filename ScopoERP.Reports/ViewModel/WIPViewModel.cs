using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class WIPViewModel
    {
        [Display(Name = "Buyer")]
        public string BuyerName { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        public string AccountName { get; set; }
        public string BodyStyle { get; set; }
        public string StyleNo { get; set; }
        public string PoNo { get; set; }
        public string Remarks { get; set; }
        public string DevisionName { get; set; }
        public string Item { get; set; }
        public int OrderQuantity { get; set; }
        public int Capacity { get; set; }
        public string FactoryName { get; set; }
        public decimal? SubContractRate { get; set; }
        public decimal? FactoryCM { get; set; }

        public System.DateTime? RmReadyDate { get; set; }
        public System.DateTime? DeliveryDate { get; set; }
        public int PoExitMonth { get; set; }
        public int PoExitWeek { get; set; }
        public System.DateTime ExitDate { get; set; }
        public System.DateTime? ProductionStartDate { get; set; }
        public int Production_L_Time { get; set; }
        public int ProductionDays { get; set; }

        public int? CurrentStatus { get; set; }

        public long? TotalCutting { get; set; }
        public long? TotalSewing { get; set; }
        public long? BalanceSewing { get; set; }

        public long? TotalShipMent { get; set; }
        public DateTime? ShippedDate { get; set; }
        public long? BalanceShipQty { get; set; }
        public int ActualDayToComplete { get; set; }

        public decimal? ReserveFundInPc { get; set; }
        public decimal? TotalReserveFund { get; set; }

        public decimal? ActualCM { get; set; }
        public decimal AgreedCm { get; set; }
        public decimal TotalAgreedCM { get; set; }
        public decimal Fob { get; set; }
        public decimal TotalFob { get; set; }
        public decimal? TotalShippedValue { get; set; }
        public decimal? FabricValue { get; set; }
        public decimal? TrimsValue { get; set; }
        public decimal? EmbPrintValue { get; set; }
        public decimal? WashValue { get; set; }

        public decimal? SubRate { get; set; }
        public decimal? SubQuantity { get; set; }

        public string JobNo { get; set; }
        public string ContractNo { get; set; }

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
        public DateTime? OnBoardDate { get; set; }
        public DateTime? CODate { get; set; }
        public DateTime? ICDate { get; set; }
        public DateTime? BLRealeaseDate { get; set; }
        public DateTime? DocDespatchDate { get; set; }
        public string Courier { get; set; }
        public DateTime? PaymentReceiveDate { get; set; }
        public string ShippingBill { get; set; }
        public DateTime? ShippingBillDate { get; set; }

        public string SeasonName { get; set; }
        public string StyleDescription { get; set; }
        public string Febrication { get; set; }
    }
}
