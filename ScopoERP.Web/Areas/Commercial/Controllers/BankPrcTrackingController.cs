using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.LC.BLL;
using ScopoERP.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Commercial.Controllers
{
    public class BankPrcTrackingController : Controller
    {
        private BankPrcTrackingLogic bankPrcLogic;
        private JobLogic jobLogic;
        private ExportInvoiceLogic exportInvoiceLogic;

        public BankPrcTrackingController(BankPrcTrackingLogic bankPrcLogic, JobLogic jobLogic, ExportInvoiceLogic exportInvoiceLogic)
        {
            this.bankPrcLogic = bankPrcLogic;
            this.jobLogic = jobLogic;
            this.exportInvoiceLogic = exportInvoiceLogic;
        }
        // GET: Commercial/BankPrcTracking
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllBankPrcTrackingByJobID()
        {
            var data = bankPrcLogic.GetAllBankPRC();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJobDropDown()
        {
            var data = jobLogic.GetJobDropDown();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoices(string exp)
        {
            var invoices = exportInvoiceLogic.GetAllExportInvoiceForBankPRC(exp.Trim());

            return Json(invoices, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveBankPRC(BankPrcTrackingViewModel bankPRCVM)
        {
            if (!ModelState.IsValid)
            {
                var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                    
                return Json(false);
            }
            try
            {
                if (bankPRCVM.Id != 0)
                {
                    bankPRCVM.UpdatedBy = User.Identity.Name;
                    bankPRCVM.UpdatedOn = DateTime.Now;
                    return Json(bankPrcLogic.UpdateBankPRC(bankPRCVM));
                }
                else
                {
                    bankPRCVM.CreatedBy = User.Identity.Name;
                    bankPRCVM.CreatedOn = DateTime.Now;
                    return Json(bankPrcLogic.CreateBankPRC(bankPRCVM));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Json(false);
            }

        }

        public JsonResult GetInvoiceListByBankPrcTrackingID(int bankPrcTrackingID)
        {
            var data = bankPrcLogic.GetInvoiceListByBankPrcTrackingID(bankPrcTrackingID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}