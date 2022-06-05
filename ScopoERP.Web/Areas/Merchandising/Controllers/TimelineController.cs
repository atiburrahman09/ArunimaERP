using ScopoERP.Common.BLL;
using ScopoERP.Common.ViewModel;
using ScopoERP.OrderManagement.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Merchandising.Controllers
{
    public class TimelineController : Controller
    {
        private TimelineLogic timeLineLogic;
        private StyleLogic styleLogic;
        private PurchaseOrderLogic purchaseOrderLogic;

        public TimelineController(TimelineLogic timeLineLogic, PurchaseOrderLogic purchaseOrderLogic, StyleLogic styleLogic)
        {
            this.timeLineLogic = timeLineLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.styleLogic = styleLogic;
        }
        // GET: Common/Timeline
        public ActionResult Index()
        {
            return View();
        }

        

        public JsonResult GetAllTimelineByPOID(int purchaseOrderID)
        {
            try
            {
                return Json((timeLineLogic.GetAllTimelineByPOID(purchaseOrderID)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveTimeLine(TimelineViewModel timelineVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                timelineVM.ModifiedBy = User.Identity.Name;
                if (timelineVM.TimeLineID > 0)
                {                    
                    timeLineLogic.UpdateTimeline(timelineVM);
                    return Json("Timeline Updated successfully.");
                }

                else {
                    timeLineLogic.SaveTimeLine(timelineVM);
                    return Json("Timeline created successfully.");
                }
            

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RemoveTimeline(int timelineID)
        {
            try
            {
                timeLineLogic.RemoveTimeline(timelineID);
                return Json("Data successfully removed.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}