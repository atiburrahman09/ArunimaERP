using ScopoERP.Common.BLL;
using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
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
    [Authorize(Roles = "merchant")]
    public class PurchaseOrderController : Controller
    {
        private PurchaseOrderLogic purchaseOrderLogic;
        private StyleLogic styleLogic;
        private JobLogic jobLogic;
        private FactoryLogic factoryLogic;
        private PurchaseOrderStatusLogic purchaseOrderStatusLogic;
        private SeasonLogic seasonLogic;
        private CostsheetLogic costsheetLogic;

        public PurchaseOrderController
        (
            PurchaseOrderLogic purchaseOrderLogic,
            StyleLogic styleLogic,
            JobLogic jobLogic,
            SeasonLogic seasonLogic,
            FactoryLogic factoryLogic,
            PurchaseOrderStatusLogic purchaseOrderStatusLogic,
            CostsheetLogic costsheetLogic
        )
        {
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.styleLogic = styleLogic;
            this.jobLogic = jobLogic;
            this.factoryLogic = factoryLogic;
            this.purchaseOrderStatusLogic = purchaseOrderStatusLogic;
            this.seasonLogic = seasonLogic;
            this.costsheetLogic = costsheetLogic;
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
        public ActionResult GetAllPurchaseOrder()
        {
            List<PurchaseOrderViewModel> data = purchaseOrderLogic.GetAllPurchaseOrder(CurrentUser.AccountID);

            return View(new GridModel(data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");
            ViewBag.Factory = new SelectList(factoryLogic.GetFactoryDropDown(), "Value", "Text");
            ViewBag.Season = new SelectList(seasonLogic.GetSeasonDropDown(), "Value", "Text");
            ViewBag.PurchaseOrderStatus = new SelectList(purchaseOrderStatusLogic.GetProductionStatusDropDown(), "Value", "Text");

            return PartialView();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseOrderVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(PurchaseOrderViewModel purchaseOrderVM)
        {
            if (ModelState.IsValid)
            {
                if (!purchaseOrderLogic.IsUniquePurchaseOrder(purchaseOrderVM.PurchaseOrderNo.Trim()))
                {
                    ModelState.AddModelError("", purchaseOrderVM.PurchaseOrderNo + " already exists");
                }
                else
                {
                    try
                    {
                        purchaseOrderLogic.CreatePurchaseOrder(purchaseOrderVM);

                        return Json(true);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text", purchaseOrderVM.StyleID);
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", purchaseOrderVM.JobID);
            ViewBag.Factory = new SelectList(factoryLogic.GetFactoryDropDown(), "Value", "Text", purchaseOrderVM.FactoryID);
            ViewBag.Season = new SelectList(seasonLogic.GetSeasonDropDown(), "Value", "Text", purchaseOrderVM.SeasonID);
            ViewBag.PurchaseOrderStatus = new SelectList(purchaseOrderStatusLogic.GetProductionStatusDropDown(), "Value", "Text", purchaseOrderVM.CurrentStatus);

            return PartialView(purchaseOrderVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Copy(int id)
        {
            PurchaseOrderViewModel purchaseOrderVM = purchaseOrderLogic.GetPurchaseOrderByID(id);

            if (purchaseOrderVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text", purchaseOrderVM.StyleID);
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", purchaseOrderVM.JobID);
            ViewBag.Factory = new SelectList(factoryLogic.GetFactoryDropDown(), "Value", "Text", purchaseOrderVM.FactoryID);
            ViewBag.Season = new SelectList(seasonLogic.GetSeasonDropDown(), "Value", "Text", purchaseOrderVM.SeasonID);
            ViewBag.PurchaseOrderStatus = new SelectList(purchaseOrderStatusLogic.GetProductionStatusDropDown(), "Value", "Text", purchaseOrderVM.CurrentStatus);

            return PartialView("Create", purchaseOrderVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            PurchaseOrderViewModel purchaseOrderVM = purchaseOrderLogic.GetPurchaseOrderByID(id);

            if (purchaseOrderVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text", purchaseOrderVM.StyleID);
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", purchaseOrderVM.JobID);
            ViewBag.Factory = new SelectList(factoryLogic.GetFactoryDropDown(), "Value", "Text", purchaseOrderVM.FactoryID);
            ViewBag.Season = new SelectList(seasonLogic.GetSeasonDropDown(), "Value", "Text", purchaseOrderVM.SeasonID);
            ViewBag.PurchaseOrderStatus = new SelectList(purchaseOrderStatusLogic.GetProductionStatusDropDown(), "Value", "Text", purchaseOrderVM.CurrentStatus);
            ViewBag.CostSheetList = new SelectList(costsheetLogic.GetCostSheetDropDown(purchaseOrderVM.StyleID), "ValueString", "Text", purchaseOrderVM.CostSheetNo);

            return PartialView(purchaseOrderVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseOrderVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(PurchaseOrderViewModel purchaseOrderVM)
        {
            if (ModelState.IsValid)
            {
                if (!purchaseOrderLogic.IsUniquePurchaseOrder(purchaseOrderVM.PurchaseOrderNo.Trim(), purchaseOrderVM.PurchaseOrderID))
                {
                    ModelState.AddModelError("", @"This PurchaseOrder No is already exists");
                }
                else
                {
                    try
                    {
                        purchaseOrderLogic.UpdatePurchaseOrder(purchaseOrderVM);

                        return Json(true);
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text", purchaseOrderVM.StyleID);
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", purchaseOrderVM.JobID);
            ViewBag.Factory = new SelectList(factoryLogic.GetFactoryDropDown(), "Value", "Text", purchaseOrderVM.FactoryID);
            ViewBag.Season = new SelectList(seasonLogic.GetSeasonDropDown(), "Value", "Text", purchaseOrderVM.SeasonID);
            ViewBag.PurchaseOrderStatus = new SelectList(purchaseOrderStatusLogic.GetProductionStatusDropDown(), "Value", "Text", purchaseOrderVM.CurrentStatus);
            ViewBag.CostSheetList = new SelectList(costsheetLogic.GetCostSheetDropDown(purchaseOrderVM.StyleID), "ValueString", "Text", purchaseOrderVM.CostSheetNo);

            return PartialView(purchaseOrderVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Split()
        {
            ViewBag.styleList = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleID"></param>
        /// <returns></returns>
        public ActionResult GetPurchaseOrderList(int styleID, int purchaseOrderID)
        {
            SplitViewModel splitVM = new SplitViewModel();

            splitVM.SplitList = purchaseOrderLogic.GetPurchaseOrderForSplit(styleID, purchaseOrderID);

            return View("~/Areas/OrderManagement/Views/PurchaseOrder/_SplitList.cshtml", splitVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseOrderID"></param>
        /// <returns></returns>
        public JsonResult GetPurchaseOrder(int purchaseOrderID)
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderByID(purchaseOrderID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleID"></param>
        /// <returns></returns>
        public JsonResult GetPurchaseOrderDropDown(int styleID)
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderDropDown(styleID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="splitVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Split(SplitViewModel splitVM)
        {
            purchaseOrderLogic.Split(splitVM);

            ViewBag.styleList = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            
            return RedirectToAction("Split");
        }
    }
}