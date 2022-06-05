using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Reports.ViewModel
{
    public class ShipmentDetailsViewModel
    {
        public string FactoryName { get; set; }
        public string BuyerName { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime ExitDate { get; set; }

        public string ChalanNo { get; set; }
        public DateTime? ChalanDate { get; set; }
        public int ChalanQuantity { get; set; }

        public decimal? AdvanceCM { get; set; }
        public decimal TotalAgreedCM { get; set; }
        public decimal ShippedAgreedCM { get; set; }
        public decimal? TotalAdvanceCM { get; set; }
        public decimal? TotalRemainCM { get; set; }
        public decimal TotalFOB { get; set; }
        public decimal? ShippedFOB { get; set; }
        public decimal? InvoiceFOB { get; set; }
        
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? OnBoardDate { get; set; }
        public string BL { get; set; }
        public DateTime? BLRealeaseDate { get; set; }
        public string EXP { get; set; }
        public DateTime? EXPDate { get; set; }
        public string FCR { get; set; }
        public DateTime? FCRDate { get; set; }
        public string ShippingBill { get; set; }
        public DateTime? ShippingBillDate { get; set; }
        public DateTime? DocDespatchDate { get; set; }
        public string FDBPNo { get; set; }
        public DateTime? RealizationDate { get; set; }
        public decimal? RealizationValue { get; set; }

        public string BankForwardingNo { get; set; }
        public DateTime? BankForwardingDate { get; set; }

        public string PortOfLoading { get; set; }
        public string FinalDestination { get; set; }
        public string CountryName { get; set; }
    }
}
