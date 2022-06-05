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
    public class ExpenseLogic
    {
        private UnitOfWork unitOfWork;
        private Expense expense;

        public ExpenseLogic(UnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }

        public void Create(ExpenseViewModel expenseVM)
        {
            var lastRef = unitOfWork.ExpenseRepository.Get()
                .OrderByDescending(x => x.ExpenseID)
                .Select(x => x.ReferenceNo).FirstOrDefault();
            string newRef = "EV-"+DateTime.Now.Year + "-";
            if (lastRef == null)
            {
                newRef += "00001";
            }
            else
            {
                int num = int.Parse(lastRef.Substring(8, 5));
                num += 1;
                newRef+=num.ToString().PadLeft(5, '0');
            }

            expense = new Expense
            {
                ExpenseID = expenseVM.ExpenseID,
                ChartOfAccountID = expenseVM.ChartOfAccountID,
                ExpenseAmount = expenseVM.ExpenseAmount,
                ExpenseDate = expenseVM.ExpenseDate,
                ExpenseBy = expenseVM.ExpenseBy,
                Description = expenseVM.Description,
                IsApproved = false,
                ReferenceNo = newRef
            };

            unitOfWork.ExpenseRepository.Insert(expense);
            unitOfWork.Save();
        }

        public void Update(ExpenseViewModel expenseVM)
        {
            expense = unitOfWork.ExpenseRepository.GetById(expenseVM.ExpenseID);

            expense.ExpenseID = expenseVM.ExpenseID;
            expense.ChartOfAccountID = expenseVM.ChartOfAccountID;
            expense.ExpenseAmount = expenseVM.ExpenseAmount;
            expense.ExpenseDate = expenseVM.ExpenseDate;
            expense.ExpenseBy = expenseVM.ExpenseBy;
            expense.Description = expenseVM.Description;
            expense.ReferenceNo = expenseVM.ReferenceNo;
            expense.IsApproved = expense.IsApproved;

            unitOfWork.ExpenseRepository.Update(expense);
            unitOfWork.Save();
        }

        public List<ExpenseViewModel> GetAllExpense()
        {
            var result = (from s in unitOfWork.ExpenseRepository.Get()
                          join a in unitOfWork.ChartOfAccountRepository.Get() on s.ChartOfAccountID equals a.ChartOfAccountID
                          select new ExpenseViewModel
                          {
                              ExpenseID = s.ExpenseID,
                              ChartOfAccountID = s.ChartOfAccountID,
                              AccountNo = "(" + a.AccountNo + ") " + a.AccountName,
                              ExpenseAmount = s.ExpenseAmount,
                              ExpenseDate = s.ExpenseDate,
                              ExpenseBy = s.ExpenseBy,
                              Description = s.Description,
                              ReferenceNo = s.ReferenceNo
                          }).OrderByDescending(x => x.ExpenseDate).ToList();

            return result;
        }

        public ExpenseViewModel GetExpenseByID(int id)
        {
            var result = (from s in unitOfWork.ExpenseRepository.Get()
                          where s.ExpenseID == id
                          select new ExpenseViewModel
                          {
                              ExpenseID = s.ExpenseID,
                              ChartOfAccountID = s.ChartOfAccountID,
                              ExpenseAmount = s.ExpenseAmount,
                              ExpenseDate = s.ExpenseDate,
                              ExpenseBy = s.ExpenseBy,
                              Description = s.Description,
                              ReferenceNo=s.ReferenceNo
                          }).SingleOrDefault();

            return result;
        }
    }
}
