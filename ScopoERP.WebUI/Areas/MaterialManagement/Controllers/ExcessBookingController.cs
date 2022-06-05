using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.MaterialManagement.Controllers
{
    [Authorize(Roles = "Merchant")]
    public class ExcessBookingController : Controller
    {
        private ExcessBookingLogic excessBookingLogic;
        private JobLogic jobLogic;
        private ItemLogic itemLogic;

        public ExcessBookingController(ExcessBookingLogic excessBookingLogic, JobLogic jobLogic, ItemLogic itemLogic)
        {
            this.excessBookingLogic = excessBookingLogic;
            this.jobLogic = jobLogic;
            this.itemLogic = itemLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetAllExcessBooking()
        {
            List<ExcessBookingViewModel> data = excessBookingLogic.GetAllExcessBooking();

            return View(new GridModel(data));
        }

        public ActionResult Create()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            ViewBag.Item = new SelectList(itemLogic.GetItemDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(ExcessBookingViewModel excessBookingVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    excessBookingLogic.CreateExcessBooking(excessBookingVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", excessBookingVM.JobID);
            ViewBag.Item = new SelectList(itemLogic.GetItemDropDown(), "Value", "Text", excessBookingVM.ItemID);

            return View(excessBookingVM);
        }

        public ActionResult Edit(int id)
        {
            ExcessBookingViewModel excessBookingVM = excessBookingLogic.GetExcessBookingByID(id);

            if (excessBookingVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", excessBookingVM.JobID);
            ViewBag.Item = new SelectList(itemLogic.GetItemDropDown(), "Value", "Text", excessBookingVM.ItemID);

            return View(excessBookingVM);
        }

        [HttpPost]
        public ActionResult Edit(ExcessBookingViewModel excessBookingVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    excessBookingLogic.UpdateExcessBooking(excessBookingVM);

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", excessBookingVM.JobID);
            ViewBag.Item = new SelectList(itemLogic.GetItemDropDown(), "Value", "Text", excessBookingVM.ItemID);

            return View(excessBookingVM);
        }
    }
}