using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.Production.ViewModel;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.ProductionStatus.BLL
{
    public class StyleOperationLogic
    {
        private UnitOfWork unitOfWork;
        private StyleOperation styleOperation;

        public StyleOperationLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void createStyleOperation(List<StyleOperationViewModel> operationList)
        {
            unitOfWork.StyleOperationRepository.RawQuery("DELETE FROM StyleOperations WHERE StyleID = " + operationList[0].StyleID);

            var SizeList = operationList[0].SizeListVM;

            if (operationList != null)
            {
                foreach (var operation in operationList)
                {
                    var i = 0;
                    foreach (var size in operation.SizeListVM)
                    {
                        styleOperation = new StyleOperation
                        {
                            StyleID = operation.StyleID,
                            sam = size.Sam,
                            OperationID = operation.OperationID,
                            SpecID=operation.SpecID,
                            AuxSam = operation.AuxSam,
                            MachineID = operation.MachineID,
                            SectionNo = operation.SectionNo,
                            SupervisorID = operation.SupervisorID,
                            Size = SizeList[i].Size
                        };
                        unitOfWork.StyleOperationRepository.Insert(styleOperation);
                        i++;
                    }

                }
            }

            unitOfWork.Save();

        }

        public List<StyleOperationViewModel> GetStyleOperationListByStyleID(int styleId)
        {
            List<StyleOperationViewModel> styleList = new List<StyleOperationViewModel>();

            int k = 0;

            var result = (from s in unitOfWork.StyleOperationRepository.Get()
                          join st in unitOfWork.StandardOperationRepository.Get() on s.OperationID equals st.OperationID into group1
                          from g in group1.DefaultIfEmpty()
                          where s.StyleID == styleId
                          select new StyleOperationViewModel
                          {
                              StyleOperationID = s.StyleOperationID,
                              StyleID = s.StyleID,
                              Sam = s.sam,
                              OperationID = s.OperationID,
                              SpecID=s.SpecID,
                              StandardOperationName = g.OperationName,
                              MachineID = s.MachineID,
                              SectionNo = s.SectionNo,
                              SupervisorID = s.SupervisorID,
                              Size = s.Size,
                              AuxSam=s.AuxSam
                          }).ToList();

            for (int i = 0; i < result.Count(); i++)
            {
                StyleOperationViewModel styleVM = new StyleOperationViewModel();
                List<SizeListViewModel> sizeList = new List<SizeListViewModel>();
                styleVM.StyleOperationID = result[i].StyleOperationID;
                styleVM.StyleID = result[i].StyleID;
                //styleVM.Sam = result[i].Sam;
                styleVM.AuxSam = result[i].AuxSam;
                styleVM.OperationID = result[i].OperationID;
                styleVM.SpecID = result[i].SpecID;
                styleVM.StandardOperationName = result[i].StandardOperationName;
                styleVM.MachineID = result[i].MachineID;
                styleVM.SectionNo = result[i].SectionNo;
                styleVM.SupervisorID = result[i].SupervisorID;

                //second loop to get all size list for a single Operation
                for (int j = i; j < result.Count(); j++)
                {
                    if (result[j].SpecID == result[i].SpecID)
                    {
                        k++;
                        SizeListViewModel sizeVM = new SizeListViewModel();
                        sizeVM.Size = result[j].Size;
                        sizeVM.Sam = result[j].Sam;

                        sizeList.Add(sizeVM);
                    }
                }
                i = k - 1;

                styleVM.SizeListVM = sizeList;
                styleList.Add(styleVM);
            }
            return styleList;
        }

        public object GetStyleOperationJobClassListByStyleID(int styleID)
        {
            var result = (from s in unitOfWork.StyleOperationRepository.Get()
                          join st in unitOfWork.StandardOperationRepository.Get() on s.OperationID equals st.OperationID into group1
                          from g in group1.DefaultIfEmpty()
                          join spec in unitOfWork.SpecRepository.Get() on g.OperationID equals spec.OperationID into group2
                          from sp in group2.DefaultIfEmpty()
                          join j in unitOfWork.JobClassRepository.Get() on g.JobClassID equals j.JobClassID into group3
                          from job in group3.DefaultIfEmpty()
                          where s.StyleID == styleID
                          select new CouponViewModel
                          {
                              StyleOperationID = s.StyleOperationID,
                              StyleID = s.StyleID,
                              Sam = s.sam,
                              AuxSam = s.AuxSam,
                              OperationID = s.OperationID,
                              OperationName = g.OperationName,
                              JobClassName = job.JobClassName,
                              BaseRate = job.BaseRate,
                              SpecNo=sp.SpecNo,
                              SectionNo = s.SectionNo,
                              SupervisorID = s.SupervisorID
                          }).ToList();
            return result;
        }
    }
}
