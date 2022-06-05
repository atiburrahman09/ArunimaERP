using ScopoERP.Common.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Production.BLL;
using ScopoERP.Production.ViewModel;
using ScopoERP.ProductionStatus.BLL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Production.Controllers
{
    public class SewingPlanController : Controller
    {
        private SewingPlanLogic sewingPlanLogic;
        private StyleLogic styleLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private ProductionFloorLogic productionFloorLogic;
        private FactoryLogic factoryLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sewingPlanLogic"></param>
        /// <param name="styleLogic"></param>
        /// <param name="purchaseOrderLogic"></param>
        /// <param name="productionFloorLogic"></param>
        /// <param name="factoryLogic"></param>
        public SewingPlanController(SewingPlanLogic sewingPlanLogic, StyleLogic styleLogic,
            PurchaseOrderLogic purchaseOrderLogic, ProductionFloorLogic productionFloorLogic, FactoryLogic factoryLogic)
        {
            this.sewingPlanLogic = sewingPlanLogic;
            this.styleLogic = styleLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.productionFloorLogic = productionFloorLogic;
            this.factoryLogic = factoryLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.styleList = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.factoryList = new SelectList(factoryLogic.GetFactoryDropDown(), "Value", "Text");
            ViewBag.floorList = new SelectList(productionFloorLogic.GetFloorDropDown(), "ValueString", "Text");

            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public JsonResult GetLineDropDown(string floor)
        {
            return Json(productionFloorLogic.GetLineDropDownByFloor(floor), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleID"></param>
        /// <returns></returns>
        public ViewResult GetAll(int styleID, string floor, string line, DateTime? fromDate, DateTime? toDate)
        {
            var data = sewingPlanLogic.GetAllSewingPlan(styleID, floor, line, fromDate, toDate);

            return View(data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sewingPlanList"></param>
        /// <returns></returns>
        public JsonResult Save(List<SewingPlanViewModel> sewingPlanList)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    sewingPlanLogic.SaveSewingPlan(sewingPlanList);

                    return Json(true);
                }
                catch(DbException ex)
                {
                    return Json(false);
                }
            }
            return Json(false);
        }
    }
}