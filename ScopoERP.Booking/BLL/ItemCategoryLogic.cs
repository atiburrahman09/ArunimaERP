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
    public class ItemCategoryLogic
    {
        private UnitOfWork unitOfWork;
        private itemcategory itemCategory;

        public ItemCategoryLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateItemCategory(ItemCategoryViewModel itemCategoryVM)
        {
            itemCategory = new itemcategory
            {
                Name = itemCategoryVM.Name,
                ParentCategoryId = itemCategoryVM.ParentCategoryID
            };

            unitOfWork.ItemCategoryRepository.Insert(itemCategory);
            unitOfWork.Save();
        }

        public void UpdateItemCategory(ItemCategoryViewModel itemCategoryVM)
        {
            itemCategory = new itemcategory
            {
                ItemCategoryId = itemCategoryVM.ItemCategoryID,
                Name = itemCategoryVM.Name,
                ParentCategoryId = itemCategoryVM.ParentCategoryID
            };

            unitOfWork.ItemCategoryRepository.Update(itemCategory);
            unitOfWork.Save();
        }

        public List<ItemCategoryViewModel> GetAllItemCategory()
        {
            var result = (from s in unitOfWork.ItemCategoryRepository.Get()
                          orderby s.Name ascending
                          select new ItemCategoryViewModel
                          {
                              ItemCategoryID = s.ItemCategoryId,
                              Name = s.Name,
                              ParentCategoryID = s.ParentCategoryId
                          }).ToList();

            return result;
        }

        public int GetTotalItemCategory()
        {
            int count = unitOfWork.ItemCategoryRepository.Get().Count();
            return count;
        }

        public ItemCategoryViewModel GetItemCategoryByID(int id)
        {
            var result = (from s in unitOfWork.ItemCategoryRepository.Get()
                          where s.ItemCategoryId == id
                          select new ItemCategoryViewModel
                          {
                              ItemCategoryID = s.ItemCategoryId,
                              Name = s.Name,
                              ParentCategoryID = s.ParentCategoryId
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetItemCategoryDropDown()
        {
            var result = (from s in unitOfWork.ItemCategoryRepository.Get()
                          select new DropDownListViewModel
                          {
                              ValueInt = s.ItemCategoryId,
                              Value = s.ItemCategoryId,
                              Text = s.Name
                          }).ToList();

            return result;
        }

        public bool IsUniqueItemCategory(string itemCategoryName, Nullable<int> itemCategoryID = null)
        {
            IQueryable<int> result;

            if (itemCategoryID == null)
            {
                result = from s in unitOfWork.ItemCategoryRepository.Get()
                         where s.Name == itemCategoryName
                         select s.ItemCategoryId;
            }
            else
            {
                result = from s in unitOfWork.ItemCategoryRepository.Get()
                         where s.Name == itemCategoryName & s.ItemCategoryId != itemCategoryID
                         select s.ItemCategoryId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
