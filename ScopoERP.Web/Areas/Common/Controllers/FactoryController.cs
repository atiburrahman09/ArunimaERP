using ScopoERP.Common.BLL;
using ScopoERP.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Common.Controllers
{
    [Authorize(Roles = "Admin")]
    
    public class FactoryController : Controller
    {
        private FactoryLogic factoryLogic;

        public FactoryController(FactoryLogic factoryLogic)
        {
            this.factoryLogic = factoryLogic;
        }

        public ActionResult Index()
        {
            List<FactoryViewModel> data = factoryLogic.GetAllFactory();

            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FactoryViewModel factoryVM)
        {
            if (ModelState.IsValid)
            {
                if (!factoryLogic.IsUniqueFactory(factoryVM.FactoryName.Trim()))
                {
                    ModelState.AddModelError("", factoryVM.FactoryName + " already exists");
                }
                else
                {
                    try
                    {
                        factoryLogic.CreateFactory(factoryVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(factoryVM);
        }

        public ActionResult Edit(int id)
        {
            FactoryViewModel factoryVM = factoryLogic.GetFactoryByID(id);

            if (factoryVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(factoryVM);
        }

        [HttpPost]
        public ActionResult Edit(FactoryViewModel factoryVM)
        {
            if (ModelState.IsValid)
            {

                if (!factoryLogic.IsUniqueFactory(factoryVM.FactoryName.Trim(), factoryVM.FactoryID))
                {
                    ModelState.AddModelError("", @"This Factory No is already exists");
                }
                else
                {
                    try
                    {
                        factoryLogic.UpdateFactory(factoryVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }

            }
            return View(factoryVM);
        }
    }
}