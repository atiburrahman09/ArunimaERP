using ScopoERP.Commercial.BankForwardingL;
using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.LC.BLL;
using ScopoERP.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.Web.Areas.Commercial.Controllers
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

        public JsonResult GetAllBankForwardingByJobID(int jobID)
        {            
            var data = bankForwardingLogic.GetAllBankForwarding(jobID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJobDropDown()
        {
            var data = jobLogic.GetJobDropDown();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoiceDropDownByJob(int jobID)
        {
            var jobList = exportInvoiceLogic.GetInvoiceSummaryListByJob(jobID);

            return Json(jobList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveBankForwarding(BankForwardingViewModel bankForwardingVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    string bangforwardingNo=bankForwardingLogic.SaveBankForwarding(bankForwardingVM, CurrentUser.UserID);
                    return Json(bangforwardingNo);
                }
                catch
                {
                    
                }

            }
            return Json(false);
        }

        public JsonResult GetInvoiceListByBankForwardingID(int bankForwardingID)
        {
            var data = exportInvoiceLogic.GetInvoiceListByBankForwardingID(bankForwardingID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}