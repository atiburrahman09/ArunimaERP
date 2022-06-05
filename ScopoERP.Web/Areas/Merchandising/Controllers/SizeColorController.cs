using ScopoERP.Common.BLL;
using ScopoERP.LC.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.Web.Areas.Merchandising.Controllers
{
    [Authorize(Roles = "merchant")]
    public class SizeColorController : Controller
    {
        private SizeColorLogic sizeColorLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private StyleLogic styleLogic;

        public SizeColorController(SizeColorLogic sizeColorLogic, PurchaseOrderLogic purchaseOrderLogic, StyleLogic styleLogic)
        {
            this.sizeColorLogic = sizeColorLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.styleLogic = styleLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(List<SizeColorViewModel> sizeColorList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sizeColorLogic.CreateSizeColor(sizeColorList);

                    return Json(true);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return Json(false);
        }

        public ActionResult GetSizeColorByPurchaseOrder(int purchaseOrderID)
        {
            return Json(sizeColorLogic.GetSizeColorByPurchaseOrderID(purchaseOrderID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPODropDown() {
            return Json(purchaseOrderLogic.GetPurchaseOrderDropDown(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Copy(int fromPurchaseOrderID, int toPurchaseOrderID)
        {
            try
            {
                if (!sizeColorLogic.IsSizeColorExists(fromPurchaseOrderID))
                {
                    return Json(new { errorMessage = "Selected PO has no size color" }, JsonRequestBehavior.AllowGet);
                }

                if (sizeColorLogic.IsSizeColorExists(toPurchaseOrderID))
                {
                    return Json(new { errorMessage = "Size Color is already exists" }, JsonRequestBehavior.AllowGet);
                }

                sizeColorLogic.CopySizeColor(fromPurchaseOrderID, toPurchaseOrderID);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SizeWiseFOB() { 
            return View();
        }

        [HttpPost]
        public ActionResult CreateFOB(List<SizeColorViewModel> sizeColorList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Json(true);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return Json(false);
        }

        public ActionResult GetStyleDropDown()
        {
            return Json(styleLogic.GetStyleDropDown(1), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSinglePODropDownBySingleStyle(int styleID)
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderDropDown(styleID), JsonRequestBehavior.AllowGet);
        }
    }
}