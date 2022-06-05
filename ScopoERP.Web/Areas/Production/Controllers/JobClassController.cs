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
    public class JobClassController : Controller
    {
        private JobClassLogic jobClassLogic;

        public JobClassController(JobClassLogic jobClassLogic)
        {
            this.jobClassLogic = jobClassLogic;
        }
        
        // GET: Production/JobClass
        public ActionResult Index()
        {
            return View();
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
        public JsonResult SaveJobClass(JobClassViewModel jobClassVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (jobClassVM.JobClassID > 0)
                {
                    if (!jobClassLogic.IsUnique(jobClassVM))
                    {
                        jobClassLogic.Update(jobClassVM);
                        return Json(new { Message = "Job Class Successfully Updated" });
                    }
                    return Json(new { Message = "Job Class Already Exists" });

                }
                if (jobClassVM.JobClassID == 0)
                {
                    try
                    {
                        if (!jobClassLogic.IsUnique(jobClassVM))
                        {
                            jobClassLogic.Create(jobClassVM);
                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Message = "Job Class Successfully Created" });
                        }
                        return Json(new { Message = "Job Class Already Exists" });

                    }
                    catch (Exception ex)
                    {
                        Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                        return Json(ex.Message);
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    return Json("Data has been violated");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetJobClassDetailByID(int jobClassID)
        {
            try
            {
                return Json(jobClassLogic.GetJobClassDetailByID(jobClassID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}