using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Production.ViewModel;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Production.BLL
{
    public class ProductionFloorLogic
    {
        private UnitOfWork unitOfWork;
        private floorline producttionFloor;

        public ProductionFloorLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateProductionFloor(ProductionFloorViewModel productionFloorVM)
        {
            producttionFloor = new floorline
            {
                Floor = productionFloorVM.Floor,
                Line = productionFloorVM.Line,
                Devision = productionFloorVM.Division,
                Status = productionFloorVM.Status
            };

            unitOfWork.ProductionFloorRepository.Insert(producttionFloor);
            unitOfWork.Save();
        }

        public void UpdateProductionFloor(ProductionFloorViewModel productionFloorVM)
        {
            producttionFloor = new floorline
            {
                FloorLineId = productionFloorVM.ProductionFloorID,
                Floor = productionFloorVM.Floor,
                Line = productionFloorVM.Line,
                Devision = productionFloorVM.Division,
                Status = productionFloorVM.Status
            };

            unitOfWork.ProductionFloorRepository.Update(producttionFloor);
            unitOfWork.Save();
        }

        public List<ProductionFloorViewModel> GetAllProductionFloor()
        {
            var result = (from s in unitOfWork.ProductionFloorRepository.Get()
                          where s.Status == 1
                          select new ProductionFloorViewModel
                          {
                              ProductionFloorID = s.FloorLineId,
                              Floor = s.Floor,
                              Line = s.Line,
                              Division = s.Devision,
                              Status = s.Status
                          }).ToList();

            return result;
        }

        public ProductionFloorViewModel GetProductionFloorByID(int id)
        {
            var result = (from s in unitOfWork.ProductionFloorRepository.Get()
                          where s.FloorLineId == id & s.Status == 1
                          select new ProductionFloorViewModel
                          {
                              ProductionFloorID = s.FloorLineId,
                              Floor = s.Floor,
                              Line = s.Line,
                              Division = s.Devision,
                              Status = s.Status
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetFloorDropDown()
        {
            var result = (from c in unitOfWork.ProductionFloorRepository.Get()
                          where c.Status == 1
                          select new DropDownListViewModel
                          {
                              Text = c.Floor,
                              ValueString = c.Floor
                          }).Distinct().ToList();

            return result; 
        }
        


        public List<DropDownListViewModel> GetLineDropDownByFloor(string floor)
        {
            var result = (from c in unitOfWork.ProductionFloorRepository.Get()
                          where c.Status == 1 && c.Floor == floor
                          select new DropDownListViewModel
                          {
                              ValueString = c.Line,
                              Text = c.Line
                          }).ToList();

            return result;
        }

        public List<LineViewModel> GetAllLineByFloor(string floor)
        {
            var result = (from c in unitOfWork.ProductionFloorRepository.Get()
                          where c.Status == 1 && c.Floor == floor
                          select new LineViewModel
                          {
                              id = c.FloorLineId,
                              name = c.Line
                          }).ToList();

            return result;
        }

        public bool IsUniqueProductionFloor(string floor, string line, Nullable<int> productionFloorID = null)
        {
            IQueryable<int> result;

            if (productionFloorID == null)
            {
                result = from s in unitOfWork.ProductionFloorRepository.Get()
                         where s.Floor == floor && s.Line == line
                         select s.FloorLineId;
            }
            else
            {
                result = from s in unitOfWork.ProductionFloorRepository.Get()
                         where s.Floor == floor && s.Line == line && s.FloorLineId != productionFloorID
                         select s.FloorLineId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }



        public List<DropDownListViewModel> GetLineDropDown()
        {
            var result = (from c in unitOfWork.ProductionFloorRepository.Get()                          
                          select new DropDownListViewModel
                          {
                              ValueString = c.Line,
                              Text = c.Line
                          }).ToList();

            return result;
        }
    }
}
