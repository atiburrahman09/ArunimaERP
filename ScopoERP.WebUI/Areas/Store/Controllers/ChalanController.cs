using ScopoERP.Common.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Production.BLL;
using ScopoERP.Store.BLL;
using ScopoERP.Store.ViewModel;
using ScopoERP.WebUI.Helper;
using ScopoERP.WebUI.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace ScopoERP.WebUI.Areas.Store.Controllers
{
    public class ChalanController : Controller
    {
        private ChalanLogic chalanLogic;
        private ShipmentLogic shipmentLogic;
        private StyleLogic styleLogic;
        private ProductionFloorLogic productionFloorLogic;
        private PurchaseOrderLogic purchaseOrderLogic;

        public ChalanController(ChalanLogic chalanLogic, ShipmentLogic shipmentLogic, ProductionFloorLogic productionFloorLogic,
                                    StyleLogic styleLogic, PurchaseOrderLogic purchaseOrderLogic)
        {
            this.chalanLogic          = chalanLogic;
            this.shipmentLogic        = shipmentLogic;
            this.productionFloorLogic = productionFloorLogic;
            this.styleLogic           = styleLogic;
            this.purchaseOrderLogic   = purchaseOrderLogic;
        }

        
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult ChalanExport(string filter)
        {
            IQueryable<ChalanViewModel> chalanList = chalanLogic.GetAllChalan();
            var data = chalanList.ToGridModel(0, 0, string.Empty, string.Empty, filter).Data.Cast<ChalanViewModel>().ToList();

            var output = ReportHelper.ConvertToCSV<ChalanViewModel>(string.Empty, data);

            return File(output, "text/comma-separated-values", "Chalan.csv");
        }

        
        public ActionResult ShipmentExport(string filter)
        {
            IQueryable<ShipmentViewModel> shipmentList = shipmentLogic.GetAllShipment();
            var data = shipmentList.ToGridModel(0, 0, string.Empty, string.Empty, filter).Data.Cast<ShipmentViewModel>().ToList();

            var output = ReportHelper.ConvertToCSV<ShipmentViewModel>(string.Empty, data);

            return File(output, "text/comma-separated-values", "Shipment.csv");
        }

        
        [GridAction]
        public ActionResult GetAllChalan()
        {
            List<ChalanViewModel> data = chalanLogic.GetAllChalan().ToList();

            return View(new GridModel(data));
        }

        
        [GridAction]
        public ActionResult GetAllShipment()
        {
            List<ShipmentViewModel> data = shipmentLogic.GetAllShipment().ToList();

            return View(new GridModel(data));
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

                    chalanLogic.CreateChalan(chalanVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");

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