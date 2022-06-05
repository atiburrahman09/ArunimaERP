using ScopoERP.Common.BLL;
using ScopoERP.Common.ViewModel;
using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Merchandising.Controllers
{
    [Authorize(Roles = "merchant")]
    public class PurchaseOrderController : Controller
    {
        private PurchaseOrderLogic purchaseOrderLogic;
        private StyleLogic styleLogic;
        private JobLogic jobLogic;
        private FactoryLogic factoryLogic;
        private PurchaseOrderStatusLogic purchaseOrderStatusLogic;
        private SeasonLogic seasonLogic;
        private CostsheetLogic costsheetLogic;
        private WorksheetLogic worksheetLogic;

        public PurchaseOrderController(PurchaseOrderLogic purchaseOrderLogic, StyleLogic styleLogic, JobLogic jobLogic,
            SeasonLogic seasonLogic, FactoryLogic factoryLogic, PurchaseOrderStatusLogic purchaseOrderStatusLogic, CostsheetLogic costsheetLogic, WorksheetLogic worksheetLogic)
        {
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.styleLogic = styleLogic;
            this.jobLogic = jobLogic;
            this.factoryLogic = factoryLogic;
            this.purchaseOrderStatusLogic = purchaseOrderStatusLogic;
            this.seasonLogic = seasonLogic;
            this.costsheetLogic = costsheetLogic;
            this.worksheetLogic = worksheetLogic;
        }

        // GET: Orders/PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }
     
        public JsonResult GetJobDropDown()
        {
            try
            {
                return Json(jobLogic.GetJobDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
  
        public JsonResult GetStyleDropDown()
        {
            try
            {
                return Json(styleLogic.GetStyleDropDown(), JsonRequestBehavior.AllowGet);
            }            
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFactoryDropDown()
        {
            try
            {
                return Json(factoryLogic.GetFactoryDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSeasonDropDown()
        {
            try
            {
                return Json(seasonLogic.GetSeasonDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
  
        public JsonResult GetCurrentStatusDropDown()
        {
            try
            {
                return Json(purchaseOrderStatusLogic.GetProductionStatusDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPurchaseOrderListByStyle(int styleID)
        {
            try
            {
                return Json(purchaseOrderLogic.GetAllPurchaseOrderByStyleID(styleID), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreatePurchaseOrder(PurchaseOrderViewModel purchaseOrderVM) {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model State is not valid!");
            }

            if (!purchaseOrderLogic.IsUniquePurchaseOrder(purchaseOrderVM.PurchaseOrderNo.Trim(), purchaseOrderVM.StyleID))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Duplicate Purchase Order!");
            }

            try
            {
                purchaseOrderLogic.CreatePurchaseOrder(purchaseOrderVM);
                Response.StatusCode = (int)HttpStatusCode.Created;
                return Json("Created Successfully!");
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult UpdatePurchaseOrder(PurchaseOrderViewModel purchaseOrderVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model State is not valid!");
            }

            if(purchaseOrderLogic.IsUniquePurchaseOrder(purchaseOrderVM.PurchaseOrderNo.Trim(), purchaseOrderVM.StyleID))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("You Can't Change Purchase Order No!");
            }

            try
            {
                purchaseOrderLogic.UpdatePurchaseOrder(purchaseOrderVM);
                return Json("Updated Successfully!");
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetCSByStyleID(int styleID)
        {
            if(styleID < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please select a style!", JsonRequestBehavior.AllowGet);
            }

            try
            {
                return Json(costsheetLogic.GetCostSheetDropDown(styleID), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCostSheetByCostsheetNo(string costsheetNo)
        {
            if (string.IsNullOrEmpty(costsheetNo))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Cost Sheet No is Invalid!", JsonRequestBehavior.AllowGet);
            }

            try
            {
                return Json(costsheetLogic.GetCostSheetByCostsheetNo(costsheetNo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        #region Bill Of Materials

        public JsonResult GetOrCreateWorkSheet(string costSheetNo, int purchaseOrderID)
        {
            if(string.IsNullOrEmpty(costSheetNo) || purchaseOrderID < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Cost Sheet No or Purchase Order No is Invalid!", JsonRequestBehavior.AllowGet);
            }

            try
            {
                return Json(worksheetLogic.GetWorksheetsCreateIfEmpty(costSheetNo, purchaseOrderID), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPut]
        public JsonResult UpdateWorkSheets(List<WorksheetViewModel> worksheetsList)
        {
            if(!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Model State!");
            }
            try
            {
                worksheetLogic.UpdateWorksheet(worksheetsList);
                return Json("Work Sheets Updated Successfully!");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }            
        }

        public JsonResult GetPurchaseOrdersForBulkEdit(int jobId)
        {
            if(jobId == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please Select a Job!", JsonRequestBehavior.AllowGet);
            }

            try
            {                
                return Json(purchaseOrderLogic.GetAllPurchaseOrderByJobId(jobId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult SaveAmendment(List<PurchaseOrderViewModel> bulkPurchaseOrderVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model State is invalid!");
            }

            try
            {
                //save amendment
                purchaseOrderLogic.UpdatePurchaseOrder(bulkPurchaseOrderVM);
                return Json("Amendment Successful!");
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public JsonResult GetWorkSheetsByCostSheetAndPurchaseOrderId(string costSheetNo, int purchaseOrderId)
        {
            if (string.IsNullOrEmpty(costSheetNo) || purchaseOrderId < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Cost Sheet No or Purchase Order No is Invalid!", JsonRequestBehavior.AllowGet);
            }

            try
            {
                var workSheets = worksheetLogic.TryToGetWorksheets(costSheetNo, purchaseOrderId);
                if(workSheets.Count == 0)
                {
                    //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                return Json(workSheets, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Split

        public ActionResult Split()
        {            
            return View();
        }

        //[HttpPost]
        //public JsonResult Split(SplitViewModel splitVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errList = ModelState.Values
        //                        .SelectMany(x => x.Errors)
        //                        .Select(x => x.ErrorMessage).ToList();
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json(errList);
        //    }
        //    try
        //    {
        //       // purchaseOrderLogic.Split(splitVM);
        //        return Json("Successfully saved!");
        //    }
        //    catch(Exception ex)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
        //        return Json(ex.Message);
        //    }

        //}
        [HttpGet]
        public JsonResult SplitPurchaseOrder(int masterPOID, int poID)
        {
            try
            {
                // purchaseOrderLogic.Split(splitVM);
                return Json("Successfully saved!",JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }

        }


        public JsonResult GetPurchaseOrderDropDown(int styleId)
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderDropDown(styleId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseOrderForSplit(int styleId, int purchaseOrderId)
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderForSplit(styleId, purchaseOrderId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseOrderById(int purchaseOrderId)
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderByID(purchaseOrderId), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}