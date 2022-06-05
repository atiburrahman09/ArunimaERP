using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.MaterialManagement.Controllers
{
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
        public ActionResult Create(RequisitionViewModel requisitionVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    string requisitionNo = requisitionLogic.CreateRequisition(requisitionVM, CurrentUser.AccountID, CurrentUser.UserID);

                    TempData["requisitionNo"] = requisitionNo;

                    return RedirectToAction("Create");
                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError("", ex.InnerException.StackTrace);
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", requisitionVM.JobID);
            ViewBag.SupplierList = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", requisitionVM.SupplierID);

            return View();
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

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.StackTrace);
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", requisitionVM.JobID);

            return View();
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
    }
}