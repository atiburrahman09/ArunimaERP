using ScopoERP.Commercial.BankForwardingL;
using ScopoERP.Finance.BLL;
using ScopoERP.Finance.ViewModel;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Finance.Controllers
{
    public class RealizationController : Controller
    {
        private RealizationLogic realizationLogic;
        private RealizationAccountLogic realizationAccountLogic;
        private BankForwardingLogic bankForwardingLogic;

        public RealizationController(RealizationLogic realizationLogic, RealizationAccountLogic realizationAccountLogic, BankForwardingLogic bankForwardingLogic)
        {
            this.realizationLogic = realizationLogic;
            this.realizationAccountLogic = realizationAccountLogic;
            this.bankForwardingLogic = bankForwardingLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ViewResult GetAllRealizations()
        {
            List<RealizationViewModel> realizationList = realizationLogic.GetAllRealization();

            return View(new GridModel(realizationList));
        }

        public ActionResult Details()
        {
            return View();
        }

        public JsonResult Save(List<RealizationViewModel> realizationList)
        {
            if(ModelState.IsValid)
            {
                realizationList[0].UserID = CurrentUser.UserID;
                realizationLogic.SaveRealization(realizationList);
                return Json(true);
            }
            return Json(false);
        }

        public JsonResult GetAllFDBPNo()
        {
            return Json(bankForwardingLogic.GetFDBPDropDown(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllAccountType()
        {
            return Json(realizationAccountLogic.GetRealizationAccountTypeDropDown(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllRealization(int bankForwardingID, int accountType)
        {
            var result = realizationLogic.GetAllRealization(bankForwardingID, accountType);

            if (result.Count != 0)
            {
                result[0].InvoiceValue = bankForwardingLogic.GetTotalInvoiceValue(bankForwardingID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}