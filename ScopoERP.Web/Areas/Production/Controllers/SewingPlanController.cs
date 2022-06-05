using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Production.BLL;
using ScopoERP.ProductionStatus.BLL;
using ScopoERP.ProductionStatus.ViewModel;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    public class SewingPlanController : Controller
    {
        private JsonRequestBehavior Allow = JsonRequestBehavior.AllowGet;
        private JsonRequestBehavior deny = JsonRequestBehavior.DenyGet;

        private SewingPlanLogic sewingPlanLogic;

        private ProductionFloorLogic productionFloorLogic;
        private StyleLogic styleLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        public SewingPlanController(SewingPlanLogic swingPlanLogic, 
                                    ProductionFloorLogic productionFloorLogic, 
                                    StyleLogic styleLogic,
                                    PurchaseOrderLogic purchaseOrderLogic
                                    )
        {
            this.sewingPlanLogic = swingPlanLogic;
            this.styleLogic = styleLogic;
            this.productionFloorLogic = productionFloorLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
        }

        // GET: Production/SwingPlan
        public ActionResult Index()
        {           
            var data = sewingPlanLogic.GetAll();
            return View(data.ToList());
        }
        

        // GET: Production/SwingPlan/Create
        public ActionResult Create()
        {
            ViewBag.Floor = new SelectList(sewingPlanLogic.GetFLoorLineDropdown(), "Value", "Text");
            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.PO = new SelectList(new List<DropDownListViewModel>(), "Value", "Text");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductionPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Floor = new SelectList(sewingPlanLogic.GetFLoorLineDropdown(), "Value", "Text");
                ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
                ViewBag.PO = new SelectList(new List<DropDownListViewModel>(), "Value", "Text");
                return View(model);
            }


            try
            {
                sewingPlanLogic.Create(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }

        [HttpGet]
        // /Production/SewingPlan/GetPurchaseOrdersByStyle
        public JsonResult GetPurchaseOrdersByStyle(int styleId)
        {
            var data = purchaseOrderLogic.GetPurchaseOrderDropDown(styleId);
            return Json(data, Allow);
        }

        // GET: Production/SwingPlan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = sewingPlanLogic.GetById(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            ViewBag.Floor = new SelectList(sewingPlanLogic.GetFLoorLineDropdown(), "Value", "Text");            
            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.PO = new SelectList(purchaseOrderLogic.GetPurchaseOrderDropDown(data.StyleID), "Value", "Text", data.PoStyleID);
            return View(data);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductionPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                sewingPlanLogic.Update(model);
                return RedirectToAction("Index");
            }
            ViewBag.Floor = new SelectList(sewingPlanLogic.GetFLoorLineDropdown(), "Value", "Text");
            ViewBag.Style = new SelectList(styleLogic.GetStyleDropDown(), "Value", "Text");
            ViewBag.PO = new SelectList(new List<DropDownListViewModel>(), "Value", "Text");
            return View(model);
        }

        // GET: Production/SwingPlan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = sewingPlanLogic.GetById(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        // POST: Production/SwingPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sewingPlanLogic.Delete(id);
            return RedirectToAction("Index");
        }

     
        public ActionResult EmployeeMaping(int? id)
        {
            ViewBag.ProductionPlanningID = id;
            return View();
        }

        [HttpGet]
        public JsonResult GetEmployeeList(string floor)
        {
            var data = sewingPlanLogic.GetEmployeeList(floor);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        // /Production/SewingPlan/GetPurchaseOrdersByStyle
        public JsonResult GetProductionPlanningData(int? id)
        {
            var data = sewingPlanLogic.GetProductionPlanningData(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        // /Production/SewingPlan/GetPurchaseOrdersByStyle
        public JsonResult GetPoEmployeeMappingData(int? id)
        {
            var data = sewingPlanLogic.GetPoEmployeeMappingData(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]        
        public JsonResult Save(List<PoEmployeeMappingViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                return Json(false);
            }
            try
            {
                sewingPlanLogic.SavePOEmployeeMapping(model);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }

        }
    }
}
