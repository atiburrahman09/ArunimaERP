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
    public class SampleController : Controller
    {
        // GET: Merchandising/Sample
        public ActionResult Index()
        {
            return View();
        }
        private SampleApprovalLogic _sampleApprovalLogic;
        private StyleLogic _styleLogic;
        public SampleController(
            SampleApprovalLogic sampleApprovalLogic,
            StyleLogic styleLogic
            )
        {
            this._sampleApprovalLogic = sampleApprovalLogic;
            this._styleLogic = styleLogic;

        }

        public JsonResult GetAllStyle()
        {
            try
            {
                return Json((_styleLogic.GetAllStyle(1)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllSample()
        {
            try
            {
                return Json((_sampleApprovalLogic.GetAllSample()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveApprove(SampleApprovalViewModel sampleApproveVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errs = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToArray();
                    
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (sampleApproveVM.SampleApprovalID > 0)
                {
                    sampleApproveVM.UserID = User.Identity.Name;
                    _sampleApprovalLogic.UpdateApprove(sampleApproveVM);
                    return Json("Sample approve updated successfully.");
                }
                else
                {
                    sampleApproveVM.UserID = User.Identity.Name;
                    _sampleApprovalLogic.SaveApprove(sampleApproveVM);
                    return Json("Sample approve created successfully.");
                }

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAllSampleApprove(int StyleID)
        {
            try
            {
                return Json((_sampleApprovalLogic.GetAllSampleApprove(StyleID)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RemoveSampleApprove(int SampleApprovalID)
        {
            try
            {
                _sampleApprovalLogic.RemoveSampleApprove(SampleApprovalID);
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