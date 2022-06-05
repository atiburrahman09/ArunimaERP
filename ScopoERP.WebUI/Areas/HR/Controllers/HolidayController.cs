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

namespace ScopoERP.WebUI.Areas.HR.Controllers
{
    [Authorize(Roles = "HR")]
    public class HolidayController : Controller
    {
        private HolidayLogic holidayLogic;

        public HolidayController(HolidayLogic holidayLogic)
        {
            this.holidayLogic = holidayLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllHoliday(int pageSize, int skip)
        {
            List<HolidayViewModel> data = holidayLogic.GetAllHoliday(pageSize, skip);

            int total = holidayLogic.GetTotalHoliday();

            return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HolidayViewModel holidayVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    holidayVM.SetupDate = DateTime.Now;
                    holidayVM.UserID = 10;
                    holidayLogic.CreateHoliday(holidayVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(holidayVM);
        }

        public ActionResult Edit(int id)
        {
            HolidayViewModel holidayVM = holidayLogic.GetHolidayByID(id);

            if (holidayVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(holidayVM);
        }

        [HttpPost]
        public ActionResult Edit(HolidayViewModel holidayVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    holidayVM.SetupDate = DateTime.Now;
                    holidayVM.UserID = 10;
                    holidayLogic.UpdateHoliday(holidayVM);

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                    the problem persists, Contact with Entitas Technologia.");
                }
            }
            return View(holidayVM);
        }
    }
}