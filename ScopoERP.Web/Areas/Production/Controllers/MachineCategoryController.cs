using ScopoERP.LC.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using ScopoERP.Production.BLL;
using ScopoERP.Production.ViewModel;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    [Authorize(Roles = "Production, Admin, Dpr-Entry")]

    public class MachineCategoryController : Controller
    {
        private MachineCategoryLogic machineCategoryLogic;

        public MachineCategoryController(MachineCategoryLogic machineCategoryLogic)
        {
            this.machineCategoryLogic = machineCategoryLogic;
        }

        public ActionResult Index()
        {
            List<MachineCategoryViewModel> data = machineCategoryLogic.GetAllMachineCategory();
            return View(data.ToList());
        }

      
        public ActionResult GetAllMachineCategory()
        {
            List<MachineCategoryViewModel> data = machineCategoryLogic.GetAllMachineCategory();

            return View(new GridModel(data));
        }

        public ActionResult Create()
        {
            ViewBag.ParentCategory = new SelectList(machineCategoryLogic.GetMachineCategoryDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(MachineCategoryViewModel machineCategoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    machineCategoryLogic.CreateMachineCategory(machineCategoryVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.ParentCategory = new SelectList(machineCategoryLogic.GetMachineCategoryDropDown(), "Value", "Text", machineCategoryVM.ParentCategoryID);
            return View(machineCategoryVM);
        }

        public ActionResult Edit(int id)
        {
            MachineCategoryViewModel machineCategoryVM = machineCategoryLogic.GetMachineCategoryByID(id);

            if (machineCategoryVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.ParentCategory = new SelectList(machineCategoryLogic.GetMachineCategoryDropDown(), "Value", "Text", machineCategoryVM.ParentCategoryID);
            return View(machineCategoryVM);
        }

        [HttpPost]
        public ActionResult Edit(MachineCategoryViewModel machineCategoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    machineCategoryLogic.UpdateMachineCategory(machineCategoryVM);

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }
            ViewBag.ParentCategory = new SelectList(machineCategoryLogic.GetMachineCategoryDropDown(), "Value", "Text", machineCategoryVM.ParentCategoryID);
            return View(machineCategoryVM);
        }
    }
}