using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.MaterialManagement.Controllers
{
    [Authorize(Roles = "Merchant")]
    public class WorksheetController : Controller
    {
        private StyleLogic styleLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private CostsheetLogic costsheetLogic;
        private WorksheetLogic worksheetLogic;
        private SizeColorLogic sizeColorLogic;
        public WorksheetController(StyleLogic styleLogic, PurchaseOrderLogic purchaseOrderLogic, CostsheetLogic costsheetLogic, WorksheetLogic worksheetLogic, SizeColorLogic sizeColorLogic)
        {
            this.styleLogic = styleLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.costsheetLogic = costsheetLogic;
            this.worksheetLogic = worksheetLogic;
            this.sizeColorLogic = sizeColorLogic;
        }
        // GET: MaterialManagement/Worksheet
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStyleDropDown()
        {
            return Json(styleLogic.GetStyleDropDown(CurrentUser.AccountID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSingleCostsheetDropDownBySingleStyle(int styleID)
        {
            return Json(costsheetLogic.GetCostsheetNoByStyle(styleID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSinglePODropDownBySingleStyle(int styleID)
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderDropDown(styleID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFormulaDropDown()
        {
            return Json(worksheetLogic.GetFormulaDropDown(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemBySingleCostsheet(string costsheetNo)
        {
            return Json(costsheetLogic.GetItemByCostsheet(costsheetNo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWorksheet(int purchaseOrderID, int itemID)
        {
            return Json(worksheetLogic.GetWorksheet(purchaseOrderID, itemID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsSizeColorExists(int purchaseOrderID)
        {
            if (sizeColorLogic.IsSizeColorExists(purchaseOrderID))
                return Json(true, JsonRequestBehavior.AllowGet);

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateWorksheet(string costsheetNo, int purchaseOrderID, int itemID, int formula)
        {
            try
            {
                if (worksheetLogic.isValidCostsheetForPO(costsheetNo, purchaseOrderID))
                {
                    worksheetLogic.CreateWorksheet(costsheetNo, purchaseOrderID, itemID, formula);
                    return Json(worksheetLogic.GetWorksheet(purchaseOrderID, itemID));
                }
                else
                    return Json(1); // if combination of costsheet and po is not valid
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        public ActionResult UpdateWorksheet(List<WorksheetViewModel> worksheetVM)
        {
            try
            {
                worksheetLogic.UpdateWorksheet(worksheetVM);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        public ActionResult DeleteWorksheet(int purchaseOrderID, int itemID)
        {
            try
            {
                worksheetLogic.DeleteWorksheet(purchaseOrderID, itemID);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        public ActionResult GetReferenceNo(int purchaseOrderID, int itemID)
        {
            string refernceNo = worksheetLogic.GetReferenceNo(purchaseOrderID, itemID);
            return Json(refernceNo, JsonRequestBehavior.AllowGet);
        }

    }
}