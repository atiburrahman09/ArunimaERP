using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Store.BLL;
using ScopoERP.Store.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Store.Controllers
{
    public class InventoryReceiveController : Controller
    {
        private InventoryReceiveLogic inventroryReceiveLogic;
        private ItemLogic itemLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private StyleLogic styleLogic;

        public InventoryReceiveController(InventoryReceiveLogic inventroryReceiveLogic, PurchaseOrderLogic purchaseOrderLogic, StyleLogic styleLogic, ItemLogic itemLogic)
        {
            this.inventroryReceiveLogic = inventroryReceiveLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.styleLogic = styleLogic;
            this.itemLogic = itemLogic;
        }

        // GET: Store/InventoryReceive
        public ActionResult Index()
        {
            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.Chalan = new SelectList(inventroryReceiveLogic.GetImportChalanDropDown(), "Value", "Text");

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ImportChalanViewModel importChalanVM)
        {
            if (ModelState.IsValid)
            {
                inventroryReceiveLogic.CreateImportChalan(importChalanVM);

                return RedirectToAction("Index");
            }
            return View(importChalanVM);
        }

        public JsonResult GetPurchaseOrderByStyle(int styleID)
        {
            var purchaseOrderList = purchaseOrderLogic.GetPurchaseOrderDropDown(styleID);

            return Json(purchaseOrderList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemByPurchaseOrder(int purchaseOrderID)
        {
            var itemList = itemLogic.GetItemDropDownByPurchaseOrder(purchaseOrderID);

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChalanDetails(int blID, int purchaseOrderID, int itemID)
        {
            var chalanDetails = inventroryReceiveLogic.GetImportChalanDetails(blID, purchaseOrderID, itemID);

            return PartialView("GetBLDetails", chalanDetails);
        }

        public ActionResult SaveChalanDetails(List<InventoryReceiveViewModel> importChalanVMList)
        {
            try
            {
                if (importChalanVMList[0].BLDetailsID != 0)
                {
                    inventroryReceiveLogic.UpdateImportChalanDetails(importChalanVMList);
                }
                else
                {
                    inventroryReceiveLogic.CreateImportChalanDetails(importChalanVMList);
                }
                return Json(true);
            }
            catch (Exception ex)
            {

            }
            return Json(false);
        }
    }
}