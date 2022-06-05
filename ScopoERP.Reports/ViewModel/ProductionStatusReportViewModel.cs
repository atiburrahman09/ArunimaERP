using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ProductionStatusReportViewModel
    {
        public string StyleNo { get; set; }
        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public long? CuttingQuantity { get; set; }
        public long? SewingQuantity { get; set; }
        public long? WashSendQuantity { get; set; }
        public long? WashReceivedQuantity { get; set; }
        public long? PrintEmbSentQuantity { get; set; }
        public long? PrintEmbReceivedQuantity { get; set; }
        public long? FinishedQuantity { get; set; }
        public int? ShipmentQuantity { get; set; }
        public decimal? Wastage { get; set; }
        public decimal TotalFOB { get; set; }
        public decimal? SewingFOB { get; set; }
        public decimal? InvoiceFOB { get; set; }

        public decimal TotalAgreedCM { get; set; }
        public decimal? SewingCM { get; set; }
        public decimal? ShipmentCM { get; set; }

        public decimal? TotalRMCost { get; set; }
        public decimal? SewingRMCost { get; set; }
        public decimal? ShipmentRMCost { get; set; }

        public string Floor { get; set; }
        public string Line { get; set; }
    }

    public class ProductionStatusFromat2ViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime ReceivedCRD { get; set; }
        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }
        public string StyleNo { get; set; }
        public string StyleDescription { get; set; }
        public string ShipMode { get; set; }
        public string DCCode { get; set; }
        public int OrderQuantity { get; set; }

        public long? CuttingQuantity { get; set; }
        public long? CuttingBalanceQuantity { get; set; }

        public long? ProductionCompletedQuantity { get; set; }
        public long? ProductionBalanceQuantity { get; set; }

        public long? FinishingCompletedQuantity { get; set; }
        public long? FinishingBalanceQuantity { get; set; }

        public long? WashCompletedQuantity { get; set; }
        public long? WashBalanceQuantity { get; set; }

        public long? PackingCompletedQuantity { get; set; }
        public long? PackingBalanceQuantity { get; set; }

        public long? ShippedQuanity { get; set; }
        public long? ShippedBalanceQuanity { get; set; }

        public string Remarks { get; set; }
    }

    public class ProductionStatusFromat3ViewModel
    {
        public int? PurchaseOrderID { get; set; }
        public string Factory { get; set; }
        public string Line { get; set; }
        public string PONo { get; set; }
        public DateTime ReceivedCRD { get; set; }
        public DateTime? OriginalCRD { get; set; }
        public string StyleNo { get; set; }
        public string StyleDescription { get; set; }
        public int OrderQuantity { get; set; }

        public string ColorName { get; set; }

        public long? CuttingQuantity { get; set; }
        public long? TotalCuttingQuantity { get; set; }

        public long? TotalSewingInputQuantity { get; set; }
        public long? TotalSewingInputBalanceQuantity { get; set; }
        public long? SewingQuantity { get; set; }
        public long? TotalSewingQuantity { get; set; }
        public long? TotalSewingBalanceQuantity { get; set; }

        public long? SentForWashQuantity { get; set; }
        public long? TotalSentForWashQuantity { get; set; }
        public long? WashQuantity { get; set; }
        public long? TotalWashQuantity { get; set; }
        public long? TotalWashBalanceQuantity { get; set; }

        public long? PolyPackQuantity { get; set; }
        public long? TotalPolyPackQuantity { get; set; }
        public long? TotalPolyPackBalanceQuantity { get; set; }

        public DateTime? FinalAuditDatePlaned { get; set; }
        public DateTime? FinalAuditDateActual { get; set; }

        public int? ShippedQuantity { get; set; }
        public int? ShortOrOverQuantity { get; set; }

        public DateTime? ExFactory { get; set; }

        public string Remarks { get; set; }
    }
}
