using ScopoERP.Accounts.BLL;
using ScopoERP.Accounts.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.Web.Areas.Misc.Controllers
{
    [Authorize(Roles = "AdvancedCM")]
    public class PurchaseRequisitionInstallmentController : Controller
    {
        private PurchaseRequisitionInstallmentLogic purchaseRequisitionInstallmentLogic;
        private PurchaseRequisitionLogic purchaseRequisitionLogic;

        public PurchaseRequisitionInstallmentController(PurchaseRequisitionInstallmentLogic purchaseRequisitionInstallmentLogic, PurchaseRequisitionLogic purchaseRequisitionLogic)
        {
            this.purchaseRequisitionInstallmentLogic = purchaseRequisitionInstallmentLogic;
            this.purchaseRequisitionLogic = purchaseRequisitionLogic;
        }


        public ActionResult Index()
        {
            IEnumerable<PurchaseRequisitionInstallmentViewModel> purchaseRequisitionInstallmentVM = purchaseRequisitionInstallmentLogic.GetAll();
            return View(purchaseRequisitionInstallmentVM);
        }


        public ActionResult Create()
        {
            ViewBag.requisitionList = new SelectList(purchaseRequisitionLogic.GetPurchaseRequisitionDropDown(), "Value", "Text");

            return View();
        }


        [HttpPost]
        public ActionResult Create(PurchaseRequisitionInstallmentViewModel purchaseRequisitionInstallmentVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    purchaseRequisitionInstallmentLogic.Create(purchaseRequisitionInstallmentVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.requisitionList = new SelectList(purchaseRequisitionLogic.GetPurchaseRequisitionDropDown(), "Value", "Text", purchaseRequisitionInstallmentVM.PurchaseRequisitionID);
            return View(purchaseRequisitionInstallmentVM);
        }


        public ActionResult Edit(int id)
        {
            PurchaseRequisitionInstallmentViewModel purchaseRequisitionInstallmentVM = purchaseRequisitionInstallmentLogic.GetByID(id);

            if (purchaseRequisitionInstallmentVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.requisitionList = new SelectList(purchaseRequisitionLogic.GetPurchaseRequisitionDropDown(), "Value", "Text", purchaseRequisitionInstallmentVM.PurchaseRequisitionID);
            return View(purchaseRequisitionInstallmentVM);
        }


        [HttpPost]
        public ActionResult Edit(PurchaseRequisitionInstallmentViewModel purchaseRequisitionInstallmentVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    purchaseRequisitionInstallmentLogic.Update(purchaseRequisitionInstallmentVM);

                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                    the problem persists, Contact with Entitas Technologia.");
                }
            }
            ViewBag.requisitionList = new SelectList(purchaseRequisitionLogic.GetPurchaseRequisitionDropDown(), "Value", "Text", purchaseRequisitionInstallmentVM.PurchaseRequisitionID);
            return View(purchaseRequisitionInstallmentVM);
        }


        public JsonResult GetTotalAmount(int? purchaseRequisitionID)
        {
            return Json(purchaseRequisitionLogic.GetTotalAmount((int)purchaseRequisitionID), JsonRequestBehavior.AllowGet);
        }
    }
}