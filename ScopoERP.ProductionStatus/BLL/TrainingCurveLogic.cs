using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoERP.ProductionStatus.ViewModel;
using System.Data;
using System.Reflection;

namespace ScopoERP.ProductionStatus.BLL
{
    public class TrainingCurveLogic
    {
        private UnitOfWork unitOfWork;
        private TrainingCurve trainingCurve;
        private EmployeeRate empRate;

        public TrainingCurveLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void UpdateTrainingCurve(TrainingCurveViewModel trainingCurveVM)
        {
            trainingCurve = new TrainingCurve
            {
                TrainingCurveID = trainingCurveVM.TrainingCurveID,
                Stage = trainingCurveVM.Stage,
                Curve = trainingCurveVM.Curve,
                Curve_1 = trainingCurveVM.Curve_1,
                Curve_1A = trainingCurveVM.Curve_1A,
                Curve_2 = trainingCurveVM.Curve_2,
                Curve_3 = trainingCurveVM.Curve_3,
                Curve_4 = trainingCurveVM.Curve_4,
                Curve_5 = trainingCurveVM.Curve_5,
                Curve_6 = trainingCurveVM.Curve_6,
                Curve_New = trainingCurveVM.Curve_New
            };
            unitOfWork.TrainingCurveRepository.Update(trainingCurve);
            unitOfWork.Save();
        }

        public void CreateTrainingCurve(TrainingCurveViewModel trainingCurveVM)
        {
            trainingCurve = new TrainingCurve
            {
                Stage = trainingCurveVM.Stage,
                Curve = trainingCurveVM.Curve,
                Curve_1 = trainingCurveVM.Curve_1,
                Curve_1A = trainingCurveVM.Curve_1A,
                Curve_2 = trainingCurveVM.Curve_2,
                Curve_3 = trainingCurveVM.Curve_3,
                Curve_4 = trainingCurveVM.Curve_4,
                Curve_5 = trainingCurveVM.Curve_5,
                Curve_6 = trainingCurveVM.Curve_6,
                Curve_New = trainingCurveVM.Curve_New
            };
            unitOfWork.TrainingCurveRepository.Insert(trainingCurve);
            unitOfWork.Save();
        }

        public bool IsStageLeeserTwo(TrainingCurveViewModel trainingCurveVM)
        {
            var res = (from curve in unitOfWork.TrainingCurveRepository.Get()
                       where curve.Stage == trainingCurveVM.Stage
                       select curve).ToList();
            if (res.Count < 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<TrainingCurveViewModel> GetAllTrainingCurve()
        {
            var res = (from curve in unitOfWork.TrainingCurveRepository.Get()
                       select new TrainingCurveViewModel
                       {
                           TrainingCurveID = curve.TrainingCurveID,
                           Stage = curve.Stage,
                           Curve = curve.Curve,
                           Curve_1 = curve.Curve_1,
                           Curve_1A = curve.Curve_1A,
                           Curve_2 = curve.Curve_2,
                           Curve_3 = curve.Curve_3,
                           Curve_4 = curve.Curve_4,
                           Curve_5 = curve.Curve_5,
                           Curve_6 = curve.Curve_6,
                           Curve_New = curve.Curve_New

                       }).ToList();
            return res;
        }

        public EmployeeRateViewModel GetEmployeeeRateDetailsById(string cardNo)
        {
            EmployeeRateViewModel res = (from e in unitOfWork.EmployeeRateRepository.Get()
                                         where e.EmployeeCardNo == cardNo
                                         select new EmployeeRateViewModel
                                         {
                                             EmployeeRateID = e.EmployeeRateID,
                                             EmployeeCardNo = e.EmployeeCardNo,
                                             SpecNo = e.SpecNo,
                                             Curve = e.Curve,
                                             Section = e.Section,
                                             Stage = e.Stage,
                                             RTorNHorFL = e.RTorNHorFL
                                         }).SingleOrDefault();
            return res;
        }

        public void SaveEmployeeRateInfo(EmployeeRateViewModel empRateViewModel)
        {
            if (empRateViewModel.EmployeeRateID == 0)
            {
                empRate = new EmployeeRate
                {
                    EmployeeCardNo = empRateViewModel.EmployeeCardNo,
                    SpecNo = empRateViewModel.SpecNo,
                    Curve = empRateViewModel.Curve,
                    Stage = empRateViewModel.Stage,
                    Section = empRateViewModel.Section,
                    RTorNHorFL = empRateViewModel.RTorNHorFL
                };
                unitOfWork.EmployeeRateRepository.Insert(empRate);
            }
            else
            {
                empRate = new EmployeeRate
                {
                    EmployeeRateID = empRateViewModel.EmployeeRateID,
                    EmployeeCardNo = empRateViewModel.EmployeeCardNo,
                    SpecNo = empRateViewModel.SpecNo,
                    Curve = empRateViewModel.Curve,
                    Stage = empRateViewModel.Stage,
                    Section = empRateViewModel.Section,
                    RTorNHorFL = empRateViewModel.RTorNHorFL
                };
                unitOfWork.EmployeeRateRepository.Update(empRate);
            }
            unitOfWork.Save();
        }

        public object GetCurveInfo(int stage, string curve)
        {
            TargetViewModel target = new TargetViewModel();
            List<TrainingCurveViewModel> res = (from t in unitOfWork.TrainingCurveRepository.Get()
                                                where t.Stage == stage
                                                select new TrainingCurveViewModel
                                                {
                                                    Curve = t.Curve,
                                                    Curve_1 = t.Curve_1,
                                                    Curve_1A = t.Curve_1A,
                                                    Curve_2 = t.Curve_2,
                                                    Curve_3 = t.Curve_3,
                                                    Curve_4 = t.Curve_4,
                                                    Curve_5 = t.Curve_5,
                                                    Curve_6 = t.Curve_6,
                                                    Curve_New = t.Curve_New
                                                }).ToList();
           if(curve == "Curve_1")
            {
                target.Target_A = res[0].Curve_1;
                target.Target_B = res[1].Curve_1;
            }
            if (curve == "Curve_1A")
            {
                target.Target_A = res[0].Curve_1A;
                target.Target_B = res[1].Curve_1A;
            }
            if (curve == "Curve_2")
            {
                target.Target_A = res[0].Curve_2;
                target.Target_B = res[1].Curve_2;
            }
            if (curve == "Curve_3")
            {
                target.Target_A = res[0].Curve_3;
                target.Target_B = res[1].Curve_3;
            }
            if (curve == "Curve_4")
            {
                target.Target_A = res[0].Curve_4;
                target.Target_B = res[1].Curve_4;
            }
            if (curve == "Curve_5")
            {
                target.Target_A = res[0].Curve_5;
                target.Target_B = res[1].Curve_5;
            }
            if (curve == "Curve_6")
            {
                target.Target_A = res[0].Curve_6;
                target.Target_B = res[1].Curve_6;
            }
            return target;

        }
       

    }
}
