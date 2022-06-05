using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.LC.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using ScopoERP.Stackholder.BLL;
using ScopoERP.WebUI.Helper;

namespace ScopoERP.WebUI.Areas.MaterialManagement.Controllers
{
    //[Authorize(Roles = "Merchant")]
    public class PIController : Controller
    {
        private PILogic piLogic;
        private SupplierLogic supplierLogic;
        private JobLogic jobLogic;

        public PIController(PILogic piLogic, SupplierLogic supplierLogic, JobLogic jobLogic)
        {
            this.piLogic = piLogic;
            this.supplierLogic = supplierLogic;
            this.jobLogic = jobLogic;
        }

        public ActionResult Index()
        {
            return View();
        }
        
        [GridAction]
        public ActionResult GetAllPI()
        {
            List<PIViewModel> data = piLogic.GetAllPI(CurrentUser.AccountID);

            return View(new GridModel(data));
        }

        public ActionResult Edit(int id)
        {
            PIViewModel piVM = piLogic.GetPIByID(id);

            if (piVM == null)
            {
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", piVM.SupplierID);
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", piVM.LoanFromJobID);

            return View(piVM);
        }

        [HttpPost]
        public ActionResult Edit(PIViewModel piVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    piLogic.UpdatePI(piVM);

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }

            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", piVM.SupplierID);
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", piVM.LoanFromJobID);

            return View(piVM);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var piInfo = piLogic.GetPIByID(id);

            return View(piInfo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirned(int id)
        {
            try
            {
                piLogic.DeletePI(id);
            }
            catch (DataException)
            {
                ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");

                return RedirectToAction("Delete", new { id = id });
            }
            return RedirectToAction("Index");
        }
    }
}