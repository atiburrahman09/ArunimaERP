using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Repositories;
using ScopoERP.UserManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.UserManagement.BLL
{
    public class AccountLogic
    {
        UnitOfWork unitOfWork;
        public AccountLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<DropDownListViewModel> GetAccountDropdown()
        {
            var data = unitOfWork.AccountRepository.Get()
                .Select(x => new DropDownListViewModel
                {
                    Text = x.AccountName,
                    Value = x.AccountId
                }).AsEnumerable();
            return data;
        }

        public void UpdateUserAccount(UserViewModel model)
        {
            var acc = unitOfWork.UserAccountRepository.Get()
                .Where(x => x.UserId == model.UserID)
                .SingleOrDefault();
            
            if (acc == null)
            {
                acc = new Domain.Models.useraccount()
                {
                    AccountId = (int)model.AccountId,
                    UserId = model.UserID
                };
                unitOfWork.UserAccountRepository.Insert(acc);
            }
            else
            {                
                acc.AccountId = (int)model.AccountId;
                unitOfWork.UserAccountRepository.Update(acc);
            }           
            
            unitOfWork.Save();
        }
    }
}
