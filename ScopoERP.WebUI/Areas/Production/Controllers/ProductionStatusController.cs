using ScopoERP.OrderManagement.BLL;
using ScopoERP.Production.BLL;
using ScopoERP.Production.ViewModel;
using ScopoERP.WebUI.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace ScopoERP.WebUI.Areas.Production.Controllers
{
    [Authorize(Roles = "Dpr-Entry")]
    public class ProductionStatusController : Controller
    {
        private ProductionStatusLogic productionStatusLogic;
        private StyleLogic styleLogic;
        private ProductionFloorLogic productionFloorLogic;
        private PurchaseOrderLogic purchaseOrderLogic;

        public ProductionStatusController(ProductionStatusLogic productionStatusLogic, ProductionFloorLogic productionFloorLogic, 
                                            StyleLogic styleLogic, PurchaseOrderLogic purchaseOrderLogic)
        {
            this.productionStatusLogic = productionStatusLogic;
            this.styleLogic = styleLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.productionFloorLogic = productionFloorLogic;
        }

        public ActionResult Index()
        {
            return View();
        }


        [GridAction]
        public ActionResult GetAllProductionStatus(int page, int size, string filter,
                                                    string orderBy)
        {
            //if(filter == string.Empty || filter == null)
            //{
            //    filter = "Date~ge~datetime'"+ DateTime.Now.AddDays(-10) +"'~and~Date~le~datetime'"+ DateTime.Now +"'";
            //}

            var results = productionStatusLogic.GetAllProductionStatus()
                            .ToGridModel(0, 0, orderBy, string.Empty, filter).Data.Cast<ProductionStatusViewModel>()
                            .OrderByDescending(x => x.ProductionDailyReportID);

            return View(new GridModel(results));
        }

        public ActionResult Export(string column, string orderBy, string filter)
        {
            var results = productionStatusLogic.GetAllProductionStatus();
            List<ProductionStatusViewModel> data = results.ToGridModel(0, 0, orderBy, string.Empty, filter).Data.Cast<ProductionStatusViewModel>().ToList();

            var output = ReportHelper.ConvertToCSV<ProductionStatusViewModel>(column, data);

            return File(output, "text/comma-separated-values", "ProductionStatus.csv");
        }


        public JsonResult GetLineByFloor(string floor)
        {
            var lineList = productionFloorLogic.GetLineDropDownByFloor(floor);

            return Json(lineList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPurchaseOrderByStyle(int styleID)
        {
            var poList = purchaseOrderLogic.GetPurchaseOrderDropDown(styleID);

            return Json(poList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetOrderQuantityByPurchaseOrder(int purchaseOrderID)
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderByID(purchaseOrderID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTotalProductionStatusByPurchaseOrder(int purchaseOrderID)
        {
            var data = productionStatusLogic.GetTotalProductionStatusByPurchaseOrder(purchaseOrderID);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.Floors = new SelectList(productionFloorLogic.GetFloorDropDown(), "ValueString", "Text");

            return PartialView();
        }


        [HttpPost]
        public ActionResult Create(ProductionStatusViewModel productionStatusVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    productionStatusLogic.CreateProductionStatus(productionStatusVM);

                    return Json(true);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text", productionStatusVM.StyleID);
            ViewBag.PurchaseOrder = new SelectList(purchaseOrderLogic.GetPurchaseOrderDropDown(productionStatusVM.StyleID), "Value", "Text", productionStatusVM.PurchaseOrderID);
            ViewBag.Floors = new SelectList(productionFloorLogic.GetFloorDropDown(), "ValueString", "Text", productionStatusVM.Floor);
            ViewBag.Lines = new SelectList(productionFloorLogic.GetLineDropDownByFloor(productionStatusVM.Floor), "ValueString", "Text", productionStatusVM.Line);

            return PartialView(productionStatusVM);
        }


        public ActionResult Edit(int id)
        {
            ProductionStatusViewModel productionStatusVM = productionStatusLogic.GetProductionStatusByID(id);

            if (productionStatusVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text", productionStatusVM.StyleID);
            ViewBag.PurchaseOrder = new SelectList(purchaseOrderLogic.GetPurchaseOrderDropDown(productionStatusVM.StyleID), "Value", "Text", productionStatusVM.PurchaseOrderID);
            ViewBag.Floors = new SelectList(productionFloorLogic.GetFloorDropDown(), "ValueString", "Text", productionStatusVM.Floor);
            ViewBag.Lines = new SelectList(productionFloorLogic.GetLineDropDownByFloor(productionStatusVM.Floor), "ValueString", "Text", productionStatusVM.Line);

            return PartialView(productionStatusVM);
        }


        [HttpPost]
        public ActionResult Edit(ProductionStatusViewModel productionStatusVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    productionStatusLogic.UpdateProductionStatus(productionStatusVM);

                    return Json(true);
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                }
            }

            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text", productionStatusVM.StyleID);
            ViewBag.PurchaseOrder = new SelectList(purchaseOrderLogic.GetPurchaseOrderDropDown(productionStatusVM.StyleID), "Value", "Text", productionStatusVM.PurchaseOrderID);
            ViewBag.Floors = new SelectList(productionFloorLogic.GetFloorDropDown(), "ValueString", "Text", productionStatusVM.Floor);
            ViewBag.Lines = new SelectList(productionFloorLogic.GetLineDropDownByFloor(productionStatusVM.Floor), "ValueString", "Text", productionStatusVM.Line);

            return PartialView(productionStatusVM);
        }
    }
}