using ScopoERP.Common.ViewModel;
using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Stackholder.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.Web.Areas.Merchandising.Controllers
{
    [Authorize(Roles = "merchant")]
    public class RequisitionController : Controller
    {
        private JobLogic jobLogic;
        private RequisitionLogic requisitionLogic;
        private PILogic piLogic;
        private SupplierLogic supplierLogic;

        public RequisitionController(
            RequisitionLogic requisitionLogic, JobLogic jobLogic, SupplierLogic supplierLogic, PILogic piLogic)
        {
            this.requisitionLogic = requisitionLogic;
            this.jobLogic = jobLogic;
            this.piLogic = piLogic;
            this.supplierLogic = supplierLogic;
        }

        // GET: MaterialManagement/Requisition/
        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetAllRequisition()
        {
            var data = requisitionLogic.GetAllRequisition();

            return View(new GridModel(data));
        }

        // GET: MaterialManagement/Requisition/Create
        public ActionResult Create()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            ViewBag.SupplierList = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public JsonResult Create(RequisitionViewModel requisitionVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    string requisitionNo = requisitionLogic.CreateRequisition(requisitionVM, 1, 1);

                    TempData["requisitionNo"] = requisitionNo;

                    return Json("Requisition create successfull.");
                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError("", ex.InnerException.StackTrace);
                    return Json(ex.InnerException.StackTrace);
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", requisitionVM.JobID);
            ViewBag.SupplierList = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", requisitionVM.SupplierID);

            return Json("ok");
        }

        public ActionResult Edit(int id)
        {
            RequisitionViewModel requisitionVM = requisitionLogic.GetRequisitionByID(id);

            if (requisitionVM == null)
            {
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", requisitionVM.JobID);
            ViewBag.SupplierList = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", requisitionVM.SupplierID);

            return View(requisitionVM);
        }

        [HttpPost]
        public ActionResult Edit(RequisitionViewModel requisitionVM) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    requisitionLogic.UpdateRequisition(requisitionVM);

                    return Json("Requisition updated successfull.");
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError("", ex.InnerException.StackTrace);
                    return Json(ex.InnerException.StackTrace);
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", requisitionVM.JobID);

            return Json("ok");
        }

        public ActionResult GetPIDropDownByJob(int jobID)
        {
            var results = piLogic.GetPIDropDownByJob(jobID);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPIValueByPIID(int piID)
        {
            var results = piLogic.GetPIValueByID(piID);

            return Json(new { piValue = results }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLastRequisitionDate(int jobID, int supplierID)
        {
            return Json(requisitionLogic.GetLastRequisitionDate(jobID, supplierID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJobDropDown()
        {
            List<DropDownListViewModel> jobList = jobLogic.GetJobDropDown();
            return Json(jobList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSupplierDropDown()
        {
            List<SupplierViewModel> supplierList = supplierLogic.GetAllSupplier();
            return Json(supplierList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReqByJob(int jobID)
        {
            List<RequisitionViewModel> reqList = requisitionLogic.GetRequisitionByJobID(jobID);

            foreach (var item in reqList)
            {
                item.PIList = requisitionLogic.GetPISummaryByReqID(item.RequisitionID);

                foreach (var pi in item.PIList)
                {
                    pi.PIValue = piLogic.GetPIValueByID((int)pi.PIID);
                }
            }

            return Json(reqList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPISummaryByJobID(int jobID)
        {
            var results = requisitionLogic.GetPISummaryByJobID(jobID);
            foreach (var pi in results)
            {
                pi.PIValue = piLogic.GetPIValueByID((int)pi.PIID);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}