using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.BLL;
using ScopoERP.Domain.Repositories;
using ScopoERP.LC.BLL;
using ScopoERP.LC.ViewModel;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.WebUI.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace ScopoERP.WebUI.Areas.Commercial.Controllers
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

        [GridAction]
        public ActionResult GetAllBackToBackLC()
        {
            List<BackToBackLCViewModel> data = backToBackLCLogic.GetAllBackToBackLC().ToList();

            return View(new GridModel(data));
        }

        public ActionResult BackToBackLCExport(string column, string orderBy, string filter)
        {
            var results = backToBackLCLogic.GetAllBackToBackLC();
            List<BackToBackLCViewModel> data = results.ToGridModel(0, 0, orderBy, string.Empty, filter)
                                                        .Data.Cast<BackToBackLCViewModel>().ToList();

            var output = ReportHelper.ConvertToCSV<BackToBackLCViewModel>(column, data);

            return File(output, "text/comma-separated-values", "BackToBackLC.csv");
        }

        public ActionResult GetPIDropDownByJob(int jobID)
        {
            var results = piLogic.GetPIDropDownByJob(jobID);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPIValueByPIID(int piID)
        {
            decimal? piValue;

            piValue = piLogic.GetPIValueByID(piID);

            if (piValue == null || piValue == 0)
            {
                piValue = advancedCMLogic.GetPIValueFromAdvancedCM(piID);
            }

            return Json(new { piValue = piValue }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            ViewBag.LCType = new SelectList(lcTypeLogic.GetLCTypeDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(BackToBackLCViewModel backToBackLCVM)
        {
            if (ModelState.IsValid)
            {
                if (!backToBackLCLogic.IsUniqueBackToBackLC(backToBackLCVM.BackToBackLCNo.Trim()))
                {
                    ModelState.AddModelError("", backToBackLCVM.BackToBackLCNo + " already exists");
                }
                else
                {
                    try
                    {
                        backToBackLCLogic.CreateBackToBackLC(backToBackLCVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", backToBackLCVM.JobID);
            ViewBag.LCType = new SelectList(lcTypeLogic.GetLCTypeDropDown(), "Value", "Text", backToBackLCVM.LCTypeID);

            return View(backToBackLCVM);
        }

        public ActionResult Edit(int id)
        {
            BackToBackLCViewModel backToBackLCVM = backToBackLCLogic.GetBackToBackLCByID(id);

            if (backToBackLCVM == null)
            {
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", backToBackLCVM.JobID);
            ViewBag.LCType = new SelectList(lcTypeLogic.GetLCTypeDropDown(), "Value", "Text", backToBackLCVM.LCTypeID);

            return View(backToBackLCVM);
        }

        [HttpPost]
        public ActionResult Edit(BackToBackLCViewModel backToBackLCVM)
        {
            if (ModelState.IsValid)
            {
                if (!backToBackLCLogic.IsUniqueBackToBackLC(backToBackLCVM.BackToBackLCNo.Trim(), backToBackLCVM.BackToBackLCID))
                {
                    ModelState.AddModelError("", @"This BackToBackLC No is already exists");
                }
                else
                {
                    try
                    {
                        backToBackLCLogic.UpdateBackToBackLC(backToBackLCVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", backToBackLCVM.JobID);
            ViewBag.LCType = new SelectList(lcTypeLogic.GetLCTypeDropDown(), "Value", "Text", backToBackLCVM.LCTypeID);

            return View(backToBackLCVM);
        }
    }
}