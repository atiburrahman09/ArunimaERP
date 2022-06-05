using ScopoERP.Common.BLL;
using ScopoERP.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Common.Controllers
{
    [Authorize(Roles = "Admin")]
   
    public class DivisionController : Controller
    {
        private DivisionLogic divisionLogic;

        public DivisionController(DivisionLogic divisionLogic)
        {
            this.divisionLogic = divisionLogic;
        }

        public ActionResult Index()
        {
            List<DivisionViewModel> data = divisionLogic.GetAllDivision();

            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DivisionViewModel divisionVM)
        {
            if (ModelState.IsValid)
            {
                if (!divisionLogic.IsUniqueDivision(divisionVM.DivisionName.Trim()))
                {
                    ModelState.AddModelError("", divisionVM.DivisionName + " already exists");
                }
                else
                {
                    try
                    {
                        divisionLogic.CreateDivision(divisionVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(divisionVM);
        }

        public ActionResult Edit(int id)
        {
            DivisionViewModel divisionVM = divisionLogic.GetDivisionById(id);

            if (divisionVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(divisionVM);
        }

        [HttpPost]
        public ActionResult Edit(DivisionViewModel divisionVM)
        {
            if (ModelState.IsValid)
            {

                if (!divisionLogic.IsUniqueDivision(divisionVM.DivisionName.Trim(), divisionVM.DivisionID))
                {
                    ModelState.AddModelError("", @"This Buyer No is already exists");
                }
                else
                {
                    try
                    {
                        divisionLogic.UpdateDivision(divisionVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }

            }
            return View(divisionVM);
        }
    }
}