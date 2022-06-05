using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoERP.Common.ViewModel;
using ScopoERP.Finance.ViewModel;
using ScopoERP.Domain.Models;

namespace ScopoERP.Finance.BLL
{
    public class RealizationAccountLogic
    {
        private UnitOfWork unitOfWork;
        private realizationaccount realizationAccount;

        public RealizationAccountLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateRealizationAccount(RealizationAccountViewModel realizationAccountVM)
        {
            realizationAccount = new realizationaccount
            {
                RealizationAccountNo = realizationAccountVM.RealizationAccountNo,
                RealizationAccountName = realizationAccountVM.RealizationAccountName,
                RealizationAccountType = realizationAccountVM.RealizationAccountType,
                UserID = realizationAccountVM.UserID,
                SetDate = realizationAccountVM.SetDate
            };

            unitOfWork.RealizationAccountRepository.Insert(realizationAccount);
            unitOfWork.Save();
        }

        public void UpdateRealizationAccount(RealizationAccountViewModel realizationAccountVM)
        {
            realizationAccount = new realizationaccount
            {
                RealizationAccountID = realizationAccountVM.RealizationAccountID,
                RealizationAccountNo = realizationAccountVM.RealizationAccountNo,
                RealizationAccountName = realizationAccountVM.RealizationAccountName,
                RealizationAccountType = realizationAccountVM.RealizationAccountType,
                UserID = realizationAccountVM.UserID,
                SetDate = realizationAccountVM.SetDate
            };

            unitOfWork.RealizationAccountRepository.Update(realizationAccount);
            unitOfWork.Save();
        }

        public List<RealizationAccountViewModel> GetAllRealizationAccount()
        {
            var result = (from s in unitOfWork.RealizationAccountRepository.Get()
                          orderby s.RealizationAccountID descending
                          select new RealizationAccountViewModel
                          {
                              RealizationAccountID = s.RealizationAccountID,
                              RealizationAccountNo = s.RealizationAccountNo,
                              RealizationAccountName = s.RealizationAccountName,
                              RealizationAccountType = s.RealizationAccountType,
                              UserID = s.UserID,
                              SetDate = s.SetDate
                          }).ToList();

            return result;
        }

        public RealizationAccountViewModel GetRealizationAccountByID(int id)
        {
            var result = (from s in unitOfWork.RealizationAccountRepository.Get()
                          where s.RealizationAccountID == id
                          select new RealizationAccountViewModel
                          {
                              RealizationAccountID = s.RealizationAccountID,
                              RealizationAccountNo = s.RealizationAccountNo,
                              RealizationAccountName = s.RealizationAccountName,
                              RealizationAccountType = s.RealizationAccountType,
                              UserID = s.UserID,
                              SetDate = s.SetDate
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetRealizationAccountTypeDropDown()
        {
            List<DropDownListViewModel> accountTypeList = new List<DropDownListViewModel>();
            accountTypeList.Add(new DropDownListViewModel { Text = "Arunima", Value = 1 });
            accountTypeList.Add(new DropDownListViewModel { Text = "DMC", Value = 2 });
            //accountTypeList.Add(new DropDownListViewModel { Text = "Common", Value = 3 });

            return accountTypeList;
        }

        public List<DropDownListViewModel> GetRealizationAccountNoDropDown(int realizationAccountType)
        {
            var result = (from s in unitOfWork.RealizationAccountRepository.Get()
                          where s.RealizationAccountType == realizationAccountType
                          select new DropDownListViewModel
                          {
                              Text = s.RealizationAccountNo,
                              Value = s.RealizationAccountID
                          }).ToList();

            return result;
        }

        public bool IsUniqueRealizationAccount(string realizationAccountNo, string realizationAccountName, Nullable<int> realizationAccountID = null)
        {
            IQueryable<int> result;

            if (realizationAccountID == null)
            {
                result = from s in unitOfWork.RealizationAccountRepository.Get()
                         where s.RealizationAccountNo.ToLower() == realizationAccountNo.ToLower() 
                         & s.RealizationAccountName.ToLower() == realizationAccountName.ToLower()
                         select s.RealizationAccountID;
            }
            else
            {
                result = from s in unitOfWork.RealizationAccountRepository.Get()
                         where s.RealizationAccountNo.ToLower() == realizationAccountNo.ToLower()
                         & s.RealizationAccountName.ToLower() == realizationAccountName.ToLower()
                         & s.RealizationAccountID != realizationAccountID
                         select s.RealizationAccountID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
