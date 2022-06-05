using ScopoERP.Common.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Production.BLL;
using ScopoERP.Store.BLL;
using ScopoERP.Store.ViewModel;
using ScopoERP.Web.Helper;
using ScopoERP.Web.Reports;
using ScopoERP.WebUI.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace ScopoERP.Web.Areas.Store.Controllers
{
    [Authorize(Roles = "Shipment")]
    public class ChalanController : Controller
    {
        private ChalanLogic chalanLogic;
        private ShipmentLogic shipmentLogic;
        private StyleLogic styleLogic;
        private ProductionFloorLogic productionFloorLogic;
        private PurchaseOrderLogic purchaseOrderLogic;

        public ChalanController(
            ChalanLogic _chalanLogic, 
            ShipmentLogic _shipmentLogic, 
            ProductionFloorLogic _productionFloorLogic,
            StyleLogic _styleLogic, 
            PurchaseOrderLogic _purchaseOrderLogic)
        {
            this.chalanLogic          = _chalanLogic;
            this.shipmentLogic        = _shipmentLogic;
            this.productionFloorLogic = _productionFloorLogic;
            this.styleLogic           = _styleLogic;
            this.purchaseOrderLogic   = _purchaseOrderLogic;
        }

        public ActionResult Index(string searchString)
        {
            List<ChalanViewModel> data;

            if (String.IsNullOrEmpty(searchString))
            {
                data = chalanLogic.GetAllChalan()
                        .Take(10)
                        .OrderByDescending(x => x.ChalanID)
                        .ToList();
            }
            else
            {
                data = chalanLogic.GetAllChalan()
                        .Where(x => x.ChalanNo.ToLower().Contains(searchString))
                        .OrderByDescending(x => x.ChalanID)
                        .ToList();
            }

            return View(data);
        }

        public JsonResult GetPurchaseOrderByStyle(int styleID)
        {
            var poList = purchaseOrderLogic.GetPurchaseOrderDropDown(styleID);

            return Json(poList, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult GetShipmentByPurchaseOrder(int purchaseOrderID)
        {
            var purchaseOrderDetails = shipmentLogic.GetPurchaseOrderByID(purchaseOrderID);

            return Json(purchaseOrderDetails, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Create()
        {
            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");

            return View();
        }

        
        [HttpPost]
        public ActionResult Create(ChalanViewModel chalanVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    chalanVM.UserID = CurrentUser.UserID;
                    chalanVM.SetupDate = DateTime.Now;

                    ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");

                    if (!chalanLogic.IsUniqueChalan(chalanVM.ChalanNo)) {
                        ModelState.AddModelError("", @"Chalan No is not unique. Creation failed.");
                        return View();
                    }

                    var response=chalanLogic.CreateChalan(chalanVM);
                    if (!response) {
                        ModelState.AddModelError("", @"Shipment list is null. Creation failed.");
                        return View();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ModelState.AddModelError("", @"Modelstate invalid. Creation failed.");

            return View(chalanVM);
        }

        
        public ActionResult Edit(int id)
        {
            ChalanViewModel chalanVM = chalanLogic.GetChalanByID(id);
            var shipmentList = shipmentLogic.GetAllShipmentByChalan(id);
            chalanVM.ShipmentList = shipmentList;

            if (chalanVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.FloorList = new SelectList(productionFloorLogic.GetFloorDropDown(), "ValueString", "Text");

            return View(chalanVM);
        }

        
        [HttpPost]
        public ActionResult Edit(ChalanViewModel chalanVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    chalanVM.UserID = CurrentUser.UserID;
                    chalanVM.SetupDate = DateTime.Now;

                    chalanLogic.UpdateChalan(chalanVM);

                    return RedirectToAction("Index");
                }
                catch (DataException ex)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");

            return View(chalanVM);
        }
    }
}