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
using ScopoERP.Web.Helper;
using System.Net;

namespace ScopoERP.Web.Areas.Misc.Controllers
{
    [Authorize(Roles = "AdvancedCM")]
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
            var requisitionList = purchaseRequisitionLogic
                    .GetAllPurchaseRequisition(User.Identity.Name);
            return View(requisitionList);
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
            if (ModelState.IsValid)
            {
                try
                {
                    purchaseRequisition.UserID = 0;
                    string requisitionNo = purchaseRequisitionLogic
                            .SavePurchaseRequisition(purchaseRequisition);
                    return Json(requisitionNo, JsonRequestBehavior.DenyGet);
                }
                catch (Exception ex)
                {
                    var errMsg = ex.InnerException.Message;
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(errMsg, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.DenyGet);
        }
    }
}