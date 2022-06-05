using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.OrderManagement.BLL
{
    public class StyleLogic
    {
        private UnitOfWork unitOfWork;
        private styleinfo style;

        public StyleLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateStyle(StyleViewModel styleVM)
        {
            style = new styleinfo
            {
                StyleNo = styleVM.StyleNo,
                StyleDescription = styleVM.StyleDescription,
                Capacity = styleVM.Capacity,
                Sam = styleVM.SAM,
                BodyStyle = styleVM.BodyStyle,
                Item = styleVM.Item,
                Febrication = styleVM.Febrication,
                BuyerId = styleVM.BuyerID,
                CustomerId = styleVM.CustomerID,
                DevisionId = styleVM.DivisionID,
                AccountId = styleVM.AccountID,
                //Image=styleVM.Image
            };

            unitOfWork.StyleRepository.Insert(style);
            unitOfWork.Save();
        }

        public void UpdateStyle(StyleViewModel styleVM)
        {
            style = new styleinfo
            {
                StyleId = styleVM.StyleID,
                StyleNo = styleVM.StyleNo,
                StyleDescription = styleVM.StyleDescription,
                Capacity = styleVM.Capacity,
                Sam = styleVM.SAM,
                BodyStyle = styleVM.BodyStyle,
                Item = styleVM.Item,
                Febrication = styleVM.Febrication,
                BuyerId = styleVM.BuyerID,
                CustomerId = styleVM.CustomerID,
                DevisionId = styleVM.DivisionID,
                AccountId = styleVM.AccountID,
                //Image = styleVM.Image
            };

            unitOfWork.StyleRepository.Update(style);
            unitOfWork.Save();
        }

        public List<StyleViewModel> GetAllStyle(int accountID)
        {
            var result = (from s in unitOfWork.StyleRepository.Get()
                          join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                          join c in unitOfWork.CustomerRepository.Get() on s.CustomerId equals c.CustomerId
                          join d in unitOfWork.DivisionRepository.Get() on s.DevisionId equals d.DevisionId
                          where s.AccountId == accountID
                          orderby s.StyleId descending
                          select new StyleViewModel
                          {
                              StyleID = s.StyleId,
                              StyleNo = s.StyleNo,
                              StyleDescription = s.StyleDescription,
                              Capacity = s.Capacity,
                              SAM = s.Sam,
                              BodyStyle = s.BodyStyle,
                              Item = s.Item,
                              Febrication = s.Febrication,

                              BuyerID = s.BuyerId,
                              BuyerName = b.BuyerName,

                              CustomerID = s.CustomerId,
                              CustomerName = c.CustomerName,

                              DivisionID = s.DevisionId,
                              DivisionName = d.DevisionName,

                              AccountID = s.AccountId,

                              //Image = s.Image
                          }).ToList();

            return result;
        }

        public List<StyleViewModel> GetAllStyleByBuyer(int accountID,int buyerID)
        {
            var result = (from s in unitOfWork.StyleRepository.Get()
                          join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                          join c in unitOfWork.CustomerRepository.Get() on s.CustomerId equals c.CustomerId
                          join d in unitOfWork.DivisionRepository.Get() on s.DevisionId equals d.DevisionId
                          where (s.AccountId == accountID) && (s.BuyerId == buyerID)
                          orderby s.StyleId descending
                          select new StyleViewModel
                          {
                              StyleID = s.StyleId,
                              StyleNo = s.StyleNo,
                              StyleDescription = s.StyleDescription,
                              Capacity = s.Capacity,
                              SAM = s.Sam,
                              BodyStyle = s.BodyStyle,
                              Item = s.Item,
                              Febrication = s.Febrication,

                              BuyerID = s.BuyerId,
                              BuyerName = b.BuyerName,

                              CustomerID = s.CustomerId,
                              CustomerName = c.CustomerName,

                              DivisionID = s.DevisionId,
                              DivisionName = d.DevisionName,

                              AccountID = s.AccountId,
                              //Image = s.Image
                          }).ToList();

            return result;
        }

        public StyleViewModel GetStyleByID(int id)
        {
            var result = (from s in unitOfWork.StyleRepository.Get()
                          where s.StyleId == id
                          select new StyleViewModel
                          {
                              StyleID = s.StyleId,
                              StyleNo = s.StyleNo,
                              StyleDescription = s.StyleDescription,
                              Capacity = s.Capacity,
                              SAM = s.Sam,
                              BodyStyle = s.BodyStyle,
                              Item = s.Item,
                              Febrication = s.Febrication,
                              BuyerID = s.BuyerId,
                              CustomerID = s.CustomerId,
                              DivisionID = s.DevisionId,
                              AccountID = s.AccountId,
                              //Image = s.Image
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetStyleDropDown()
        {
            var result = (from s in unitOfWork.StyleRepository.Get()
                          select new DropDownListViewModel
                          {
                              Text = s.StyleNo,
                              Value = s.StyleId
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetStyleDropDown(int accountID)
        {
            var result = (from s in unitOfWork.StyleRepository.Get()
                          where s.AccountId == accountID
                          select new DropDownListViewModel
                          {
                              Text = s.StyleNo,
                              Value = s.StyleId
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetStyleDropDown(int factoryID, DateTime fromDate, DateTime toDate)
        {
            var result = (from s in unitOfWork.StyleRepository.Get()
                          join p in unitOfWork.PurchaseOrderRepository.Get()
                              on s.StyleId equals p.StyleId
                          where p.ExitDate >= fromDate && p.ExitDate <= toDate && p.FactoryId == factoryID
                          select new DropDownListViewModel
                          {
                              Value = s.StyleId,
                              Text = s.StyleNo
                          }).Distinct();

            return result.ToList();
        }

        public List<DropDownListViewModel> GetStyleDropDownByExistingCostsheet()
        {
            var results = (from s in unitOfWork.StyleRepository.Get()
                           join c in unitOfWork.CostsheetRepository.Get()
                           on s.StyleId equals c.StyleId
                           where c.Status == 1
                           select new DropDownListViewModel
                           {
                               Text = s.StyleNo,
                               Value = s.StyleId
                           }).Distinct();

            return results.ToList();
        }

        public bool IsUniqueStyle(string styleNo, Nullable<int> styleID = null)
        {
            IQueryable<int> result;

            if (styleID == null)
            {
                result = from s in unitOfWork.StyleRepository.Get()
                         where s.StyleNo == styleNo
                         select s.StyleId;
            }
            else
            {
                result = from s in unitOfWork.StyleRepository.Get()
                         where s.StyleNo == styleNo & s.StyleId != styleID
                         select s.StyleId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }


        public List<DropDownListViewModel> GetStyleDropDownByBuyerID(int accountID, int buyerID)
        {
            var result = (from s in unitOfWork.StyleRepository.Get()
                          join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                          where /*s.AccountId == accountID &&*/ s.BuyerId == buyerID
                          orderby s.StyleId descending
                          select new DropDownListViewModel
                          {
                              Text = s.StyleNo,
                              Value = s.StyleId
                          }).ToList();
            return result;
        }

        //New Added

        public List<DropDownListViewModel> GetStyleDropDownByBuyerID(string inputString, int buyerID)
        {
            var result = (from s in unitOfWork.StyleRepository.Get()
                          join b in unitOfWork.BuyerRepository.Get() on s.BuyerId equals b.BuyerId
                          where /*s.AccountId == accountID &&*/ s.BuyerId == buyerID &&
                          s.StyleNo.ToLower()
                          .Contains(inputString.ToLower())
                          orderby s.StyleId descending
                          select new DropDownListViewModel
                          {
                              Text = s.StyleNo,
                              Value = s.StyleId
                          }).ToList();
            return result;
        }
    }
}
