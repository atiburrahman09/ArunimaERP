using ScopoERP.Common.BLL;
using ScopoERP.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Common.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SeasonController : Controller
    {
        private SeasonLogic seasonLogic;

        public SeasonController(SeasonLogic seasonLogic)
        {
            this.seasonLogic = seasonLogic;
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
        public ViewResult GetAllSeason()
        {
            List<SeasonViewModel> seasonList = seasonLogic.GetAllSeason();

            return View(new GridModel(seasonList));
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
        /// <param name="seasonVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SeasonViewModel seasonVM)
        {
            if (ModelState.IsValid)
            {
                if (!seasonLogic.IsUniqueSeason(seasonVM.SeasonName.Trim()))
                {
                    ModelState.AddModelError("", seasonVM.SeasonName + " already exists");
                }
                else
                {
                    try
                    {
                        seasonLogic.CreateSeason(seasonVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(seasonVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            SeasonViewModel seasonVM = seasonLogic.GetSeasonById(id);

            if (seasonVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(seasonVM);
        }


        [HttpPost]
        public ActionResult Edit(SeasonViewModel seasonVM)
        {
            if (ModelState.IsValid)
            {

                if (!seasonLogic.IsUniqueSeason(seasonVM.SeasonName.Trim(), seasonVM.SeasonId))
                {
                    ModelState.AddModelError("", @"This Buyer No is already exists");
                }
                else
                {
                    try
                    {
                        seasonLogic.UpdateSeason(seasonVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }

            }
            return View(seasonVM);
        }

    }
}