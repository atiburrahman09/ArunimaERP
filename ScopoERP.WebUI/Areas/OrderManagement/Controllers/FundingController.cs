using ScopoERP.LC.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using ScopoERP.Stackholder.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.OrderManagement.Controllers
{
    [Authorize(Roles = "merchant")]
    public class FundingController : Controller
    {
        private JobLogic jobLogic;
        private SupplierLogic supplierLogic;
        private FundingLogic fundingLogic;

        public FundingController(JobLogic jobLogic, SupplierLogic supplierLogic, FundingLogic fundingLogic)
        {
            this.jobLogic = jobLogic;
            this.supplierLogic = supplierLogic;
            this.fundingLogic = fundingLogic;
        }

        // GET: OrderManagement/Funding
        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetAllFunding()
        {
            List<FundingViewModel> data = fundingLogic.GetAllFunding();

            return View(new GridModel(data));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text");

            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(FundingViewModel fundingVM)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    fundingLogic.Create(fundingVM);

                    return Json(true);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                    the problem persists, Contact with Entitas Technologia.");
                }
                
            }

            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", fundingVM.SupplierID);
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", fundingVM.JobID);

            return PartialView(fundingVM);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            FundingViewModel fundingVM = fundingLogic.GetFundingByID(id);

            if (fundingVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", fundingVM.SupplierID);
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", fundingVM.JobID);

            return PartialView(fundingVM);
        }

        [HttpPost]
        public ActionResult Edit(FundingViewModel fundingVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    fundingLogic.Update(fundingVM);

                    return Json(true);
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                    the problem persists, Contact with Entitas Technologia.");
                }
            }

            ViewBag.Supplier = new SelectList(supplierLogic.GetSupplierDropDown(), "Value", "Text", fundingVM.SupplierID);
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", fundingVM.JobID);

            return PartialView(fundingVM);
        }
    }
}