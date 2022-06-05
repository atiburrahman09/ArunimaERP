using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Web.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace ScopoERP.Web.Areas.Misc.Controllers
{
    [Authorize(Roles = "AdvancedCM")]
    public class AdvancedCMController : Controller
    {
        private AdvancedCMLogic advancedCMLogic;
        private JobLogic jobLogic;
        private SupplierLogic supplierLogic;

        public AdvancedCMController(AdvancedCMLogic advancedCMLogic, JobLogic jobLogic, SupplierLogic supplierLogic)
        {
            this.advancedCMLogic = advancedCMLogic;
            this.jobLogic = jobLogic;
            this.supplierLogic = supplierLogic;
        }

        // GET: Misc/AdvanceCM
        public ActionResult Index()
        {
            List<AdvancedCMViewModel> advancedCMVM = advancedCMLogic.GetAllAdvancedCM().ToList();
            return View(advancedCMVM);
        }

     

        public ActionResult AdvancedCMExport(string column, string orderBy, string filter)
        {
            var results = advancedCMLogic.GetAllAdvancedCM();
            List<AdvancedCMViewModel> data = results.ToGridModel(0, 0, orderBy, string.Empty, filter)
                                                        .Data.Cast<AdvancedCMViewModel>().ToList();

            var output = ReportHelper.ConvertToCSV<AdvancedCMViewModel>(column, data);

            return File(output, "text/comma-separated-values", "AdvancedCM.csv");
        }

        public ActionResult Create()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(AdvancedCMViewModel advancedCMVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    advancedCMVM.UserID = 1;
                    advancedCMVM.SetupDate = DateTime.Now;

                    advancedCMLogic.CreateAdvancedCM(advancedCMVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", advancedCMVM.JobID);
            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", advancedCMVM.SupplierID);

            return View(advancedCMVM);
        }

        public ActionResult Edit(int id)
        {
            AdvancedCMViewModel advancedCMVM = advancedCMLogic.GetAdvancedCMByID(id);

            if (advancedCMVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", advancedCMVM.JobID);
            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", advancedCMVM.SupplierID);

            return View(advancedCMVM);
        }

        [HttpPost]
        public ActionResult Edit(AdvancedCMViewModel advancedCMVM)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    advancedCMVM.UserID = 1;
                    advancedCMVM.SetupDate = DateTime.Now;
                    advancedCMLogic.UpdateAdvancedCM(advancedCMVM);

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", advancedCMVM.JobID);
            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", advancedCMVM.SupplierID);

            return View(advancedCMVM);
        }
    }
}
