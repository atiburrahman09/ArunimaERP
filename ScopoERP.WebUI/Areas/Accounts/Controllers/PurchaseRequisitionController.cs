using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.Accounts.ViewModel;
using ScopoERP.Accounts.BLL;
using ScopoERP.Common.BLL;
using ScopoERP.Stackholder.BLL;
using Telerik.Web.Mvc;
using ScopoERP.WebUI.Helper;

namespace ScopoERP.WebUI.Areas.Accounts.Controllers
{
    public class PurchaseRequisitionController : Controller
    {
        private ConsumptionUnitLogic consumpUnitLogic;
        private PurchaseRequisitionLogic purchaseRequisitionLogic;
        private DepartmentLogic departmentLogic;
        private SupplierLogic supplierLogic;


        public PurchaseRequisitionController(
            ConsumptionUnitLogic consumpUnitLogic, 
            PurchaseRequisitionLogic purchaseRequisitionLogic, 
            DepartmentLogic departmentLogic,
            SupplierLogic supplierLogic)
        {
            this.consumpUnitLogic = consumpUnitLogic;
            this.purchaseRequisitionLogic = purchaseRequisitionLogic;
            this.departmentLogic = departmentLogic;
            this.supplierLogic = supplierLogic;
        }


        public ActionResult Index()
        {
            return View();
        }


        [GridAction]
        public ViewResult GetAllPurchaseRequisition()
        {
            List<PurchaseRequisitionViewModel> requisitionList = purchaseRequisitionLogic.GetAllPurchaseRequisition();

            return View(new GridModel(requisitionList));
        }
        

        public ActionResult Create(int? id = null)
        {
            ViewBag.PurchaseRequisitionID = id;

            return View();
        }


        public JsonResult GetPurchaseRequisition(int purchaseRequisitionID)
        {
            PurchaseRequisitionViewModel purchaseRequisition 
                = purchaseRequisitionLogic.GetPurchaseRequisition(purchaseRequisitionID);

            return Json(purchaseRequisition, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllUnit()
        {
            return Json(consumpUnitLogic.GetConsumptionUnitDropDown(), JsonRequestBehavior.AllowGet); 
        }


        public JsonResult GetAllDepartment()
        {
            return Json(departmentLogic.GetDepartmentDropDown(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllSupplier()
        {
            return Json(supplierLogic.GetSupplierDropDown(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult Save(PurchaseRequisitionViewModel purchaseRequisition)
        {
            if(ModelState.IsValid)
            {
                purchaseRequisition.UserID = CurrentUser.UserID;

                string requisitionNo = purchaseRequisitionLogic.SavePurchaseRequisition(purchaseRequisition);

                return Json(requisitionNo, JsonRequestBehavior.DenyGet);
            }
            return Json(null, JsonRequestBehavior.DenyGet);
        }
    }
}