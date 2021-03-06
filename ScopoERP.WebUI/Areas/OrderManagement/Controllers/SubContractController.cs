using ScopoERP.Common.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.OrderManagement.Controllers
{
    public class SubContractController : Controller
    {
        private SubContractLogic subContractLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private FactoryLogic factoryLogic;

        public SubContractController(SubContractLogic subContractLogic, PurchaseOrderLogic purchaseOrderLogic, FactoryLogic factoryLogic)
        {
            this.subContractLogic = subContractLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.factoryLogic = factoryLogic;
        }

        public ActionResult Index(int id)
        {
            ViewBag.PurchaseOrderID = id;

            return View();
        }

        public JsonResult GetAllSubContract(int purchaseOrderID)
        {
            return Json(subContractLogic.GetAllSubContract(purchaseOrderID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseOrderDetails(int purchaseOrderID)
        {
            return Json(purchaseOrderLogic.GetPurchaseOrderByID(purchaseOrderID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFactoryList()
        {
            return Json(factoryLogic.GetFactoryDropDown(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(int purchaseOrderID, List<SubContractViewModel> subContractVMList)
        {
            subContractLogic.SaveSubContract(purchaseOrderID, subContractVMList);

            return Json(true);
        }
    }
}