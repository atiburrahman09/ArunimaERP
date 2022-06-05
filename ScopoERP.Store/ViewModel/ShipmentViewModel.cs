using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Store.ViewModel
{
    public class ShipmentViewModel
    {
        public int ShipmentID { get; set; }

        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }
        public int OrderQuantity { get; set; }

        public int? ChalanID { get; set; }
        public string ChalanNo { get; set; }
        public DateTime? ChalanDate { get; set; }

        [Required]
        public int ChalanQuantity { get; set; }

        public decimal? CBM { get; set; }
        public decimal? CartoonQuantity { get; set; }

        public string FactoryName { get; set; }
        public string Floor { get; set; }

        public int? InvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? InvoiceFOB { get; set; }
        public string Destination { get; set; }

        public decimal? ShippedFOB { get; set; }

        public int UserID { get; set; }
        public DateTime SetupDate { get; set; }
    }
}
