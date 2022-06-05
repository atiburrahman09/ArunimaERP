using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.ViewModel
{
    public class SeasonViewModel
    {
        public int SeasonId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public String SeasonName { get; set; }
    }
}
