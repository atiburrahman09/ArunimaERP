using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.BLL
{
    public class DepartmentLogic
    {
        private UnitOfWork unitOfWork;

        public DepartmentLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<DropDownListViewModel> GetDepartmentDropDown()
        {
            var result = (from s in unitOfWork.DepartmentRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.DepartmentID,
                              ValueInt = s.DepartmentID,
                              Text = s.DepartmentName
                          }).ToList();

            return result;
        }
    }
}
