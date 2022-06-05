using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.Common.BLL
{
    public class DivisionLogic
    {
        private UnitOfWork unitOfWork;
        private devision division;

        public DivisionLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="divisionVM"></param>
        public void CreateDivision(DivisionViewModel divisionVM)
        {
            division = new devision
            {
                DevisionName = divisionVM.DivisionName
            };

            unitOfWork.DivisionRepository.Insert(division);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="divisionVM"></param>
        public void UpdateDivision(DivisionViewModel divisionVM)
        {
            division = new devision
            {
                DevisionId = divisionVM.DivisionID,
                DevisionName = divisionVM.DivisionName
            };

            unitOfWork.DivisionRepository.Update(division);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DivisionViewModel> GetAllDivision()
        {
            var result = (from s in unitOfWork.DivisionRepository.Get()
                          orderby s.DevisionId descending
                          select new DivisionViewModel
                          {
                              DivisionID = s.DevisionId,
                              DivisionName = s.DevisionName
                          }).ToList();

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DivisionViewModel GetDivisionById(int id)
        {
            var result = (from s in unitOfWork.DivisionRepository.Get()
                          where s.DevisionId == id
                          select new DivisionViewModel
                          {
                              DivisionID = s.DevisionId,
                              DivisionName = s.DevisionName
                          }).SingleOrDefault();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DropDownListViewModel> GetDivisionDropDown()
        {
            var result = (from s in unitOfWork.DivisionRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.DevisionId,
                              Text = s.DevisionName
                          }).ToList();


            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="divisionName"></param>
        /// <param name="divisionId"></param>
        /// <returns></returns>
        public bool IsUniqueDivision(String divisionName, Nullable<int> divisionId = null)
        {
            IQueryable<int> result;

            if (divisionId == null)
            {
                result = from s in unitOfWork.DivisionRepository.Get()
                         where s.DevisionName == divisionName
                         select s.DevisionId;

            }
            else
            {
                result = from s in unitOfWork.DivisionRepository.Get()
                         where s.DevisionName == divisionName & s.DevisionId != divisionId
                         select s.DevisionId;

            }

            if (result.Count() > 0)
            {
                return false;
            }

            return true;
        }



    }
}
