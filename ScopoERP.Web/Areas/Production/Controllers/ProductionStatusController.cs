using ScopoERP.Common.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using ScopoERP.Production.BLL;
using ScopoERP.Production.ViewModel;
using ScopoERP.ProductionStatus.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Stackholder.ViewModel;
using ScopoERP.Web.Reports;
using ScopoERP.WebUI.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    [Authorize(Roles = "Dpr-Entry")]
    
    public class ProductionStatusController : Controller
    {
        private ProductionStatusLogic productionStatusLogic;
        private StyleLogic styleLogic;
        private ProductionFloorLogic productionFloorLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        //```````````````````````````````````````````\\
        private BuyerLogic buyerLogic;

        public ProductionStatusController(ProductionStatusLogic productionStatusLogic, ProductionFloorLogic productionFloorLogic,
                                            StyleLogic styleLogic, PurchaseOrderLogic purchaseOrderLogic, BuyerLogic buyerLogic)
        {
            this.productionStatusLogic = productionStatusLogic;
            this.styleLogic = styleLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.productionFloorLogic = productionFloorLogic;
            //````````````````````````````````````````````````\\
            this.buyerLogic = buyerLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        
        public JsonResult GetAllBuyer()
        {
            List<BuyerViewModel> buyerList = buyerLogic.GetAllBuyer();
            return Json(buyerList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStylesByBuyerID(int accountID, int buyerID)
        {
            List<DropDownListViewModel> styleList = styleLogic.GetStyleDropDownByBuyerID(accountID, buyerID);
            return Json(styleList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionFloorDropDown()
        {
            return Json(productionFloorLogic.GetFloorDropDown(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionLineByFloor(string floor)
        {
            return Json(productionFloorLogic.GetLineDropDownByFloor(floor), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFilteredProductionStatus(ProductionStatusFilterViewModel filterVM)
        {
            var data = productionStatusLogic.GetFilteredProductionStatus(filterVM);
            return Json(data);
        }

        public JsonResult GetAllStyle()
        {
            var data = styleLogic.GetStyleDropDown();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllProductionLine()
        {
            var data = productionFloorLogic.GetLineDropDown();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveProductionStatus(List<ProductionStatusViewModel> statusList)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    productionStatusLogic.SaveProductionStatus(statusList);
                    return Json(true);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.StackTrace);
                    return Json(ex.InnerException.StackTrace);
                }
            }

            return Json(false);
        }

        public JsonResult GetPurchaseOrdersByStyleIDs(int[] list)
        {
            var data = purchaseOrderLogic.GetPurchaseOrderDropDown(list);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        // New Methods

        public JsonResult GetPurchaseOrdersByStyleID(int Value)
        {
            var data = purchaseOrderLogic.GetPurchaseOrderDropDown(Value);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetStyleDropDownByKeyword(string inputString, int buyerID)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please enter style name .", JsonRequestBehavior.AllowGet);
            }

            try
            {
                List<DropDownListViewModel> styleList = styleLogic.GetStyleDropDownByBuyerID(inputString, buyerID);
                return Json(styleList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult GetBuyerDropDownByKeyword(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please enter Buyer name .", JsonRequestBehavior.AllowGet);
            }

            try
            {
                List<BuyerViewModel> buyerList = buyerLogic.GetAllBuyerDropDown(inputString);
                return Json(buyerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult RemoveProductionStatus(int ProductionDailyReportId)
        {
            try
            {
                productionStatusLogic.RemoveProductionStatus(ProductionDailyReportId);
                return Json("Data successfully removed.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}