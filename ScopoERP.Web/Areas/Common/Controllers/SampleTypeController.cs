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
    public class SampleTypeController : Controller
    {
        private SampleTypeLogic sampleTypeLogic;

        public SampleTypeController(SampleTypeLogic sampleTypeLogic)
        {
            this.sampleTypeLogic = sampleTypeLogic;
        }
        // GET: Common/SampleType
        public ActionResult Index()
        {
            List<SampleTypeViewModel> data = sampleTypeLogic.GetAllSampleType();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SampleTypeViewModel sampleVM)
        {
            if (ModelState.IsValid)
            {
                if (!sampleTypeLogic.IsUniqueSampleType(sampleVM.SampleTypeName.Trim(),sampleVM.SampleTypeID))
                {
                    ModelState.AddModelError("", sampleVM.SampleTypeName + " already exists");
                }
                else
                {
                    try
                    {
                        sampleTypeLogic.CreateSampleType(sampleVM,User.Identity.Name);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(sampleVM);
        }

        public ActionResult Edit(int id)
        {
            SampleTypeViewModel sampleVM = sampleTypeLogic.GetSampleTypeById(id);

            if (sampleVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(sampleVM);
        }

        [HttpPost]
        public ActionResult Edit(SampleTypeViewModel sampleVM)
        {
            if (ModelState.IsValid)
            {

                if (!sampleTypeLogic.IsUniqueSampleType(sampleVM.SampleTypeName.Trim(), sampleVM.SampleTypeID))
                {
                    ModelState.AddModelError("", @"This Buyer No is already exists");
                }
                else
                {
                    try
                    {
                        sampleTypeLogic.CreateSampleType(sampleVM,User.Identity.Name);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }

            }
            return View(sampleVM);
        }




    }
}