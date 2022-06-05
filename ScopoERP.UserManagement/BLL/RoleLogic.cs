using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.UserManagement.BLL
{
    public class RoleLogic
    {
        private UnitOfWork unitOfWork;

        public RoleLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<DropDownListViewModel> GetRoleDropDown()
        {
            var result = (from c in unitOfWork.RoleRepository.Get()
                          select new DropDownListViewModel
                          {
                              Text = c.RoleName,
                              ValueString = c.RoleId.ToString()
                          }).AsEnumerable();

            return result;
        }
    }
}
