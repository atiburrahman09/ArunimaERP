using ScopoERP.Accounts.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Accounts.BLL
{
    public class ChartOfAccountLogic
    {
        private UnitOfWork unitOfWork;

        public ChartOfAccountLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ChartOfAccountViewModel> GetAllAccounts()
        {
            var data = (from s in unitOfWork.ChartOfAccountRepository.Get()
                        select new ChartOfAccountViewModel
                        {
                            ChartOfAccountID = s.ChartOfAccountID,
                            AccountNo = s.AccountNo,
                            AccountName = s.AccountName
                        }).AsEnumerable();

            return data;
        }

        public ChartOfAccountViewModel GetAccountByID(int id)
        {
            var data = (from s in unitOfWork.ChartOfAccountRepository.Get()
                        where s.ChartOfAccountID == id
                        select new ChartOfAccountViewModel
                        {
                            ChartOfAccountID = s.ChartOfAccountID,
                            AccountNo = s.AccountNo,
                            AccountName = s.AccountName
                        }).SingleOrDefault();

            return data;
        }

        public IEnumerable<DropDownListViewModel> GetAccountDropDown()
        {
            var data = (from s in unitOfWork.ChartOfAccountRepository.Get()
                        select new DropDownListViewModel
                        {
                            Value = s.ChartOfAccountID,
                            Text = "(" + s.AccountNo + ") " + s.AccountName
                        }).AsEnumerable();

            return data;
        }

        public void Create(ChartOfAccountViewModel chartOfAccountVM)
        {
            ChartOfAccount chartOfAccount = new ChartOfAccount
            {
                ChartOfAccountID = chartOfAccountVM.ChartOfAccountID,
                AccountNo = chartOfAccountVM.AccountNo,
                AccountName = chartOfAccountVM.AccountName
            };

            unitOfWork.ChartOfAccountRepository.Insert(chartOfAccount);
            unitOfWork.Save();
        }

        public void Update(ChartOfAccountViewModel chartOfAccountVM)
        {
            ChartOfAccount chartOfAccount = new ChartOfAccount
            {
                ChartOfAccountID = chartOfAccountVM.ChartOfAccountID,
                AccountNo = chartOfAccountVM.AccountNo,
                AccountName = chartOfAccountVM.AccountName
            };

            unitOfWork.ChartOfAccountRepository.Update(chartOfAccount);
            unitOfWork.Save();
        }
    }
}
