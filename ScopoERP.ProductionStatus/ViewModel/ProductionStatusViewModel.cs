using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Production.ViewModel
{
    public class ProductionStatusViewModel
    {
        public int ProductionDailyReportID { get; set; }
        public DateTime Date { get; set; }
        public int? Hour { get; set; }

        public int BuyerID { get; set; }
        public string BuyerName { get; set; }

        public int StyleID { get; set; }
        public string StyleNo { get; set; }

        public int PurchaseOrderID { get; set; }
        public string PONo { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Floor { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Line { get; set; }

        public string Color { get; set; }

        public long? Cutting { get; set; }
        public long? SentPrintEmb { get; set; }
        public long? ReceivedPrintEmb { get; set; }
        public long? SewingInput { get; set; }
        public long? TodaySewing { get; set; }
        public long? SentWash { get; set; }
        public long? ReceivedWash { get; set; }
        public long? TodayFinish { get; set; }
    }
}
