using ScopoERP.OrderManagement.BLL;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.OrderManagement.Controllers
{
    public class SampleApprovalController : Controller
    {
        private StyleLogic styleLogic;

        public SampleApprovalController(StyleLogic styleLogic)
        {
            this.styleLogic = styleLogic;
        }

        public ActionResult Index()
        {
            ViewBag.styleList = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");

            return View();
        }

        public JsonResult Create()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}