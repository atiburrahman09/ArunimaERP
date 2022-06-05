using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.BLL
{
    public class LCTypeLogic
    {
        private UnitOfWork unitOfWork;

        public LCTypeLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<DropDownListViewModel> GetLCTypeDropDown()
        {
            var result = (from s in unitOfWork.LCTypeRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.PaymentTypeID,
                              Text = s.PaymentTitle
                          }).ToList();

            return result;
        }
    }
}
