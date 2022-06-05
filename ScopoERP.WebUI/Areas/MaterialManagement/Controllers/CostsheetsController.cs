using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace ScopoERP.WebUI.Areas.MaterialManagement.Controllers
{
    public class CostsheetsController : Controller
    {
        private StyleLogic styleLogic;
        private CostsheetLogic costsheetLogic;
        private ConsumptionUnitLogic consumptionUnitLogic;
        private ItemCategoryLogic itemCategoryLogic;


        public CostsheetsController
        (
            StyleLogic styleLogic,
            CostsheetLogic costsheetLogic, 
            ConsumptionUnitLogic consumptionUnitLogic,
            ItemCategoryLogic itemCategoryLogic
        )
        {
            this.styleLogic           = styleLogic;
            this.costsheetLogic       = costsheetLogic;
            this.consumptionUnitLogic = consumptionUnitLogic;
            this.itemCategoryLogic    = itemCategoryLogic;
        }


        public ActionResult Index()
        {
            @ViewBag.styleList = new SelectList(styleLogic.GetStyleDropDown(CurrentUser.AccountID), "Value", "Text");

            return View();
        }


        public ActionResult Details(string costsheetNo)
        {
            @ViewBag.CostSheetNo = costsheetNo;

            return PartialView("~/Areas/MaterialManagement/Views/Costsheets/Details.cshtml");
        }


        public JsonResult GetCostSheetByStyle(int styleID)
        {
            return Json(costsheetLogic.GetCostSheetDropDown(styleID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCostSheetDetails(string costSheetNo)
        {
            return Json(costsheetLogic.GetCostSheetByCostsheetNo(costSheetNo), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetConsumptionUnit()
        {
            return Json(consumptionUnitLogic.GetConsumptionUnitDropDown(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetItemCategory()
        {
            return Json(itemCategoryLogic.GetItemCategoryDropDown(), JsonRequestBehavior.AllowGet);
        }
    }
}