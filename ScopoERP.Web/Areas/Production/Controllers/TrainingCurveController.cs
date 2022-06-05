using ScopoERP.ProductionStatus.BLL;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    [Authorize]
    public class TrainingCurveController : Controller
    {
        private TrainingCurveLogic trainingCurveLogic;
        private EmployeeCapabilityService empCapabilityService;
        private StandardOperationLogic standardOperationLogic;


        public TrainingCurveController(TrainingCurveLogic trainingCurveLogic, EmployeeCapabilityService empCapabilityService, StandardOperationLogic standardOperationLogic)
        {
            this.trainingCurveLogic = trainingCurveLogic;
            this.empCapabilityService = empCapabilityService;
            this.standardOperationLogic = standardOperationLogic;
        }

        // GET: Production/TrainingCurve
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveTrainingCurve(TrainingCurveViewModel trainingCurveVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (trainingCurveVM.TrainingCurveID > 0)
                {
                    trainingCurveLogic.UpdateTrainingCurve(trainingCurveVM);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Message = "Training curve updated." });
                }
                else
                {
                    if (trainingCurveLogic.IsStageLeeserTwo(trainingCurveVM))
                    {
                        trainingCurveLogic.CreateTrainingCurve(trainingCurveVM);
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(new { Message = "Training curve created." });
                    }
                    else
                    {
                        //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new {ErrorCode= false, Message = "Two Training curve for this stage already created." });
                    }
                   
                }



            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        public JsonResult GetAllTrainingCurve()
        {
            try
            {
                return Json(trainingCurveLogic.GetAllTrainingCurve(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        //Training Curve Assign To Employee

        public ActionResult EmployeeRate()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllOperations()
        {
            return Json(standardOperationLogic.GetAllStandardOperations(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeeRateDetailsById(string CardNo)
        {
            try
            {
                EmployeeRateViewModel empInfo = trainingCurveLogic.GetEmployeeeRateDetailsById(CardNo);
                return Json(empInfo, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult SaveInformation(EmployeeRateViewModel empRateViewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!", JsonRequestBehavior.AllowGet);
            }
            try
            {

                trainingCurveLogic.SaveEmployeeRateInfo(empRateViewModel);
                return Json("Successfully saved!", JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}