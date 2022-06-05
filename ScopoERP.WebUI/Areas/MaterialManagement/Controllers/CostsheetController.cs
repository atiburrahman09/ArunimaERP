using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.WebUI.Helper;
using ScopoERP.WebUI.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.MaterialManagement.Controllers
{
    [Authorize(Roles = "Merchant")]
    public class CostsheetController : Controller
    {
        private StyleLogic styleLogic;
        private ConsumptionUnitLogic consumptionUnitLogic;
        private CostsheetLogic costsheetLogic;
        private ItemLogic itemLogic;
        private ItemCategoryLogic itemCategoryLogic;

        public CostsheetController(StyleLogic styleLogic, ConsumptionUnitLogic consumptionUnitLogic, CostsheetLogic costsheetLogic, ItemLogic itemLogic, ItemCategoryLogic itemCategoryLogic)
        {
            this.styleLogic = styleLogic;
            this.consumptionUnitLogic = consumptionUnitLogic;
            this.costsheetLogic = costsheetLogic;
            this.itemLogic = itemLogic;
            this.itemCategoryLogic = itemCategoryLogic;
        }
        public ActionResult Index()
        {
            ViewBag.StyleDropDown = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.CopyFromStyleDropDown = new SelectList(styleLogic.GetStyleDropDownByExistingCostsheet(), "Value", "Text");
            return View();
        }

        public ActionResult GetCosheetNoDropDown(int styleID)
        {
            return Json(costsheetLogic.GetCostsheetNoByStyle(styleID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemDropDown(int itemCategoryID)
        {
            return Json(itemLogic.GetItemDropDown(itemCategoryID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(string costSheetNo = null) 
        {
            if (costSheetNo != null)
            {
                List<CostsheetViewModel> costSheetList = costsheetLogic.GetCostSheetByCostsheetNo(costSheetNo);

                List<SelectList> itemCategoryList = new List<SelectList>();
                List<SelectList> itemList = new List<SelectList>();
                List<SelectList> consumtionUnitList = new List<SelectList>();
                List<SelectList> conversionUnitList = new List<SelectList>();

                for (int i = 0; i < costSheetList.Count; i++) 
                {
                    itemCategoryList.Add(new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text", costSheetList[i].ItemCategoryID));
                    itemList.Add(new SelectList(itemLogic.GetItemDropDown(costSheetList[i].ItemCategoryID), "Value", "Text", costSheetList[i].ItemID));
                    consumtionUnitList.Add(new SelectList(consumptionUnitLogic.GetConsumptionUnitDropDown(), "Value", "Text", costSheetList[i].ConsumptionUnitID));
                    conversionUnitList.Add(new SelectList(consumptionUnitLogic.GetConsumptionUnitDropDown(), "Value", "Text", costSheetList[i].ConversionUnitID));
                }

                ViewBag.ItemCategoryDropDown = itemCategoryList;
                ViewBag.ItemDropDown = itemList;
                ViewBag.ConsumptionUnitDropDown = consumtionUnitList;
                ViewBag.ConversionUnitDropDown = conversionUnitList;

                return PartialView("_Edit", costSheetList);
            }
            ViewBag.ItemCategoryDropDown = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text");
            ViewBag.UnitDropDown = new SelectList(consumptionUnitLogic.GetConsumptionUnitDropDown(), "Value", "Text");

            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult Create(List<CostsheetViewModel> costSheetVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = (from c in costSheetVM
                                  group c.ItemID by c.ItemID into g
                                  where g.Count() > 1
                                  select g.Count()).FirstOrDefault();

                    if (result > 0)
                    {
                        return Json(new { errorMessage = "Duplicate item is not allowed" });
                    }

                    string costsheetNo = costsheetLogic.CreateCostsheet(costSheetVM);
                    return Json(costsheetNo);
                }
                catch (Exception ex)
                {

                }
            }
            return Json(new { errorMessage = "Please insert all the field" });
        }

        [HttpPost]
        public ActionResult Copy(string existingCostsheetNo, int toStyleID)
        {
            try
            {
                string costSheetNo = costsheetLogic.CopyCostSheet(existingCostsheetNo, toStyleID);
                return Json(costSheetNo);
            }
            catch (Exception ex)
            {
            }
            return Json(false);
        }

        public ActionResult Export(string costsheetNo)
        {
            List<CostsheetViewModel> data = costsheetLogic.GetCostSheetByCostsheetNo(costsheetNo);
            var output = ReportHelper.ConvertToCSV<CostsheetViewModel>("", data);

            return File(output, "text/comma-separated-values", "Initial Costsheet " + DateTime.Now.Date + " .csv");
        }

        [HttpPost]
        public ActionResult Delete(string costsheetNo)
        {
            try
            {
                costsheetLogic.DeleteCostSheet(costsheetNo);
                return Json(true);
            }
            catch (Exception ex)
            {
            }
            return Json(false);
        }
    }
}