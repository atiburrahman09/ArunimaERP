using ScopoERP.Common.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using ScopoERP.ProductionStatus.BLL;
using ScopoERP.ProductionStatus.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Stackholder.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    [Authorize(Roles = "Production")]
    public class CouponController : Controller
    {

        private StyleLogic styleLogic;
        private CouponLogic couponLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private CuttingPlanLogic cuttingPlanLogic;
        private StyleOperationLogic styleOperationLogic;
        private BuyerLogic buyerLogic;
        private StandardOperationLogic stdOperationLogic;
        private EmployeeCapabilityService empCapabilityService;
        private TrainingCurveLogic trainingCurveLogic;

        public CouponController(StyleLogic styleLogic, CouponLogic couponLogic, PurchaseOrderLogic purchaseOrderLogic, CuttingPlanLogic cuttingPlanLogic, StyleOperationLogic styleOperationLogic, BuyerLogic buyerLogic, StandardOperationLogic stdOperationLogic, EmployeeCapabilityService empCapabilityService, TrainingCurveLogic trainingCurveLogic)
        {
            this.styleLogic = styleLogic;
            this.couponLogic = couponLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.cuttingPlanLogic = cuttingPlanLogic;
            this.styleOperationLogic = styleOperationLogic;
            this.buyerLogic = buyerLogic;
            this.stdOperationLogic = stdOperationLogic;
            this.empCapabilityService = empCapabilityService;
            this.trainingCurveLogic = trainingCurveLogic;
        }


        // GET: Production/Coupon
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult getAllBuyer()
        {
            List<BuyerViewModel> buyerList = buyerLogic.GetAllBuyer();
            return Json(buyerList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllStyle(int accountId, int buyerId)
        {
            List<DropDownListViewModel> stylelist = styleLogic.GetStyleDropDownByBuyerID(accountId, buyerId);
            return Json(stylelist, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetStyleDropDown()
        //{
        //    try
        //    {
        //        return Json(styleLogic.GetStyleDropDown(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //POList
        public JsonResult GetPOList(int styleID)
        {
            try
            {
                return Json(purchaseOrderLogic.GetPurchaseOrderDropDown(styleID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //cutplanList
        public JsonResult GetCuttingPlanList(int poID)
        {
            try
            {
                return Json(cuttingPlanLogic.GetCuttingPlanDropDown(poID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStyleInformation(int StyleID)
        {
            try
            {
                return Json(styleOperationLogic.GetStyleOperationJobClassListByStyleID(StyleID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult Save(List<CouponViewModel> couponListVM)
        {
            try
            {
                couponLogic.Save(couponListVM);
                return Json(true);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GenerateCouponInformation(int CuttingPlanID, int StyleID, int PurchaseOrderID, int CutPlanNo, int operationCategoryID)
        {
            try
            {
                if (!couponLogic.IsCouponExistsForCutPlan(CuttingPlanID, operationCategoryID))
                {
                    var couponList = couponLogic.GetCouponInformation(CuttingPlanID, StyleID, PurchaseOrderID, CutPlanNo, operationCategoryID);
                    return Json(new {list= couponList, ErrorCode=false}, JsonRequestBehavior.AllowGet);
                }
                return Json(new {ErrorCode = true}, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteCoupon(int cuttingPlanID , int operationCategoryID)
        {
            try
            {
                couponLogic.DeleteCoupon(cuttingPlanID, operationCategoryID);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //coupon entry
        public ActionResult CouponEntry()
        {
            return View();
        }

        public JsonResult GetAllCoupons()
        {
            try
            {
                return Json(couponLogic.GetCouponDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        
        public JsonResult GetAllBundles(string specNo)
        {
            try
            {
                return Json(couponLogic.GetBundleDropDownBySpecNo(specNo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SaveAssignCoupon(AssignCouponViewModel assignCouponVM)
        {
            try
            {
                var i = couponLogic.SaveAssignCoupon(assignCouponVM);
                return Json(i, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult IsUniqueCouponEntry(int couponID)
        {
            try
            {
                return Json(couponLogic.IsUniqueCouponEntry(couponID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateGumSheet(GumSheetViewModel gumsheetVM, List<string> bundleList)
        {
            try
            {
                var res = couponLogic.CreateGumSheet(gumsheetVM, bundleList);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateCoupon(List<string> bundleList, string employeeCardNo, string specNo)
        {
            try
            {
                couponLogic.UpdateCoupon(bundleList, employeeCardNo, specNo);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SearchCoupon(int cuttingPlanID, int PurchaseOrderID, int cutPlanNo, int OperationCategoryID)
        {
            try
            {

                List<CouponViewModel> couponList=couponLogic.SearchCoupon(cuttingPlanID, PurchaseOrderID, cutPlanNo, OperationCategoryID);
                return Json(couponList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllOperationCategories()
        {
            return Json(stdOperationLogic.GetAllOperationCategories(), JsonRequestBehavior.AllowGet);
        }

        //Calculation For GumSheet OffStandard

        public JsonResult IsEmpAssignedToSpec(string empCardNo, string specNo)
        {
            try
            {
                return Json(couponLogic.IsEmpAssignedToSpec(empCardNo, specNo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetValueForCalculation(string empCardNo, DateTime completedDate, string specNo)
        {
            try
            {
                return Json(couponLogic.GetValueForCalculation(empCardNo, completedDate, specNo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmployeeLearningCurve(string empCardNo)
        {
            try
            {
                return Json(couponLogic.GetEmployeeLearningCurve(empCardNo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGumSheetData(string empCardNo, DateTime completedDate)
        {
            try
            {
                return Json(couponLogic.GetGumSheetData(empCardNo,completedDate), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCurveInfo(int stage, string curve)
        {
            try
            {
                return Json(trainingCurveLogic.GetCurveInfo(stage, curve), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CreateOffStandard(gumSheetOffStandanrdViewModel offStandardViewModel)
        {
            try
            {
                return Json(couponLogic.CreateOffStandard(offStandardViewModel), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
