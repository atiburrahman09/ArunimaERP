using ScopoERP.Accounts.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Accounts.BLL
{
    public class BudgetLogic
    {
        private UnitOfWork unitOfWork;
        private Budget budget;
        
        public BudgetLogic(UnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }
        
        public void Create(BudgetViewModel budgetVM)
        {
            budget = new Budget
            {
                BudgetID = budgetVM.BudgetID,
                ChartOfAccountID = budgetVM.ChartOfAccountID,
                Month = budgetVM.Month,
                BudgetAmount = budgetVM.BudgetAmount
            };

            unitOfWork.BudgetRepository.Insert(budget);
            unitOfWork.Save();
        }
        
        public void Update(BudgetViewModel budgetVM)
        {
            budget = new Budget
            {
                BudgetID = budgetVM.BudgetID,
                ChartOfAccountID = budgetVM.ChartOfAccountID,
                Month = budgetVM.Month,
                BudgetAmount = budgetVM.BudgetAmount
            };

            unitOfWork.BudgetRepository.Update(budget);
            unitOfWork.Save();
        }
        
        public List<BudgetViewModel> GetAllBudget()
        {
            var result = (from s in unitOfWork.BudgetRepository.Get()
                          join a in unitOfWork.ChartOfAccountRepository.Get() on s.ChartOfAccountID equals a.ChartOfAccountID
                          select new BudgetViewModel
                          {
                              BudgetID = s.BudgetID,
                              ChartOfAccountID = s.ChartOfAccountID,
                              AccountNo = "("+a.AccountNo + ") " + a.AccountName,
                              Month = s.Month,
                              BudgetAmount = s.BudgetAmount
                          }).ToList();

            return result;
        }
        
        public BudgetViewModel GetBudgetByID(int id)
        {
            var result = (from s in unitOfWork.BudgetRepository.Get()
                          where s.BudgetID == id
                          select new BudgetViewModel
                          {
                              BudgetID = s.BudgetID,
                              ChartOfAccountID = s.ChartOfAccountID,
                              Month = s.Month,
                              BudgetAmount = s.BudgetAmount
                          }).SingleOrDefault();

            return result;
        }
                
        public bool IsUniqueBudget(int month, int chartOfAccountID, Nullable<int> BudgetID = null)
        {
            IQueryable<int> result;

            if (BudgetID == null)
            {
                result = from s in unitOfWork.BudgetRepository.Get()
                         where s.Month == month && s.ChartOfAccountID == chartOfAccountID
                         select s.BudgetID;
            }
            else
            {
                result = from s in unitOfWork.BudgetRepository.Get()
                         where s.Month == month && s.ChartOfAccountID == chartOfAccountID & s.BudgetID != BudgetID
                         select s.BudgetID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
