using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.MaterialManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.MaterialManagement.BLL
{
    public class ItemLogic
    {
        private UnitOfWork unitOfWork;
        private item item;

        public ItemLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateItem(ItemViewModel itemVM)
        {
            item = new item
            {
                ItemCode = itemVM.ItemCode,
                ItemDescription = itemVM.ItemDescription,
                ItemCategoryId = itemVM.ItemCategoryID
            };

            unitOfWork.ItemRepository.Insert(item);
            unitOfWork.Save();
        }

        public void UpdateItem(ItemViewModel itemVM)
        {
            item = new item
            {
                ItemId = itemVM.ItemID,
                ItemCode = itemVM.ItemCode,
                ItemDescription = itemVM.ItemDescription,
                ItemCategoryId = itemVM.ItemCategoryID
            };

            unitOfWork.ItemRepository.Update(item);
            unitOfWork.Save();
        }

        public List<ItemViewModel> GetAllItem()
        {
            var result = (from s in unitOfWork.ItemRepository.Get()
                          join it in unitOfWork.ItemCategoryRepository.Get()
                          on s.ItemCategoryId equals it.ItemCategoryId
                          orderby it.Name, s.ItemDescription ascending
                          select new ItemViewModel
                          {
                              ItemID = s.ItemId,
                              ItemCode = s.ItemCode,
                              ItemDescription = s.ItemDescription,
                              ItemCategoryID = s.ItemCategoryId,
                              ItemCategoryName = it.Name
                          }).ToList();

            return result;
        }
        public IQueryable<ItemViewModel> GetAllItemForIQuarry()
        {
            var result = (from s in unitOfWork.ItemRepository.Get()
                          join it in unitOfWork.ItemCategoryRepository.Get()
                          on s.ItemCategoryId equals it.ItemCategoryId
                          orderby it.Name, s.ItemDescription ascending
                          select new ItemViewModel
                          {
                              ItemID = s.ItemId,
                              ItemCode = s.ItemCode,
                              ItemDescription = s.ItemDescription,
                              ItemCategoryID = s.ItemCategoryId,
                              ItemCategoryName = it.Name
                          }).AsQueryable();

            return result;
        }

        public int GetTotalItem()
        {
            int count = unitOfWork.ItemRepository.Get().Count();
            return count;
        }

        public ItemViewModel GetItemByID(int id)
        {
            var result = (from s in unitOfWork.ItemRepository.Get()
                          join it in unitOfWork.ItemCategoryRepository.Get()
                          on s.ItemCategoryId equals it.ItemCategoryId
                          where s.ItemId == id
                          select new ItemViewModel
                          {
                              ItemID = s.ItemId,
                              ItemCode = s.ItemCode,
                              ItemDescription = s.ItemDescription,
                              ItemCategoryID = s.ItemCategoryId,
                              ItemCategoryName = it.Name
                          }).SingleOrDefault();

            return result;
        }

        public ItemViewModel GetItemByCode(string itemCode)
        {
            var result = (from s in unitOfWork.ItemRepository.Get()
                          join it in unitOfWork.ItemCategoryRepository.Get()
                          on s.ItemCategoryId equals it.ItemCategoryId
                          where s.ItemCode == itemCode
                          select new ItemViewModel
                          {
                              ItemID = s.ItemId,
                              ItemCode = s.ItemCode,
                              ItemDescription = s.ItemDescription,
                              ItemCategoryID = s.ItemCategoryId,
                              ItemCategoryName = it.Name
                          }).FirstOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetItemDropDown()
        {
            var result = (from s in unitOfWork.ItemRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.ItemId,
                              Text = s.ItemDescription
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetItemDropDown(int itemCategoryID)
        {
            var result = (from s in unitOfWork.ItemRepository.Get()
                          where s.ItemCategoryId == itemCategoryID
                          select new DropDownListViewModel
                          {
                              Value = s.ItemId,
                              Text = s.ItemDescription
                          }).ToList();

            return result;
        }

        public List<DropDownListViewModel> GetItemDropDownByPI(int piID) {
            var result = (from i in unitOfWork.ItemRepository.Get()
                          join b in unitOfWork.BookingRepository.Get()
                              on i.ItemId equals b.ItemID
                          where b.PIId == piID
                          select new DropDownListViewModel
                          {
                              Value = i.ItemId,
                              Text = i.ItemDescription
                          }).Distinct().ToList();

            return result;
        }

        public List<DropDownListViewModel> GetItemDropDownByPurchaseOrder(int purchaseOrderID)
        {
            var result = (from i in unitOfWork.ItemRepository.Get()
                          join b in unitOfWork.BookingRepository.Get()
                              on i.ItemId equals b.ItemID
                          where b.PurchaseOrderID == purchaseOrderID
                          select new DropDownListViewModel
                          {
                              Value = i.ItemId,
                              Text = i.ItemDescription
                          }).Distinct().ToList();

            return result;
        }

        public bool IsUniqueItem(string itemDescription, Nullable<int> itemID = null)
        {
            IQueryable<int> result;

            if (itemID == null)
            {
                result = from s in unitOfWork.ItemRepository.Get()
                         where s.ItemDescription == itemDescription
                         select s.ItemId;
            }
            else
            {
                result = from s in unitOfWork.ItemRepository.Get()
                         where s.ItemDescription == itemDescription & s.ItemId != itemID
                         select s.ItemId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public object GetItemByAllPO(List<DropDownListViewModel> poList)
        {
            throw new NotImplementedException();
        }
    }
}
