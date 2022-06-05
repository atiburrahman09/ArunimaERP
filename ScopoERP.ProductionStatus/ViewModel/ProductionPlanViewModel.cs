using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.ViewModel
{
    public class ProductionPlanViewModel
    {
        public int ProductionPlanningID { get; set; }
        public int PoStyleID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int FloorLineID { get; set; }
        public int Quantity { get; set; }
        public int StyleID { get; set; }
        public string StyleNo { get; set; }
        public int Capacity { get; set; }
        public DateTime ExitDate { get; set; }
        public string FloorName { get; set; }
        public string PoNo { get; set; }
        

        public int resource
        {
            get { return FloorLineID; }
        }
        public string title
        {
            get { return PurchaseOrderNo; }
        }
        public string start
        {
            get { return StartDate.ToString("yyyy-MM-dd"); }
        }
        public string end
        {
            get { return EndDate.ToString("yyyy-MM-dd"); }
        }

        public string color
        {
            get 
            {
                if (EndDate > ExitDate)
                    return "#DB0909";
                else
                    return "#3366cc";
            }
        }
        
    }
}
