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
    public class CuttingController : Controller
    {
        private CuttingPlanLogic cuttingPlanLogic;
        private StyleLogic styleLogic;
        private BuyerLogic buyerLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private CouponLogic couponLogic;
        private SupervisorLogic supervisorLogic;

        public CuttingController(CuttingPlanLogic cuttingPlanLogic, StyleLogic styleLogic, BuyerLogic buyerLogic, PurchaseOrderLogic purchaseOrderLogic, CouponLogic couponLogic, SupervisorLogic supervisorLogic)
        {
            this.cuttingPlanLogic = cuttingPlanLogic;
            this.styleLogic = styleLogic;
            this.buyerLogic = buyerLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.couponLogic = couponLogic;
            this.supervisorLogic = supervisorLogic;
        }

        // GET: Production/Cutting
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
            List<StyleViewModel> stylelist = styleLogic.GetAllStyleByBuyer(accountId, buyerId);
            return Json(stylelist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseOrders(int StyleID)
        {
            List<PurchaseOrderViewModel> purchaseOrderList = purchaseOrderLogic.GetAllPurchaseOrderByStyleID(StyleID);
            return Json(purchaseOrderList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCuttingClassList()
        {
            try
            {
                return Json(cuttingPlanLogic.GetAllCuttingPlan(), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult SaveCuttingClass(CuttingPlanViewModel cuttingPlanViewModel)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (!cuttingPlanLogic.IsUnique(cuttingPlanViewModel))
                {
                    int cuttingPLanID = cuttingPlanLogic.CreatecuttingPlan(cuttingPlanViewModel);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Message = "Cutting Class Successfully Created", ID = cuttingPLanID });

                }
                return Json(new { Message = "Cutting no already exists.", ID = 0 });

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult UpdateCuttingClass(CuttingPlanViewModel cuttingPlanViewModel)
        {
            
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if(!couponLogic.IsCouponExists(cuttingPlanViewModel.CuttingPlanID))
                {
                    cuttingPlanLogic.UpdatecuttingPlan(cuttingPlanViewModel);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(true);
                }

                return Json(false);

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetCuttingDetails(int POId)
        {
            List<CuttingPlanViewModel> cuttingDetails = cuttingPlanLogic.GetCuttingDetails(POId);
            return Json(cuttingDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBundleInformation(List<BundleViewModel> bundleVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                cuttingPlanLogic.CreateBundle(bundleVM);
                Response.StatusCode = (int)HttpStatusCode.Created;
                return Json(new { Message = "Bundle Successfully Created" });

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult UpdateBundleInformation(List<BundleViewModel> bundleVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                cuttingPlanLogic.UpdateBundle(bundleVM);
                Response.StatusCode = (int)HttpStatusCode.Created;
                return Json(true);

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetBundleInfoByCuttingID(int CuttingPLanID)
        {
            List<BundleViewModel> bundleList = cuttingPlanLogic.GetBundleInfoByCuttingID(CuttingPLanID);
            return Json(bundleList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCuttingListByPOID(int POID)
        {
            try
            {
                return Json(cuttingPlanLogic.GetCuttingPlanByPOID(POID), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetLastBundleNo()
        {
            int lastBundleNO = cuttingPlanLogic.GetLastBundleNo();
            return Json(lastBundleNO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSupervisors()
        {
            try
            {
                return Json(supervisorLogic.GetAllSupervisors(), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

    }
}