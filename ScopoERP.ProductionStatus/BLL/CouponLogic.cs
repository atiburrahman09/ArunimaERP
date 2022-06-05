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
    public class CouponLogic
    {
        private UnitOfWork unitOfWork;

        private CuttingPlanLogic cuttingPlanLogic;
        private StyleOperationLogic styleOperationlogic;
        private coupon coupon;
        private gumSheetOffStandard offstandard;

        public CouponLogic(UnitOfWork unitOfWork, CuttingPlanLogic cuttingPlanLogic, StyleOperationLogic styleOperationlogic)
        {
            this.unitOfWork = unitOfWork;
            this.cuttingPlanLogic = cuttingPlanLogic;
            this.styleOperationlogic = styleOperationlogic;
        }

        public void Save(List<CouponViewModel> couponListVM)
        {
            foreach (var c in couponListVM)
            {
                coupon = new coupon
                {
                    BundleID = c.BundleID,
                    OperationID = c.OperationID,
                    SpecID = c.SpecID,
                    BaseRate = Convert.ToDecimal(c.BaseRate),
                    Value = c.Value,
                    EmployeeCardNo = c.EmployeeCardNo,
                    CompletedDate = DateTime.Now,
                    SerialNo = c.SerialNo,
                    CuttingPlanID = c.CuttingPlanID,
                    OperationCategoryID = c.OperationCategory,
                    StyleOperationID = c.StyleOperationID,
                    JobClassID = c.JobClassID,
                    PoStyleId = c.PoStyleId,
                    Size = c.Size,
                    Quantity = c.Quantity,
                    Time = c.Time,
                    SectionNo = c.SectionNo,
                    SupervisorID = c.SupervisorID


                };
                unitOfWork.CouponRepository.Insert(coupon);
            }
            unitOfWork.Save();
        }

        public object GetCouponInformation(int cuttingPlanID, int styleID, int PurchaseOrderID, int cutPlanNo, int operationCategoryID)
        {
            int k = 0;

            List<CouponViewModel> couponList = new List<CouponViewModel>();
            CouponViewModel coupon;

            var bundleList = unitOfWork.BundleRepository.Get().Where(x => x.CuttingPlanID == cuttingPlanID).ToList();
            var purchaseInfo = unitOfWork.PurchaseOrderRepository.Get().Where(x => x.PoStyleId == PurchaseOrderID).SingleOrDefault();

            foreach (var bundle in bundleList)
            {
                var styleOpearationList = (from spec in unitOfWork.SpecRepository.Get()
                                           join s in unitOfWork.StyleOperationRepository.Get() on spec.SpecID equals s.SpecID
                                           join st in unitOfWork.StandardOperationRepository.Get() on s.OperationID equals st.OperationID
                                           join j in unitOfWork.JobClassRepository.Get() on st.JobClassID equals j.JobClassID
                                           where s.StyleID == styleID && s.Size == bundle.Size && st.OperationCategoryID == operationCategoryID
                                           select new CouponViewModel
                                           {
                                               StyleOperationID = s.StyleOperationID,
                                               StyleID = s.StyleID,
                                               AuxSam = s.AuxSam,
                                               Sam = s.sam,
                                               OperationID = st.OperationID,
                                               SpecID = spec.SpecID,
                                               OperationName = st.OperationName,
                                               JobClassName = j.JobClassName,
                                               BaseRate = j.BaseRate,
                                               SpecNo = spec.SpecNo,
                                               SectionNo = s.SectionNo,
                                               SupervisorID = s.SupervisorID,
                                               Size = s.Size,
                                               SpecName = spec.SpecName,
                                               JobClassID = j.JobClassID

                                           }).ToList();


                foreach (var styleOpearation in styleOpearationList)
                {
                    coupon = new CouponViewModel();

                    coupon.OperationID = styleOpearation.OperationID;
                    coupon.SpecID = styleOpearation.SpecID;
                    coupon.BundleID = bundle.BundleID;
                    coupon.BaseRate = styleOpearation.BaseRate;
                    coupon.SerialNo = k++;
                    coupon.SpecName = styleOpearation.SpecName;
                    coupon.SpecNo = styleOpearation.SpecNo;
                    coupon.OperationName = styleOpearation.OperationName;
                    coupon.PurchaseOrderNo = purchaseInfo.PoNo;
                    coupon.PoStyleId = purchaseInfo.PoStyleId;
                    coupon.BundleNo = bundle.BundleNo;
                    coupon.Size = bundle.Size;
                    coupon.Quantity = bundle.Quantity;
                    coupon.JobClassID = styleOpearation.JobClassID;
                    coupon.JobClassName = styleOpearation.JobClassName;
                    coupon.Time = (decimal)(styleOpearation.AuxSam + styleOpearation.Sam * bundle.Quantity);
                    coupon.CutNo = cutPlanNo;
                    coupon.CuttingPlanID = bundle.CuttingPlanID;
                    coupon.type = "item";
                    coupon.SectionNo = styleOpearation.SectionNo;
                    coupon.SupervisorID = styleOpearation.SupervisorID;
                    coupon.OperationCategory = operationCategoryID;
                    coupon.Value = ((decimal)(coupon.Time / 60) * styleOpearation.BaseRate);
                    coupon.StyleOperationID = styleOpearation.StyleOperationID;

                    couponList.Add(coupon);
                }
            }
            return couponList;
        }

        public bool IsCouponExistsForCutPlan(int cuttingPlanID, int operationCategoryID)
        {
            var result = (from cp in unitOfWork.CouponRepository.Get()
                          where cp.CuttingPlanID == cuttingPlanID && cp.OperationCategoryID == operationCategoryID
                          select cp).ToList();
            if (result.Count > 0)
            {
                return true;
            }
            return false;
        }

        public List<DropDownListViewModel> GetBundleDropDownBySpecNo(string specNo)
        {
            var specInfo = unitOfWork.SpecRepository.Get().Where(x => x.SpecNo == specNo).SingleOrDefault();
            var result = (from c in unitOfWork.CouponRepository.Get()
                          join b in unitOfWork.BundleRepository.Get() on c.BundleID equals b.BundleID
                          where c.SpecID == specInfo.SpecID && c.EmployeeCardNo == null
                          select new DropDownListViewModel
                          {
                              Text = b.BundleNo,
                              Value = b.BundleID,
                              CouponValue = c.Value,
                              CouponTime = c.Time
                          }).ToList();

            return result;
        }

        public bool IsCouponExists(int cuttingPLanID)
        {
            var result = (from cp in unitOfWork.CouponRepository.Get()
                          where cp.CuttingPlanID == cuttingPLanID
                          select cp).ToList();
            if (result.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void DeleteCoupon(int cuttingPlanID, int operationCategoryID)
        {
            unitOfWork.CouponRepository.RawQuery("DELETE FROM coupons WHERE CuttingPlanID = '" + cuttingPlanID + "' AND OperationCategoryID = '" + operationCategoryID + "'");
        }

        public List<DropDownListViewModel> GetCouponDropDown()
        {
            var result = (from c in unitOfWork.CouponRepository.Get()
                          select new { c.SerialNo, c.CouponID }).AsEnumerable()
                          .Select(x => new DropDownListViewModel { Text = x.SerialNo.ToString(), Value = x.CouponID })
                          .ToList();

            return result;
        }

        public List<DropDownListViewModel> GetBundleDropDown()
        {
            var result = (from c in unitOfWork.BundleRepository.Get()
                          select new { c.BundleID, c.BundleNo }).AsEnumerable()
                          .Select(x => new DropDownListViewModel { Text = x.BundleNo.ToString(), Value = x.BundleID })
                          .ToList();

            return result;
        }

        public bool SaveAssignCoupon(AssignCouponViewModel assignCouponVM)
        {
            foreach (var cp in assignCouponVM.CouponList)
            {
                coupon couponObj = (from c in unitOfWork.CouponRepository.Get()
                                    where c.SerialNo == cp.CouponNo
                                    select c).SingleOrDefault();


                couponObj.EmployeeCardNo = assignCouponVM.EmployeeCardNo;
                couponObj.CompletedDate = assignCouponVM.CompletedDate;
                unitOfWork.CouponRepository.Update(couponObj);

            }
            unitOfWork.Save();

            return true;
        }

        public bool IsUniqueCoupon(int cuttingPlanID, int styleOperationID)
        {
            bool ifExists = unitOfWork.CouponRepository.Get().Any(c => c.CuttingPlanID == cuttingPlanID && c.OperationID == styleOperationID);

            return ifExists;
        }

        public bool IsEmpAssignedToSpec(string empCardNo, string specNo)
        {
            var res = (from emp in unitOfWork.EmployeeCapabilityRepository.Get()
                       where emp.EmployeeCardNo == empCardNo && emp.SpecNo == specNo
                       select emp).ToList();
            if (res.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<offStandardRateViewModel> GetValueForCalculation(string empCardNo, DateTime completedDate, string specNo)
        {
            List<offStandardRateViewModel> offStandardRate = unitOfWork.EmployeeCapabilityRepository.SelectQuery<offStandardRateViewModel>("EXEC GetOffStandardValueForCalculation '" + empCardNo + "','" + completedDate + "','" + specNo + "'").ToList();

            if (offStandardRate.Count > 0)
            {
                //O1 - TB_Min_Pay_Per_Min
                if (offStandardRate[0].V6 > offStandardRate[0].V3)
                {
                    offStandardRate[0].O1 = offStandardRate[0].V6;
                }
                else
                {
                    offStandardRate[0].O1 = offStandardRate[0].V3;
                }

                //O1A - OPC_RATE
                if (offStandardRate[0].V6 < offStandardRate[0].V3)
                {
                    offStandardRate[0].O1A = offStandardRate[0].V6;
                }
                else
                {
                    offStandardRate[0].O1A = offStandardRate[0].V3;
                }

                //O1B - CC_RATE 
                if (offStandardRate[0].V6 == 0)
                {
                    offStandardRate[0].O1B = offStandardRate[0].V6;
                }
                else
                {
                    offStandardRate[0].O1B = offStandardRate[0].V3;
                }

                //O4 - OT_PAY_PER_MIN
                offStandardRate[0].O4 = (offStandardRate[0].V3) / 1.4;

                //O5 - HO_PAY_PER_MIN
                offStandardRate[0].O5 = (offStandardRate[0].V3) / 1.4;
            }

            return offStandardRate;
        }

        public object GetEmployeeLearningCurve(string empCardNo)
        {
            var res = (from lc in unitOfWork.EmployeeRateRepository.Get()
                       where lc.EmployeeCardNo == empCardNo
                       select new EmployeeRateViewModel
                       {
                           EmployeeRateID = lc.EmployeeRateID,
                           EmployeeCardNo = lc.EmployeeCardNo,
                           Curve = lc.Curve,
                           RTorNHorFL = lc.RTorNHorFL,
                           Section = lc.Section,
                           SpecNo = lc.SpecNo,
                           Stage = lc.Stage
                       }).SingleOrDefault();
            return res;
        }

        public object CreateOffStandard(gumSheetOffStandanrdViewModel offStandardViewModel)
        {
            var specAndOperationID = (from s in unitOfWork.SpecRepository.Get()
                                      join o in unitOfWork.StandardOperationRepository.Get() on s.OperationID equals o.OperationID
                                      where s.SpecNo == offStandardViewModel.SpecNo
                                      select new { s.SpecID, o.OperationID }).AsEnumerable()
                                      .Select(x => new gumSheetOffStandanrdViewModel { OperationID = x.OperationID, SpecID = x.SpecID }).SingleOrDefault();

            foreach (var o in offStandardViewModel.OffStandardVm)
            {
                if (o.OffStandardText != null)
                {
                    offstandard = new gumSheetOffStandard
                    {
                        EmployeeCardNo = offStandardViewModel.EmployeeCardNo,
                        OperationID = specAndOperationID.OperationID,
                        SpecID = specAndOperationID.SpecID,
                        Section = offStandardViewModel.Section,
                        WorkingDate = offStandardViewModel.CompletedDate,
                        NonStandCode = o.OffStandardText,
                        Duration = o.offStandanrdDuration,
                        EGP = o.Value
                    };
                    unitOfWork.GumSheetOffStandardRepository.Insert(offstandard);
                }
               
            }

            unitOfWork.Save();
            return true;
        }

        public object GetGumSheetData(string empCardNo, DateTime completedDate)
        {
            List<GumSheetDataViewModel> gumSheetData = unitOfWork.GumSheetRepository.SelectQuery<GumSheetDataViewModel>("EXEC GetGhumSheetData '" + empCardNo + "','" + completedDate + "'").ToList();
            return gumSheetData;
        }

        public void UpdateCoupon(List<string> bundleVM, string employeeCardNo, string specNo)
        {
            foreach (var b in bundleVM)
            {
                var operationSpecInfo = (from s in unitOfWork.SpecRepository.Get()
                                          join o in unitOfWork.StandardOperationRepository.Get() on s.OperationID equals o.OperationID
                                          where s.SpecNo == specNo
                                         select new { s.SpecID, o.OperationID }).AsEnumerable()
                                       .Select(x => new gumSheetOffStandanrdViewModel { OperationID = x.OperationID, SpecID = x.SpecID }).SingleOrDefault();
                var bundleInfo = unitOfWork.BundleRepository.Get().Where(x => x.BundleNo == b).SingleOrDefault();


                var coupon = (from c in unitOfWork.CouponRepository.Get()
                              where c.BundleID == bundleInfo.BundleID
                              && c.SpecID == operationSpecInfo.SpecID
                              select c).SingleOrDefault();
                coupon.EmployeeCardNo = employeeCardNo;
                unitOfWork.CouponRepository.Update(coupon);
                unitOfWork.Save();
            }
        }

        public bool IsUniqueCouponEntry(int couponID)
        {
            bool ifExists = unitOfWork.CouponRepository.Get().Any(c => c.CouponID == couponID && c.EmployeeCardNo != null);
            return ifExists;
        }

        public bool CreateGumSheet(GumSheetViewModel gumsheetVM, List<string> bundleList)
        {
            //var operationInfo = unitOfWork.StandardOperationRepository.Get().Where(x => x.OperationCodeNo == gumsheetVM.OperationNo).SingleOrDefault();
            var specInfo = unitOfWork.SpecRepository.Get().Where(x => x.SpecNo == gumsheetVM.SpecNo).SingleOrDefault();

            foreach (var b in bundleList)
            {
                var bundleInfo = unitOfWork.BundleRepository.Get().Where(x => x.BundleNo == b).SingleOrDefault();
                var res = (from c in unitOfWork.CouponRepository.Get()
                           where c.BundleID == bundleInfo.BundleID && c.SpecID == specInfo.SpecID
                           select c).SingleOrDefault();

                gumsheet gumsheetObj = new gumsheet
                {
                    CompletedDate = Convert.ToDateTime(gumsheetVM.CompletedDate),
                    EmployeeCardNo = gumsheetVM.EmployeeCardNo,
                    SpecID = specInfo.SpecID,
                    BundleNo = bundleInfo.BundleNo,
                    Duration = res.Time,
                    EGP = res.Value,
                    ClockedTime = gumsheetVM.ClockedTime,
                    Section = gumsheetVM.Section,
                    MachineTrouble = gumsheetVM.MachineTrouble,
                    PayMethod = gumsheetVM.PayMethod,
                    LearningCurve = gumsheetVM.LearningCurve,
                    Allowance = gumsheetVM.Allowance
                };

                unitOfWork.GumSheetRepository.Insert(gumsheetObj);
            }



            unitOfWork.Save();
            return true;
        }

        public List<CouponViewModel> SearchCoupon(int cuttingPlanID, int PurchaseOrderID, int cutPlanNo, int operationCategoryID)
        {
            //var bundleList = unitOfWork.BundleRepository.Get().Where(x => x.CuttingPlanID == cuttingPlanID).ToList();
            var purchaseInfo = unitOfWork.PurchaseOrderRepository.Get().Where(x => x.PoStyleId == PurchaseOrderID).SingleOrDefault();

            var res = (from c in unitOfWork.CouponRepository.Get()
                           //join s in unitOfWork.StyleOperationRepository.Get() on c.StyleOperationID equals s.StyleOperationID
                       join st in unitOfWork.StandardOperationRepository.Get() on c.OperationID equals st.OperationID
                       join spec in unitOfWork.SpecRepository.Get() on st.OperationID equals spec.OperationID
                       join j in unitOfWork.JobClassRepository.Get() on st.JobClassID equals j.JobClassID
                       join b in unitOfWork.BundleRepository.Get() on c.BundleID equals b.BundleID
                       where c.CuttingPlanID == cuttingPlanID && st.OperationCategoryID == operationCategoryID
                       select new CouponViewModel
                       {
                           OperationID = st.OperationID,
                           Value = c.Value,
                           BundleID = c.BundleID,
                           BaseRate = j.BaseRate,
                           SpecNo = spec.SpecNo,
                           SpecName = spec.SpecName,
                           OperationName = st.OperationName,
                           PurchaseOrderNo = purchaseInfo.PoNo,
                           BundleNo = b.BundleNo,
                           Size = b.Size,
                           Quantity = b.Quantity,
                           JobClassName = j.JobClassName,
                           Time = c.Time,
                           CutNo = cutPlanNo,
                           CuttingPlanID = cuttingPlanID,
                           type = "item",
                           SectionNo = c.SectionNo,
                           SupervisorID = c.SupervisorID
                       }
            ).ToList();

            return res;
        }
    }
}
