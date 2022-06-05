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
    public class SampleTypeLogic
    {
        private UnitOfWork unitOfWork;
        private sampletype sampleType;

        public SampleTypeLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateSampleType(Common.ViewModel.SampleTypeViewModel sampleTypeVM)
        {
            sampleType = new sampletype
            {
                SampleTypeName = sampleTypeVM.SampleTypeName,
                UserID = sampleTypeVM.UserID,
                SetDate = sampleTypeVM.SetDate
            };

            unitOfWork.SampleTypeRepository.Insert(sampleType);
            unitOfWork.Save();
        }

        public void UpdateSampleType(Common.ViewModel.SampleTypeViewModel sampleTypeVM)
        {
            sampleType = new sampletype
            {
                SampleTypeID = sampleTypeVM.SampleTypeID,
                SampleTypeName = sampleTypeVM.SampleTypeName,
                UserID = sampleTypeVM.UserID,
                SetDate = sampleTypeVM.SetDate
            };

            unitOfWork.SampleTypeRepository.Update(sampleType);
            unitOfWork.Save();
        }

        public List<Common.ViewModel.SampleTypeViewModel> GetAllSampleType()
        {
            var result = (from s in unitOfWork.SampleTypeRepository.Get()
                          orderby s.SampleTypeID descending
                          select new Common.ViewModel.SampleTypeViewModel
                          {
                              SampleTypeID = s.SampleTypeID,
                              SampleTypeName = s.SampleTypeName,
                              UserID = s.UserID,
                              SetDate = s.SetDate
                          }).ToList();

            return result;
        }

        public Common.ViewModel.SampleTypeViewModel GetSampleTypeByID(int id)
        {
            var result = (from s in unitOfWork.SampleTypeRepository.Get()
                          where s.SampleTypeID == id
                          select new Common.ViewModel.SampleTypeViewModel
                          {
                              SampleTypeID = s.SampleTypeID,
                              SampleTypeName = s.SampleTypeName,
                              UserID = s.UserID,
                              SetDate = s.SetDate
                          }).SingleOrDefault();

            return result;
        }

        public List<DropDownListViewModel> GetSampleTypeTypeDropDown()
        {
            var result = (from s in unitOfWork.SampleTypeRepository.Get()
                          select new DropDownListViewModel
                          {
                              Text = s.SampleTypeName,
                              Value = s.SampleTypeID
                          }).Distinct().ToList();

            return result;
        }

        public List<DropDownListViewModel> GetSampleTypeNoDropDown(string sampleTypeName)
        {
            var result = (from s in unitOfWork.SampleTypeRepository.Get()
                          where s.SampleTypeName == sampleTypeName
                          select new DropDownListViewModel
                          {
                              Text = s.SampleTypeName,
                              Value = s.SampleTypeID
                          }).ToList();

            return result;
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
    }
}
