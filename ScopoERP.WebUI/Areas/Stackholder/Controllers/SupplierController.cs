using ScopoERP.Stackholder.ViewModel;
using ScopoERP.Domain.Repositories;
using ScopoERP.LC.BLL;
using ScopoERP.LC.ViewModel;
using ScopoERP.Stackholder.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Stackholder.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        private SupplierLogic supplierLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierLogic"></param>
        public SupplierController(SupplierLogic supplierLogic)
        {
            this.supplierLogic = supplierLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ViewResult GetAllSupplier()
        {
            List<SupplierViewModel> supplierVM = supplierLogic.GetAllSupplier();

            return View(new GridModel(supplierVM));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SupplierViewModel supplierVM)
        {
            if (ModelState.IsValid)
            {
                if (!supplierLogic.IsUniqueSupplier(supplierVM.SupplierName.Trim()))
                {
                    ModelState.AddModelError("", supplierVM.SupplierName + " already exists");
                }
                else
                {
                    try
                    {
                        supplierLogic.CreateSupplier(supplierVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(supplierVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            SupplierViewModel supplierVM = supplierLogic.GetSupplierByID(id);

            if (supplierVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(supplierVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(SupplierViewModel supplierVM)
        {
            if (ModelState.IsValid)
            {

                if (!supplierLogic.IsUniqueSupplier(supplierVM.SupplierName.Trim(), supplierVM.SupplierID))
                {
                    ModelState.AddModelError("", @"This Supplier No is already exists");
                }
                else
                {
                    try
                    {
                        supplierLogic.UpdateSupplier(supplierVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }

            }
            return View(supplierVM);
        }
    }
}