using ScopoERP.Production.BLL;
using ScopoERP.ProductionStatus.BLL;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Common.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private ProductionFloorLogic productionFloorLogic;
        private SupervisorLogic supervisorLogic;
        private StandardOperationLogic standardOperationLogic;
        private JobClassLogic jobClassLogic;
        private SpecLogic specLogic;

        public SettingsController(ProductionFloorLogic productionFloorLogic, SupervisorLogic supervisorLogic, StandardOperationLogic standardOperationLogic, JobClassLogic jobClassLogic, SpecLogic specLogic)
        {
            this.productionFloorLogic = productionFloorLogic;
            this.supervisorLogic = supervisorLogic;
            this.standardOperationLogic = standardOperationLogic;
            this.jobClassLogic = jobClassLogic;
            this.specLogic = specLogic;
        }
        // GET: Common/Settings
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetFloor()
        {
            try
            {
                return Json(productionFloorLogic.GetFloorDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLine(string Floor)
        {
            try
            {
                return Json(productionFloorLogic.GetLineDropDownByFloor(Floor), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SaveSupervisor(SupervisorViewModel supervisorViewModel)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if(supervisorViewModel.SupervisorID > 0)
                {
                    if (!supervisorLogic.IsUnique(supervisorViewModel))
                    {
                        supervisorLogic.UpdateSupervisor(supervisorViewModel);
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(new { Message = "Supervisor Updated Created" });

                    }
                    return Json(new { Message = "Supervisor already exists."});
                }
                else
                {
                    if (!supervisorLogic.IsUnique(supervisorViewModel))
                    {
                        supervisorLogic.CreateSupervisor(supervisorViewModel);
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(new { Message = "Supervisor Successfully Created" });

                    }
                    return Json(new { Message = "Supervisor already exists."});
                }

               

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetAllSupervisors()
        {
            try
            {
                return Json(supervisorLogic.GetAllSupervisors(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveOperation(StandardOperationViewModel stdOperationVM)
        {
            if (ModelState.IsValid)
            {
                if (standardOperationLogic.IsUniqueOperation(stdOperationVM))
                {
                    stdOperationVM = standardOperationLogic.SaveStandardOperation(stdOperationVM);
                    return Json(new{ ErrorCode = 0,Message = "Operation Successfully Saved!!!" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { ErrorCode = 1, Message = "Duplicate Entry!!!" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { ErrorCode = 2, Message = "Model is Not Valid!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllStandardOperations()
        {
            return Json(standardOperationLogic.GetAllStandardOperations(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStandardOperationDropDown()
        {
            return Json(standardOperationLogic.GetStandardOperationDropDown(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllOperationCategories()
        {
            return Json(standardOperationLogic.GetAllOperationCategories(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllJobClassList()
        {
            try
            {
                return Json(jobClassLogic.GetAllJobClassList(), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult SaveSpec(SpecViewModel specVM)
        {
            if (ModelState.IsValid)
            {
                if (specLogic.IsUniqueSpec(specVM))
                {
                    specLogic.SaveSpec(specVM);
                    return Json(true, JsonRequestBehavior.AllowGet);
                   
                }
                else
                {
                    return Json(new { ErrorCode = 1, Message = "Duplicate Entry!!!!" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { ErrorCode = 2, Message = "Model is Not Valid!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllSpecs()
        {
            return Json(specLogic.GetAllSpecs(), JsonRequestBehavior.AllowGet);
        }
    }
}