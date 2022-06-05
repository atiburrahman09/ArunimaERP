using ScopoERP.ProductionStatus.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    public class EmployeeCapabilityController : Controller
    {
        private EmployeeCapabilityService empCapabilityService;
        private SpecLogic specLogic;

        public EmployeeCapabilityController(EmployeeCapabilityService empCapabilityService, SpecLogic specLogic)
        {
            this.empCapabilityService = empCapabilityService;
            this.specLogic = specLogic;
        }

        // GET: Production/EmployeeCapability
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetEmployeeeCapabilityDetailsById(string CardNo)
        {
            try
            {
                List<string> specList = empCapabilityService.GetEmployeeeCapabilityDetailsById(CardNo);
                return Json(specList, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmployeeDropDownByKeyword(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please enter employee name or card no.", JsonRequestBehavior.AllowGet);
            }

            try
            {
                return Json(empCapabilityService.GetEmployeeDropDownByKeyword(inputString), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRecentEmployees()
        {
            try
            {
                return Json(empCapabilityService.GetRecentEmployees(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        
        public JsonResult SaveInformation(string cardNo, List<string> specs)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!", JsonRequestBehavior.AllowGet);
            }
            try
            {

                empCapabilityService.SaveEmployeeCapabilityInfo(cardNo, specs);
                return Json("Successfully created!", JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllSpecs()
        {
            try
            {
                return Json(specLogic.GetSpec(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
    }
}