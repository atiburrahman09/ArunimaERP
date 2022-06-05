using ScopoERP.ProductionStatus.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    [Authorize(Roles = "Production")]
    public class ProductionReportController : Controller
    {

        private JsonRequestBehavior Allow = JsonRequestBehavior.AllowGet;
        private JsonRequestBehavior Deny = JsonRequestBehavior.DenyGet;
        ProductionReportLogic productionReportLogic;
        public ProductionReportController(ProductionReportLogic productionReportLogic)
        {
            this.productionReportLogic = productionReportLogic;
        }
        // GET: Production/ProductionReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DailyStatus()
        {
            return View();
        }
        ///Production/ProductionReport/GetDailySewingStatus
        [HttpGet]
        public JsonResult GetDailySewingStatus()
        {
            var data = productionReportLogic.GetSewingReport();
            return Json(data,Allow);
        }
        
    }
}