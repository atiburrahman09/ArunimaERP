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
    public class FactoryLogic
    {
        private UnitOfWork unitOfWork;
        private factory factory;

        public FactoryLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateFactory(FactoryViewModel factoryVM)
        {
            factory = new factory
            {
                FactoryName = factoryVM.FactoryName
            };

            unitOfWork.FactoryRepository.Insert(factory);
            unitOfWork.Save();
        }

        public void UpdateFactory(FactoryViewModel factoryVM)
        {
            factory = new factory
            {
                FactoryId = factoryVM.FactoryID,
                FactoryName = factoryVM.FactoryName
            };

            unitOfWork.FactoryRepository.Update(factory);
            unitOfWork.Save();
        }

        public List<FactoryViewModel> GetAllFactory()
        {
            var result = (from s in unitOfWork.FactoryRepository.Get()
                          orderby s.FactoryId descending
                          select new FactoryViewModel
                          {
                              FactoryID = s.FactoryId,
                              FactoryName = s.FactoryName
                          }).ToList();

            return result;
        }

        public FactoryViewModel GetFactoryByID(int id)
        {
            var result = (from s in unitOfWork.FactoryRepository.Get()
                          where s.FactoryId == id
                          select new FactoryViewModel
                          {
                              FactoryID = s.FactoryId,
                              FactoryName = s.FactoryName
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetFactoryDropDown()
        {
            var result = (from s in unitOfWork.FactoryRepository.Get()
                          select new DropDownListViewModel
                          {
                              Value = s.FactoryId,
                              Text = s.FactoryName
                          }).ToList();

            return result;
        }

        public bool IsUniqueFactory(string factoryName, int? factoryID = null)
        {
            IQueryable<int> result;

            if (factoryID == null)
            {
                result = from s in unitOfWork.BuyerRepository.Get()
                         where s.BuyerName == factoryName
                         select s.BuyerId;
            }
            else
            {
                result = from s in unitOfWork.BuyerRepository.Get()
                         where s.BuyerName == factoryName & s.BuyerId != factoryID
                         select s.BuyerId;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
