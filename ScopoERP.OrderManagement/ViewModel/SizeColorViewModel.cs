using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.OrderManagement.ViewModel
{
    public class SizeColorViewModel
    {
        public int PoStyleID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Color { get; set; }

        public List<SizeQuantity> SizeQuantity { get; set; }
    }

    public class SizeQuantity
    {
        [Required(AllowEmptyStrings = false)]
        public string Size { get; set; }

        public int Quantity { get; set; }

        public Nullable<decimal> FOB { get; set; }
    }

    public class SizeColorDetailsViewModel
    {
        public int PoStyleID { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }
    }
}
