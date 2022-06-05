using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.OrderManagement.Controllers
{
    public class SampleTypeController : Controller
    {
        private SampleTypeLogic sampleTypeLogic;

        public SampleTypeController(SampleTypeLogic sampleTypeLogic)
        {
            this.sampleTypeLogic = sampleTypeLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ViewResult GetAllSampleType()
        {
            List<SampleTypeViewModel> sampleTypeVM = sampleTypeLogic.GetAllSampleType();

            return View(new GridModel(sampleTypeVM));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SampleTypeViewModel sampleTypeVM)
        {
            if (ModelState.IsValid)
            {
                if (!sampleTypeLogic.IsUniqueSampleType(sampleTypeVM.SampleTypeName.Trim()))
                {
                    ModelState.AddModelError("", sampleTypeVM.SampleTypeName + " already exists");
                }
                else
                {
                    try
                    {
                        sampleTypeVM.UserID = CurrentUser.UserID;
                        sampleTypeVM.SetDate = DateTime.Now;
                        sampleTypeLogic.CreateSampleType(sampleTypeVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(sampleTypeVM);
        }

        public ActionResult Edit(int id)
        {
            SampleTypeViewModel sampleTypeVM = sampleTypeLogic.GetSampleTypeByID(id);

            if (sampleTypeVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(sampleTypeVM);
        }

        [HttpPost]
        public ActionResult Edit(SampleTypeViewModel sampleTypeVM)
        {
            if (ModelState.IsValid)
            {

                if (!sampleTypeLogic.IsUniqueSampleType(sampleTypeVM.SampleTypeName.Trim(), sampleTypeVM.SampleTypeID))
                {
                    ModelState.AddModelError("", sampleTypeVM.SampleTypeName + " already exists");
                }
                else
                {
                    try
                    {
                        sampleTypeVM.UserID = CurrentUser.UserID;
                        sampleTypeVM.SetDate = DateTime.Now;
                        sampleTypeLogic.UpdateSampleType(sampleTypeVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }

            }
            return View(sampleTypeVM);
        }
    }
}