using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.BLL
{
    public class StandardOperationLogic
    {
        private UnitOfWork unitOfWork;
        private Operation operation;

        public StandardOperationLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public StandardOperationViewModel SaveStandardOperation(StandardOperationViewModel stdOperationVM)
        {
            if (stdOperationVM.OperationID != 0)
            {
                operation = new Operation
                {
                    OperationID = stdOperationVM.OperationID,
                    OperationName = stdOperationVM.OperationName,
                    OperationCodeNo = stdOperationVM.OperationCodeNo,
                    OperationCategoryID=stdOperationVM.OperationCategoryID,
                    JobClassID=stdOperationVM.JobClassID
                };
                unitOfWork.StandardOperationRepository.Update(operation);
                unitOfWork.Save();
                return stdOperationVM;
            }
            else
            {
                operation = new Operation
                {
                    OperationName = stdOperationVM.OperationName,
                    OperationCodeNo = stdOperationVM.OperationCodeNo,
                    OperationCategoryID = stdOperationVM.OperationCategoryID,
                    JobClassID = stdOperationVM.JobClassID
                };
                unitOfWork.StandardOperationRepository.Insert(operation);
                unitOfWork.Save();
                return GetStandardOperationByID(operation.OperationID);
            }
        }

        public void DeleteStandardOperationByID(int id)
        {
            unitOfWork.StandardOperationRepository.Delete(unitOfWork.StandardOperationRepository.GetById(id));
            unitOfWork.Save();
        }

        public List<StandardOperationViewModel> GetAllStandardOperations()
        {
            var result = (from st in unitOfWork.StandardOperationRepository.Get()
                          select new StandardOperationViewModel
                          {
                              OperationID = st.OperationID,
                              OperationName = st.OperationName,
                              OperationCodeNo = st.OperationCodeNo,
                              OperationCategoryID=st.OperationCategoryID,
                              JobClassID=st.JobClassID
                          }).OrderBy(x => x.OperationCodeNo).ToList();
            return result;

        }

        public StandardOperationViewModel GetStandardOperationByID(int id)
        {
            operation = unitOfWork.StandardOperationRepository.GetById(id);
            return new StandardOperationViewModel
            {
                OperationID = operation.OperationID,
                OperationName = operation.OperationName,
                OperationCodeNo = operation.OperationCodeNo,
                OperationCategoryID=operation.OperationCategoryID
            };
        }

        public bool IsUniqueOperation(StandardOperationViewModel standardOperationVM)
        {
            IQueryable<int> result;

            if (standardOperationVM.OperationID == 0)
            {
                result = (from st in unitOfWork.StandardOperationRepository.Get()
                          where st.OperationName.ToLower().Trim() == standardOperationVM.OperationName.Trim().ToLower() && st.OperationCategoryID == standardOperationVM.OperationCategoryID
                          && st.OperationCodeNo.ToLower().Trim() == standardOperationVM.OperationCodeNo.Trim().ToLower()
                          select st.OperationID);
            }
            else
            {
                result = (from st in unitOfWork.StandardOperationRepository.Get()
                          where st.OperationName.ToLower().Trim() == standardOperationVM.OperationName.ToLower().Trim() && st.OperationID != standardOperationVM.OperationID
                          && st.OperationCodeNo.ToLower().Trim() == standardOperationVM.OperationCodeNo.Trim().ToLower()
                          && st.OperationCategoryID == standardOperationVM.OperationCategoryID
                          select st.OperationID);
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<DropDownListViewModel> GetStandardOperationDropDown()
        {
            var data = (from st in unitOfWork.StandardOperationRepository.Get()
                        select new DropDownListViewModel
                        {
                            Text = st.OperationName,
                            Value = st.OperationID,
                            ValueString=st.OperationCodeNo
                        }).OrderBy(x => x.ValueString).ToList();
            return data;
        }

        public bool IsUniqueOperationCode(StandardOperationViewModel standardOperationVM)
        {
            IQueryable<int> result;

            if (standardOperationVM.OperationID == 0)
            {
                result = (from st in unitOfWork.StandardOperationRepository.Get()
                          where st.OperationCodeNo.ToLower().Trim() == standardOperationVM.OperationCodeNo.Trim().ToLower()
                          && st.OperationCategoryID == standardOperationVM.OperationCategoryID
                          select st.OperationID);
            }
            else
            {
                result = (from st in unitOfWork.StandardOperationRepository.Get()
                          where st.OperationCodeNo.ToLower().Trim() == standardOperationVM.OperationCodeNo.ToLower().Trim() && st.OperationID != standardOperationVM.OperationID
                          && st.OperationCategoryID == standardOperationVM.OperationCategoryID
                          select st.OperationID);
            }

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public List<OperationCategoryViewModel> GetAllOperationCategories()
        {
            return (from oc in unitOfWork.OperationCategoryRepository.Get()
                    select new OperationCategoryViewModel
                    {
                        OperationCategoryID = oc.OperationCategoryID,
                        OperationCategogyName=oc.OperationCategoryName
                    }
                ).ToList();
        }

        public List<DropDownListViewModel> GetAllOperationCategoriesDropDown()
        {
            return (from oc in unitOfWork.OperationCategoryRepository.Get()
                    select new DropDownListViewModel
                    {
                        Value = oc.OperationCategoryID,
                        Text = oc.OperationCategoryName
                    }
                ).ToList();
        }
    }
}
