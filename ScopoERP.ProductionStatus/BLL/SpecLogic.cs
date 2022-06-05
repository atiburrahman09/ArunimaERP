using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoERP.ProductionStatus.ViewModel;

namespace ScopoERP.ProductionStatus.BLL
{
    public class SpecLogic
    {
        private UnitOfWork unitOfWork;
        private Spec spec;

        public SpecLogic(UnitOfWork unitOfWork, Spec spec)
        {
            this.unitOfWork = unitOfWork;
            this.spec = spec;
        }

        public List<SpecViewModel> GetAllSpecs()
        {
            var result = (from st in unitOfWork.SpecRepository.Get()
                          select new SpecViewModel
                          {
                              SpecID = st.SpecID,
                              SpecName = st.SpecName,
                              SpecNo = st.SpecNo,
                              OperationID = st.OperationID
                          }).OrderByDescending(x => x.SpecID).ToList();
            return result;
        }

        public void SaveSpec(SpecViewModel specVM)
        {
            if (specVM.SpecID != 0)
            {
                spec = new Spec
                {
                    SpecID = specVM.SpecID,
                    SpecName = specVM.SpecName,
                    SpecNo = specVM.SpecNo,
                    OperationID = specVM.OperationID
                };
                unitOfWork.SpecRepository.Update(spec);
                unitOfWork.Save();
            }
            else
            {
                spec = new Spec
                {
                    SpecName = specVM.SpecName,
                    SpecNo = specVM.SpecNo,
                    OperationID = specVM.OperationID
                };
                unitOfWork.SpecRepository.Insert(spec);
                unitOfWork.Save();
            }
        }


        public bool IsUniqueSpec(SpecViewModel specVM)
        {
            IQueryable<int> result;

            if (specVM.SpecID == 0)
            {
                result = (from st in unitOfWork.SpecRepository.Get()
                          where st.SpecName.ToLower().Trim() == specVM.SpecName.Trim().ToLower()
                          && st.SpecNo.ToLower().Trim() == specVM.SpecNo.Trim().ToLower()
                          && st.OperationID == specVM.OperationID
                          select st.SpecID);
            }
            else
            {
                result = (from st in unitOfWork.SpecRepository.Get()
                          where st.SpecName.ToLower().Trim() == specVM.SpecName.ToLower().Trim() && st.SpecID != specVM.SpecID
                          && st.SpecNo.ToLower().Trim() == specVM.SpecNo.Trim().ToLower()
                          && st.OperationID == specVM.OperationID
                          select st.SpecID);
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool IsUniqueSpecCode(SpecViewModel specVM)
        {
            IQueryable<int> result;

            if (specVM.SpecID == 0)
            {
                result = (from st in unitOfWork.SpecRepository.Get()
                          where st.SpecNo.ToLower().Trim() == specVM.SpecNo.Trim().ToLower()
                          && st.OperationID == specVM.OperationID
                          select st.SpecID);
            }
            else
            {
                result = (from st in unitOfWork.SpecRepository.Get()
                          where st.SpecNo.ToLower().Trim() == specVM.SpecNo.ToLower().Trim() && st.SpecID != specVM.SpecID
                          && st.OperationID == specVM.OperationID
                          select st.SpecID);
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<SpecViewModel> GetSpec()
        {
            var res = (from s in unitOfWork.SpecRepository.Get()
                       //where s.OperationID == operationID
                       select new SpecViewModel
                       {
                           SpecID = s.SpecID,
                           SpecNo = s.SpecNo,
                           SpecName = s.SpecName,
                           OperationID=s.OperationID
                       }).ToList();
            return res;
        }
    }
}
