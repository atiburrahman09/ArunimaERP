using ScopoERP.Common.BLL;
using ScopoERP.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Common.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DivisionController : Controller
    {
        private DivisionLogic divisionLogic;

        public DivisionController(DivisionLogic divisionLogic)
        {
            this.divisionLogic = divisionLogic;
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
        public ViewResult GetAllDivision()
        {
            List<DivisionViewModel> divisionVM = divisionLogic.GetAllDivision();

            return View(new GridModel(divisionVM));
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
        /// <param name="divisionVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(DivisionViewModel divisionVM)
        {
            if (ModelState.IsValid)
            {
                if (!divisionLogic.IsUniqueDivision(divisionVM.DivisionName.Trim()))
                {
                    ModelState.AddModelError("", divisionVM.DivisionName + " already exists");
                }
                else
                {
                    try
                    {
                        divisionLogic.CreateDivision(divisionVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(divisionVM);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            DivisionViewModel divisionVM = divisionLogic.GetDivisionById(id);

            if (divisionVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(divisionVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="divisionVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(DivisionViewModel divisionVM)
        {
            if (ModelState.IsValid)
            {

                if (!divisionLogic.IsUniqueDivision(divisionVM.DivisionName.Trim(), divisionVM.DivisionID))
                {
                    ModelState.AddModelError("", @"This Buyer No is already exists");
                }
                else
                {
                    try
                    {
                        divisionLogic.UpdateDivision(divisionVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }

            }
            return View(divisionVM);
        }

    }
}