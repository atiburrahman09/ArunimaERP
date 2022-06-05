using ScopoERP.Common.BLL;
using ScopoERP.OrderManagement.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private DashboardLogic dashboardLogic;

        public HomeController(DashboardLogic dashboardLogic)
        {
            this.dashboardLogic = dashboardLogic;
        }

        // GET: Home        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTotalOfOrderInvoicePI()
        {
            try
            {
                return Json(dashboardLogic.GetTotalOfOrderInvoicePI(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

 
        public JsonResult GetSampleCountByApSentDate()
        {
            try
            {
                return Json((dashboardLogic.GetSampleCountByApSentDate()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPICountByPIDate()
        {
            try
            {
                return Json((dashboardLogic.GetPICountByPIDate()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetShipmentDateTotalOrder()
        {
            try
            {
                return Json(dashboardLogic.GetShipmentDateTotalOrder(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetShipmentDates()
        {
            try
            {
                return Json(dashboardLogic.GetShipmentDates(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetDashboardData()
        {
            try
            {
                return Json(dashboardLogic.GetDashboardData(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetShipmentPerDayDataSet()
        {
            var data = dashboardLogic.GetShipmentPerDayDataSet();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


    }
}