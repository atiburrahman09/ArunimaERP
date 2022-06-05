using ScopoERP.Common.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Production.BLL;
using ScopoERP.ProductionStatus.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    [Authorize(Roles = "Production")]
    public class ProductionPlanController : Controller
    {
        public ProductionFloorLogic productionFloorLogic;
        public ProductionPlanningLogic productionPlanningLogic;
        public StyleLogic styleLogic;
        public PurchaseOrderLogic purchaseOrderLogic;
        public FactoryLogic factoryLogic;
        
        public ProductionPlanController(ProductionFloorLogic productionFloorLogic, ProductionPlanningLogic productionPlanningLogic, StyleLogic styleLogic, PurchaseOrderLogic purchaseOrderLogic, FactoryLogic factoryLogic)
        {
            this.productionFloorLogic = productionFloorLogic;
            this.productionPlanningLogic = productionPlanningLogic;
            this.styleLogic = styleLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.factoryLogic = factoryLogic;
        }
        
        // GET: Production/ProductionPlan
        public ActionResult Index()
        {
            ViewBag.Floor = new SelectList(productionFloorLogic.GetFloorDropDown(), "ValueString", "Text");
            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.Factory = new SelectList(factoryLogic.GetFactoryDropDown(), "Value", "Text");
            return View();
        }

        public ActionResult GetPurchaseOrderByStyle(int styleID, int factoryID, DateTime fromDate, DateTime toDate) 
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderDropDown(styleID, factoryID, fromDate, toDate), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStyleByDateRange(int factoryID, DateTime fromDate, DateTime toDate)
        {
            return Json(styleLogic.GetStyleDropDown(factoryID, fromDate, toDate), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLineByFloor(string floor)
        {
            return Json(productionFloorLogic.GetAllLineByFloor(floor), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductionPlanByMonth(int month, int year) 
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            return Json(productionPlanningLogic.GetAllProductionPlanByDate(startDate, endDate), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCreatePopUp(int purchaseOrderID) 
        {
            string StyleCapacityOrderQty = productionPlanningLogic.GetStyleCapacityOrderQty(purchaseOrderID);

            ViewBag.StyleCapacity = StyleCapacityOrderQty.Split(',')[0];
            ViewBag.OrderQty = StyleCapacityOrderQty.Split(',')[1];
            return PartialView("_ProductionPlanCreatePopUp");
        }

        public ActionResult CheckValidPlan(int productionPlanningID, int purchaseOrderID, DateTime startDate, int floorLineID)
        {
            return Json(productionPlanningLogic.CheckValidPlan(productionPlanningID, purchaseOrderID, startDate, floorLineID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(int purchaseOrderID, DateTime startDate, int floorLineID, int lineQuantity, int lineCapacity)
        {
            try
            {
                productionPlanningLogic.CreateProductionPlan(purchaseOrderID, startDate, floorLineID, lineQuantity, lineCapacity);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ReschedulePlan(int productionPlanningID, DateTime startDate, DateTime endDate, int floorLineID, int isResize)
        {
            try 
            {
                productionPlanningLogic.ReschedulePlan(productionPlanningID, startDate, endDate, floorLineID, isResize);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int productionPlanningID)
        {
            try
            {
                productionPlanningLogic.DeleteProductionPlan(productionPlanningID);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SplitProductionPlan(int productionPlanningID)
        {
            try
            {
                productionPlanningLogic.SplitProductionPlan(productionPlanningID);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetRawMaterialStatus(int purchaseOrderID)
        {
            return PartialView("_RawMaterialStatus", productionPlanningLogic.GetRawMaterialStatus(purchaseOrderID));
        }

        public ActionResult GetDailyProductionStatus(int purchaseOrderID)
        {
            return Json(productionPlanningLogic.GetDailyProductionStatus(purchaseOrderID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPurchaseOrderDetails(int purchaseOrderID)
        {
            return PartialView("_PurchaseOrderDetails");
        }

        public ActionResult GetProductionStatistics(int purchaseOrderID)
        {
            return PartialView("_ProductionStatistics");
        }
    }
}