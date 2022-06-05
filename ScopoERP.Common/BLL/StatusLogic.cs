using ScopoERP.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.BLL
{
    public class StatusLogic
    {
        public List<DropDownListViewModel> GetProcurementStatusDropDown()
        {
            var items = new List<DropDownListViewModel>(new[]
                    {
                        new DropDownListViewModel{Text="Assigned", Value=1},
                        new DropDownListViewModel{Text="Back To Inventory", Value=2},
                    }
                );

            return items;
        }
    }
}
