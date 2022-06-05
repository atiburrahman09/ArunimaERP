using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.BLL;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Repositories;
using ScopoERP.LC.BLL;
using ScopoERP.LC.ViewModel;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.Web.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace ScopoERP.Web.Areas.Commercial.Controllers
{
    [Authorize(Roles = "Commercial")]    
    public class BackToBackLCController : Controller
    {
        private BackToBackLCLogic backToBackLCLogic;
        private JobLogic jobLogic;
        private LCTypeLogic lcTypeLogic;
        private PILogic piLogic;
        private AdvancedCMLogic advancedCMLogic;

        public BackToBackLCController(
            BackToBackLCLogic backToBackLCLogic,
            JobLogic jobLogic,
            LCTypeLogic lcTypeLogic,
            PILogic piLogic,
            AdvancedCMLogic advancedCMLogic)
        {
            this.backToBackLCLogic = backToBackLCLogic;
            this.jobLogic = jobLogic;
            this.lcTypeLogic = lcTypeLogic;
            this.piLogic = piLogic;
            this.advancedCMLogic = advancedCMLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OldIndex()
        {
            return View();
        }

        public JsonResult GetJobDropDown()
        {
            List<DropDownListViewModel> jobList = jobLogic.GetJobDropDown();
            return Json(jobList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBackToBackLCByJob(int jobID)
        {
            List<BackToBackLCViewModel> b2bLCList = backToBackLCLogic.GetBackToBackLCByJob(jobID);  
            
            foreach(var item in b2bLCList)
            {
                item.PIList = backToBackLCLogic.GetPISummaryByLCID(item.BackToBackLCID);

                foreach(var pi in item.PIList)
                {
                    pi.PIValue = piLogic.GetPIValueByID((int)pi.PIID);
                }
            } 
                     
            return Json(b2bLCList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLCTypeDropDown()
        {
            List<DropDownListViewModel> lcType = lcTypeLogic.GetLCTypeDropDown();
            return Json(lcType, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBackToBackLC(BackToBackLCViewModel backToBacklcVM)
        {
            if (ModelState.IsValid)
            {

                if (backToBacklcVM.BackToBackLCID != 0)
                {
                    backToBackLCLogic.UpdateBackToBackLC(backToBacklcVM);
                }
                else {
                    backToBackLCLogic.CreateBackToBackLC(backToBacklcVM);
                }

                return Json(true);
            }
            return Json(false);
        }

        public JsonResult GetPISummaryByJobID(int jobID)
        {
            var results = backToBackLCLogic.GetPISummaryByJobID(jobID);
            foreach (var pi in results)
            {
                pi.PIValue = piLogic.GetPIValueByID((int)pi.PIID);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdatePIByLCID(BackToBackLCViewModel b2bLCVM)
        {    
            if(ModelState.IsValid)
            {
                backToBackLCLogic.UpdatePIByLCID(b2bLCVM.BackToBackLCID, b2bLCVM.PIList);
                return Json(true);
            }
            return Json(false);
        }

        [HttpDelete]
        public JsonResult DeleteBackToBackLC(int lcID)
        {
            
            if (backToBackLCLogic.DeleteBackToBackLC(lcID))
            {               
                return Json(true);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;            
            return Json(false);
        }

        public JsonResult updateBackToBackLC(BackToBackLCViewModel b2bLCVM)
        {
            if (ModelState.IsValid)
            {
                backToBackLCLogic.UpdateBackToBackLC(b2bLCVM);
                return Json(true);
            }
            return Json(false);
        }

        public JsonResult createBackToBackLC(BackToBackLCViewModel b2bLCVM)
        {
            if (ModelState.IsValid)
            {
                backToBackLCLogic.CreateBackToBackLC(b2bLCVM);
                return Json(true);
            }
            return Json(false);
        }

    }
}