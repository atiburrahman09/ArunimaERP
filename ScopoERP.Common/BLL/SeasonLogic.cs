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
    public class SeasonLogic
    {
        private UnitOfWork unitOfWork;
        private season season;

        public SeasonLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="divisionVM"></param>
        public void CreateSeason(SeasonViewModel seasonVM)
        {
            season = new season
            {
                SeasonName = seasonVM.SeasonName
            };

            unitOfWork.SeasonRepository.Insert(season);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sesaonVM"></param>
        public void UpdateSeason(SeasonViewModel sesaonVM)
        {
            season = new season
            {
                SeasonId = sesaonVM.SeasonId,
                SeasonName = sesaonVM.SeasonName
            };

            unitOfWork.SeasonRepository.Update(season);
            unitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SeasonViewModel> GetAllSeason()
        {
            var result = (from s in unitOfWork.SeasonRepository.Get()
                          orderby s.SeasonId descending
                          select new SeasonViewModel
                          {
                              SeasonId = s.SeasonId,
                              SeasonName = s.SeasonName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SeasonViewModel GetSeasonById(int id)
        {
            var result = (from s in unitOfWork.SeasonRepository.Get()
                          where s.SeasonId == id
                          select new SeasonViewModel
                          {
                              SeasonId = s.SeasonId,
                              SeasonName = s.SeasonName
                          }).SingleOrDefault();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DropDownListViewModel> GetSeasonDropDown()
        {
            var result = (from s in unitOfWork.SeasonRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.SeasonId,
                              Text = s.SeasonName
                          }).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seasonName"></param>
        /// <param name="seasonId"></param>
        /// <returns></returns>
        public bool IsUniqueSeason(String seasonName, Nullable<int> seasonId = null)
        {
            IQueryable<int> result;

            if (seasonId == null)
            {
                result = from s in unitOfWork.SeasonRepository.Get()
                         where s.SeasonName == seasonName
                         select s.SeasonId;

            }
            else
            {
                result = from s in unitOfWork.SeasonRepository.Get()
                         where s.SeasonName == seasonName & s.SeasonId != seasonId
                         select s.SeasonId;

            }

            if (result.Count() > 0)
            {
                return false;
            }

            return true;
        }
    }
}
