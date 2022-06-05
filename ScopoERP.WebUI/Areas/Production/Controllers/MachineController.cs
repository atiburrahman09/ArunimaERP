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

namespace ScopoERP.WebUI.Areas.Production.Controllers
{
    [Authorize(Roles = "Production, Admin, Dpr-Entry")]
    public class MachineController : Controller
    {
        private MachineLogic machineLogic;
        private MachineCategoryLogic machineCategoryLogic;

        public MachineController(MachineLogic machineLogic, MachineCategoryLogic machineCategoryLogic)
        {
            this.machineLogic = machineLogic;
            this.machineCategoryLogic = machineCategoryLogic;
        }


        public ActionResult Index()
        {
            return View();
        }


        [GridAction]
        public ActionResult GetAllMachine()
        {
            List<MachineViewModel> data = machineLogic.GetAllMachine();

            return View(new GridModel(data));
        }


        public ActionResult Create()
        {
            ViewBag.MachineCategory = new SelectList(machineCategoryLogic.GetMachineCategoryDropDown(), "Value", "Text");

            return View();
        }


        [HttpPost]
        public ActionResult Create(MachineViewModel machineVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    machineLogic.CreateMachine(machineVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.MachineCategory = new SelectList(machineCategoryLogic.GetMachineCategoryDropDown(), "Value", "Text", machineVM.MachineCategoryID);

            return View(machineVM);
        }


        public ActionResult Edit(int id)
        {
            MachineViewModel machineVM = machineLogic.GetMachineByID(id);

            if (machineVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.MachineCategory = new SelectList(machineCategoryLogic.GetMachineCategoryDropDown(), "Value", "Text", machineVM.MachineCategoryID);

            return View(machineVM);
        }


        [HttpPost]
        public ActionResult Edit(MachineViewModel machineVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    machineLogic.UpdateMachine(machineVM);

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }

            ViewBag.MachineCategory = new SelectList(machineCategoryLogic.GetMachineCategoryDropDown(), "Value", "Text", machineVM.MachineCategoryID);

            return View(machineVM);
        }
    }
}