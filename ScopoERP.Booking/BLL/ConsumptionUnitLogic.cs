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
    public class ConsumptionUnitLogic
    {
        private UnitOfWork unitOfWork;
        private consumptionunit consumptionUnit;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ConsumptionUnitLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumptionUnitVM"></param>
        public void CreateConsumptionUnit(ConsumptionUnitViewModel consumptionUnitVM)
        {
            consumptionUnit = new consumptionunit
            {
                UnitName = consumptionUnitVM.ConsumptionUnitName
            };

            unitOfWork.ConsumptionUnitRepository.Insert(consumptionUnit);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumptionUnitVM"></param>
        public void UpdateConsumptionUnit(ConsumptionUnitViewModel consumptionUnitVM)
        {
            consumptionUnit = new consumptionunit
            {
                ConsumptionUnitId = consumptionUnitVM.ConsumptionUnitID,
                UnitName = consumptionUnitVM.ConsumptionUnitName
            };

            unitOfWork.ConsumptionUnitRepository.Update(consumptionUnit);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ConsumptionUnitViewModel> GetAllConsumptionUnit()
        {
            var result = (from s in unitOfWork.ConsumptionUnitRepository.Get()
                          orderby s.ConsumptionUnitId descending
                          select new ConsumptionUnitViewModel
                          {
                              ConsumptionUnitID = s.ConsumptionUnitId,
                              ConsumptionUnitName = s.UnitName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConsumptionUnitViewModel GetConsumptionUnitByID(int id)
        {
            var result = (from s in unitOfWork.ConsumptionUnitRepository.Get()
                          where s.ConsumptionUnitId == id
                          select new ConsumptionUnitViewModel
                          {
                              ConsumptionUnitID = s.ConsumptionUnitId,
                              ConsumptionUnitName = s.UnitName
                          }).SingleOrDefault();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DropDownListViewModel> GetConsumptionUnitDropDown()
        {
            var result = (from s in unitOfWork.ConsumptionUnitRepository.Get()
                          select new DropDownListViewModel
                          {
                              ValueInt = s.ConsumptionUnitId,
                              Value = s.ConsumptionUnitId,
                              Text = s.UnitName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumptionUnitName"></param>
        /// <param name="consumptionUnitID"></param>
        /// <returns></returns>
        public bool IsUniqueConsumptionUnit(string consumptionUnitName, Nullable<int> consumptionUnitID = null)
        {
            IQueryable<int> result;

            if (consumptionUnitID == null)
            {
                result = from s in unitOfWork.ConsumptionUnitRepository.Get()
                         where s.UnitName == consumptionUnitName
                         select s.ConsumptionUnitId;
            }
            else
            {
                result = from s in unitOfWork.ConsumptionUnitRepository.Get()
                         where s.UnitName == consumptionUnitName & s.ConsumptionUnitId != consumptionUnitID
                         select s.ConsumptionUnitId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
