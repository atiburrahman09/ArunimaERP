using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Stackholder.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Stackholder.BLL
{
    public class BuyerLogic
    {
        private UnitOfWork unitOfWork;
        private buyerinfo buyer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public BuyerLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerVM"></param>
        public void CreateBuyer(BuyerViewModel buyerVM)
        {
            if(buyerVM.BuyerID != 0)
            {
                UpdateBuyer(buyerVM);
            }
            else
            {
                buyer = new buyerinfo
                {
                    BuyerName = buyerVM.BuyerName
                };

                unitOfWork.BuyerRepository.Insert(buyer);
                unitOfWork.Save();
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerVM"></param>
        public void UpdateBuyer(BuyerViewModel buyerVM)
        {
            buyer = new buyerinfo
            {
                BuyerId = buyerVM.BuyerID,
                BuyerName = buyerVM.BuyerName
            };

            unitOfWork.BuyerRepository.Update(buyer);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BuyerViewModel> GetAllBuyer()
        {
            var result = (from s in unitOfWork.BuyerRepository.Get()
                          orderby s.BuyerId descending
                          select new BuyerViewModel
                          {
                              BuyerID = s.BuyerId,
                              BuyerName = s.BuyerName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BuyerViewModel GetBuyerByID(int id)
        {
            var result = (from s in unitOfWork.BuyerRepository.Get()
                          where s.BuyerId == id
                          select new BuyerViewModel
                          {
                              BuyerID = s.BuyerId,
                              BuyerName = s.BuyerName
                          }).SingleOrDefault();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DropDownListViewModel> GetBuyerDropDown()
        {
            var result = (from s in unitOfWork.BuyerRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.BuyerId,
                              Text = s.BuyerName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerName"></param>
        /// <param name="buyerID"></param>
        /// <returns></returns>
        public bool IsUniqueBuyer(string buyerName,int buyerID)
        {
            IQueryable<int> result;

            if (buyerID == 0)
            {
                result = from s in unitOfWork.BuyerRepository.Get()
                         where s.BuyerName == buyerName
                         select s.BuyerId;
            }
            else
            {
                result = from s in unitOfWork.BuyerRepository.Get()
                         where s.BuyerName == buyerName & s.BuyerId != buyerID
                         select s.BuyerId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        // New Method


        public List<BuyerViewModel> GetAllBuyerDropDown(string inputString)
        {
            var result = (from b in unitOfWork.BuyerRepository.Get()
                          where b.BuyerName.ToLower()
                         .Contains(inputString.ToLower())
                          orderby b.BuyerId descending
                          select new BuyerViewModel
                          {
                              BuyerID = b.BuyerId,
                              BuyerName = b.BuyerName
                          }).ToList();

            return result;
        }
    }
}
