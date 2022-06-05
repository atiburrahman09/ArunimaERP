using ScopoERP.Domain.Repositories;
using ScopoERP.LC.BLL;
using ScopoERP.LC.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Stackholder.ViewModel;
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
    public class BuyerController : Controller
    {
        private BuyerLogic buyerLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerLogic"></param>
        public BuyerController(BuyerLogic buyerLogic)
        {
            this.buyerLogic = buyerLogic;
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
        public ViewResult GetAllBuyer()
        {
            List<BuyerViewModel> buyerVM = buyerLogic.GetAllBuyer();

            return View(new GridModel(buyerVM));
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
        /// <param name="buyerVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(BuyerViewModel buyerVM)
        {
            if (ModelState.IsValid)
            {
                if (!buyerLogic.IsUniqueBuyer(buyerVM.BuyerName.Trim()))
                {
                    ModelState.AddModelError("", buyerVM.BuyerName + " already exists");
                }
                else
                {
                    try
                    {
                        buyerLogic.CreateBuyer(buyerVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return View(buyerVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            BuyerViewModel buyerVM = buyerLogic.GetBuyerByID(id);

            if (buyerVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(buyerVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(BuyerViewModel buyerVM)
        {
            if (ModelState.IsValid)
            {

                if (!buyerLogic.IsUniqueBuyer(buyerVM.BuyerName.Trim(), buyerVM.BuyerID))
                {
                    ModelState.AddModelError("", @"This Buyer No is already exists");
                }
                else
                {
                    try
                    {
                        buyerLogic.UpdateBuyer(buyerVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }

            }
            return View(buyerVM);
        }
    }
}