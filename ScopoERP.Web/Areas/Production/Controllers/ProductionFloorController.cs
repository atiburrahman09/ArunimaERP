using ScopoERP.Production.BLL;
using ScopoERP.Production.ViewModel;
using ScopoERP.ProductionStatus.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    [Authorize(Roles = "Admin, Dpr-Entry, Production")]

    public class ProductionFloorController : Controller
    {
        private ProductionFloorLogic productionFloorLogic;

        public ProductionFloorController(ProductionFloorLogic productionFloorLogic)
        {
            this.productionFloorLogic = productionFloorLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ViewResult GetAllProductionFloor()
        {
            List<ProductionFloorViewModel> productionFloorVM = productionFloorLogic.GetAllProductionFloor();

            return View(new GridModel(productionFloorVM));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductionFloorViewModel productionFloorVM)
        {
            if (ModelState.IsValid)
            {
                if (!productionFloorLogic.IsUniqueProductionFloor(productionFloorVM.Floor.Trim(), productionFloorVM.Line.Trim()))
                {
                    ModelState.AddModelError("", productionFloorVM.Floor + " -> " + productionFloorVM.Line + " already exists");
                }
                else
                {
                    try
                    {
                        productionFloorVM.Status = 1;
                        productionFloorVM.Division = "1";
                        productionFloorLogic.CreateProductionFloor(productionFloorVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(productionFloorVM);
        }

        public ActionResult Edit(int id)
        {
            ProductionFloorViewModel productionFloorVM = productionFloorLogic.GetProductionFloorByID(id);

            if (productionFloorVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(productionFloorVM);
        }

        [HttpPost]
        public ActionResult Edit(ProductionFloorViewModel productionFloorVM)
        {
            if (ModelState.IsValid)
            {

                if (!productionFloorLogic.IsUniqueProductionFloor(productionFloorVM.Floor.Trim(), productionFloorVM.Line, productionFloorVM.ProductionFloorID))
                {
                    ModelState.AddModelError("", productionFloorVM.Floor + " -> " + productionFloorVM.Line + " already exists");
                }
                else
                {
                    try
                    {
                        productionFloorVM.Status = 1;
                        productionFloorVM.Division = "1";
                        productionFloorLogic.UpdateProductionFloor(productionFloorVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }

            }
            return View(productionFloorVM);
        }
    }
}