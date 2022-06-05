using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Store.ViewModel
{
    public class ChalanViewModel
    {
        public int ChalanID { get; set; }
        public string ChalanNo { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ChalanDate { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string MobileNo { get; set; }
        public string ShippedBy { get; set; }
        public string SealNo { get; set; }

        public int UserID { get; set; }
        public DateTime SetupDate { get; set; }

        public List<ShipmentViewModel> ShipmentList { get; set; }
    }
}
