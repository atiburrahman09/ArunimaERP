using ScopoERP.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.BLL
{
    public class PurchaseOrderStatusLogic
    {
        public List<DropDownListViewModel> GetProductionStatusDropDown()
        {
            var items = new List<DropDownListViewModel>(new[]
                    {
                        new DropDownListViewModel{Text="Archive", Value=0},
                        new DropDownListViewModel{Text="In Production", Value=1},
                        new DropDownListViewModel{Text="Part Shipped", Value=2},
                        new DropDownListViewModel{Text="Full Shipped", Value=3},
                        new DropDownListViewModel{Text="Excess Shipped", Value=4},
                        new DropDownListViewModel{Text="Short Shipped", Value=5},                      
                        new DropDownListViewModel{Text="Canceled", Value=7},
                        new DropDownListViewModel{Text="TBA", Value=8},
                    }
                );

            return items;
        }
    }
}
