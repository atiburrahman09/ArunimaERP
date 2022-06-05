using ScopoERP.Commercial.BankForwardingL;
using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.LC.BLL;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Commercial.Controllers
{
    [Authorize(Roles = "Commercial")]
    public class BankForwardingController : Controller
    {
        private JobLogic jobLogic;
        private BankForwardingLogic bankForwardingLogic;
        private ExportInvoiceLogic exportInvoiceLogic;

        public BankForwardingController(BankForwardingLogic bankForwardingLogic, JobLogic jobLogic, ExportInvoiceLogic exportInvoiceLogic)
        {
            this.bankForwardingLogic = bankForwardingLogic;
            this.jobLogic = jobLogic;
            this.exportInvoiceLogic = exportInvoiceLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetAllBankForwarding()
        {
            var data = bankForwardingLogic.GetAllBankForwarding();

            return View(new GridModel(data));
        }

        public ActionResult Create()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(BankForwardingViewModel bankForwardingVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string bankForwardingNo = bankForwardingLogic.CreateBankForwarding(bankForwardingVM, CurrentUser.UserID);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.StackTrace);
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", bankForwardingVM.JobID);

            return View();
        }

        public ActionResult Edit(int id)
        {
            BankForwardingViewModel bankForwardingVM = bankForwardingLogic.GetBankForwardingByID(id);

            if (bankForwardingVM == null)
            {
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", bankForwardingVM.JobID);

            return View(bankForwardingVM);
        }

        [HttpPost]
        public ActionResult Edit(BankForwardingViewModel bankForwardingVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bankForwardingLogic.UpdateBankForwarding(bankForwardingVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.StackTrace);
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", bankForwardingVM.JobID);

            return View();
        }

        public JsonResult GetInvoiceDropDownByJob(int jobID)
        {
            var jobList = exportInvoiceLogic.GetInvoiceDropDown(jobID);

            return Json(jobList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExportInvoiceByID(int invoiceID)
        {
            var invoice = exportInvoiceLogic.GetExportInvoiceByID(invoiceID);

            return Json(invoice, JsonRequestBehavior.AllowGet);
        }
    }
}