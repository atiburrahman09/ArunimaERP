using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Production.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Production.BLL
{
    public class MachineCategoryLogic
    {
        private UnitOfWork unitOfWork;
        private machinecategory machineCategory;

        public MachineCategoryLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateMachineCategory(MachineCategoryViewModel machineCategoryVM)
        {
            machineCategory = new machinecategory
            {
                Name = machineCategoryVM.Name,
                ParentCategoryID = machineCategoryVM.ParentCategoryID,
                SetupDate = DateTime.Now
            };

            unitOfWork.MachineCategoryRepository.Insert(machineCategory);
            unitOfWork.Save();
        }

        public void UpdateMachineCategory(MachineCategoryViewModel machineCategoryVM)
        {
            machineCategory = new machinecategory
            {
                MachineCategoryID = machineCategoryVM.MachineCategoryID,
                Name = machineCategoryVM.Name,
                ParentCategoryID = machineCategoryVM.ParentCategoryID,
                SetupDate = DateTime.Now
            };

            unitOfWork.MachineCategoryRepository.Update(machineCategory);
            unitOfWork.Save();
        }

        public List<MachineCategoryViewModel> GetAllMachineCategory()
        {
            var result = (from s in unitOfWork.MachineCategoryRepository.Get()
                          orderby s.MachineCategoryID descending
                          select new MachineCategoryViewModel
                          {
                              MachineCategoryID = s.MachineCategoryID,
                              Name = s.Name,
                              ParentCategoryID = s.ParentCategoryID
                          }).ToList();

            return result;
        }

        public int GetTotalMachineCategory()
        {
            int count = unitOfWork.MachineCategoryRepository.Get().Count();

            return count;
        }

        public MachineCategoryViewModel GetMachineCategoryByID(int id)
        {
            var result = (from s in unitOfWork.MachineCategoryRepository.Get()
                          where s.MachineCategoryID == id
                          select new MachineCategoryViewModel
                          {
                              MachineCategoryID = s.MachineCategoryID,
                              Name = s.Name,
                              ParentCategoryID = s.ParentCategoryID
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetMachineCategoryDropDown()
        {
            var result = (from s in unitOfWork.MachineCategoryRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.MachineCategoryID,
                              Text = s.Name
                          }).ToList();

            return result;
        }

        public bool IsUniqueMachineCategory(string machineCategoryName, Nullable<int> machineCategoryID = null)
        {
            IQueryable<int> result;

            if (machineCategoryID == null)
            {
                result = from s in unitOfWork.MachineCategoryRepository.Get()
                         where s.Name == machineCategoryName
                         select s.MachineCategoryID;
            }
            else
            {
                result = from s in unitOfWork.MachineCategoryRepository.Get()
                         where s.Name == machineCategoryName & s.MachineCategoryID != machineCategoryID
                         select s.MachineCategoryID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
