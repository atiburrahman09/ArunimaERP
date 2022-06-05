using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.LC.BLL;
using ScopoERP.LC.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.LC.Controllers
{
    //[Authorize(Roles = "merchant")]
    public class JobController : Controller
    {
        private JobLogic jobLogic;
        private BankLogic bankLogic;

        public JobController(JobLogic jobLogic, BankLogic bankLogic)
        {
            this.jobLogic = jobLogic;
            this.bankLogic = bankLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ViewResult GetAllJob()
        {
            List<JobViewModel> jobVM = jobLogic.GetAllJob();

            return View(new GridModel(jobVM));
        }

        public ActionResult Create()
        {
            ViewBag.BankList = new SelectList(bankLogic.GetBankDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(JobViewModel jobVM)
        {
            if (ModelState.IsValid)
            {
                if (!jobLogic.IsUniqueJob(jobVM.JobNo.Trim()))
                {
                    ModelState.AddModelError("", jobVM.JobNo + " already exists");
                }
                else
                {
                    try
                    {
                        jobLogic.CreateJob(jobVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            ViewBag.BankList = new SelectList(bankLogic.GetBankDropDown(), "Value", "Text", jobVM.BankID);
            return View(jobVM);
        }

        public ActionResult Edit(int id)
        {
            JobViewModel jobVM = jobLogic.GetJobByID(id);

            if (jobVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.BankList = new SelectList(bankLogic.GetBankDropDown(), "Value", "Text", jobVM.BankID);
            return View(jobVM);
        }

        [HttpPost]
        public ActionResult Edit(JobViewModel jobVM)
        {
            if (ModelState.IsValid)
            {
                if (!jobLogic.IsUniqueJob(jobVM.JobNo.Trim(), jobVM.JobId))
                {
                    ModelState.AddModelError("", @"This Job No is already exists");
                }
                else
                {
                    try
                    {
                        jobLogic.UpdateJob(jobVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }
            }
            ViewBag.BankList = new SelectList(bankLogic.GetBankDropDown(), "Value", "Text", jobVM.BankID);
            return View(jobVM);
        }

        public ActionResult UpdateAdvancedCM()
        {
            return View();
        }

        public JsonResult GetJobList(int year)
        {
            return Json(jobLogic.GetAllJobForAdvancedCM(year), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAdvancedCM(List<JobViewModel> jobList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    jobLogic.UpdateAdvancedCM(jobList);

                    return Json(true);
                }
                catch(DataException ex)
                {
                    return Json(false);
                }
            }
            return Json(false);
        }
    }
}