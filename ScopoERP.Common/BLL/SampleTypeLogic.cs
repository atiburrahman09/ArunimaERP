using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;

namespace ScopoERP.Common.BLL
{
    public class SampleTypeLogic
    {
        private UnitOfWork unitOfWork;
        private sampletype sampleType;
        public SampleTypeLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool IsUniqueSampleType(string sampleTypeName, Nullable<int> sampleTypeID = null)
        {
            IQueryable<int> result;

            if (sampleTypeID == null)
            {
                result = from s in unitOfWork.SampleTypeRepository.Get()
                         where s.SampleTypeName == sampleTypeName
                         select s.SampleTypeID;
            }
            else
            {
                result = from s in unitOfWork.SampleTypeRepository.Get()
                         where s.SampleTypeName == sampleTypeName & s.SampleTypeID != sampleTypeID
                         select s.SampleTypeID;
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public void CreateSampleType(SampleTypeViewModel sampleTypeVM, string name)
        {
            if (sampleTypeVM.SampleTypeID != 0)
            {
                sampleType = new sampletype
                {
                    SampleTypeName = sampleTypeVM.SampleTypeName,
                    SampleTypeID = sampleTypeVM.SampleTypeID,
                    SetDate = DateTime.Now,
                    UserID = name
                };
                unitOfWork.SampleTypeRepository.Update(sampleType);

            }
            else
            {
                sampleType = new sampletype
                {
                    SampleTypeName = sampleTypeVM.SampleTypeName,
                    SetDate = DateTime.Now,
                    UserID = name
                };
                unitOfWork.SampleTypeRepository.Insert(sampleType);
            }

            unitOfWork.Save();
        }

        public SampleTypeViewModel GetSampleTypeById(int id)
        {
            SampleTypeViewModel sample = (from s in unitOfWork.SampleTypeRepository.Get()
                                          where s.SampleTypeID == id
                                          select new SampleTypeViewModel
                                          {
                                              SampleTypeID=s.SampleTypeID,
                                              SampleTypeName=s.SampleTypeName
                                          }).SingleOrDefault();

            return sample;
        }

        public List<SampleTypeViewModel> GetAllSampleType()
        {
            List<SampleTypeViewModel> list = (from s in unitOfWork.SampleTypeRepository.Get()
                                              select new SampleTypeViewModel
                                              {
                                                  SampleTypeID = s.SampleTypeID,
                                                  SampleTypeName = s.SampleTypeName,
                                                  SetDate = s.SetDate,
                                                  UserID = s.UserID
                                              }).ToList();

            return list;
        }
    }
}
