using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.LC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.LC.BLL
{
    public class BankLogic
    {
        private UnitOfWork unitOfWork;
        private bank bank;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public BankLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankVM"></param>
        public void CreateBank(BankViewModel bankVM)
        {
            bank = new bank
            {
                BankName = bankVM.BankName,
                BankAddress = bankVM.BankAddress,
                UserID = bankVM.UserID,
                SetDate = DateTime.Now
            };

            unitOfWork.BankRepository.Insert(bank);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankVM"></param>
        public void UpdateBank(BankViewModel bankVM)
        {
            bank = new bank
            {
                BankID = bankVM.BankID,
                BankName = bankVM.BankName,
                BankAddress = bankVM.BankAddress,
                UserID = bankVM.UserID,
                SetDate = DateTime.Now
            };

            unitOfWork.BankRepository.Update(bank);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BankViewModel> GetAllBank()
        {
            var result = (from s in unitOfWork.BankRepository.Get()
                          orderby s.BankID descending
                          select new BankViewModel
                          {
                              BankID = s.BankID,
                              BankName = s.BankName,
                              BankAddress = s.BankAddress,
                              UserID = s.UserID,
                              SetDate = s.SetDate
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BankViewModel GetBankByID(int id)
        {
            var result = (from s in unitOfWork.BankRepository.Get()
                          where s.BankID == id
                          select new BankViewModel
                          {
                              BankID = s.BankID,
                              BankName = s.BankName,
                              BankAddress = s.BankAddress,
                              UserID = s.UserID,
                              SetDate = s.SetDate
                          }).SingleOrDefault();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DropDownListViewModel> GetBankDropDown()
        {
            var result = (from s in unitOfWork.BankRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.BankID,
                              Text = s.BankName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankName"></param>
        /// <param name="bankID"></param>
        /// <returns></returns>
        public bool IsUniqueBank(string bankName, Nullable<int> bankID = null)
        {
            IQueryable<int> result;

            if (bankID == null)
            {
                result = from s in unitOfWork.BankRepository.Get()
                         where s.BankName == bankName
                         select s.BankID;
            }
            else
            {
                result = from s in unitOfWork.BankRepository.Get()
                         where s.BankName == bankName & s.BankID != bankID
                         select s.BankID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
