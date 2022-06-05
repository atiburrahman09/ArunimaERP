using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.LC.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Store.BLL;
using ScopoERP.Store.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Commercial.Controllers
{
    [Authorize(Roles = "Commercial")]
    [AllowAnonymous]
    public class ExportInvoiceController : Controller
    {
        private ExportInvoiceLogic exportInvoiceLogic;
        private JobLogic jobLogic;
        private ShipmentLogic shipmentLogic;
        private PurchaseOrderLogic purchaseOrderLogic;

        public ExportInvoiceController(ExportInvoiceLogic exportInvoiceLogic, JobLogic jobLogic, ShipmentLogic shipmentLogic,
                                        PurchaseOrderLogic purchaseOrderLogic)
        {
            this.exportInvoiceLogic = exportInvoiceLogic;
            this.jobLogic = jobLogic;
            this.shipmentLogic = shipmentLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
        }


        public ViewResult Index()
        {
            return View();
        }


        [GridAction]
        public ViewResult GetAllExportInvoice()
        {
            var exportInvoiceList = exportInvoiceLogic.GetAllExportInvoice();

            return View(new GridModel(exportInvoiceList));
        }


        [GridAction]
        public ActionResult GetAllShipment()
        {
            List<ShipmentViewModel> data = shipmentLogic.GetAllShipment().ToList();

            return View(new GridModel(data));
        }


        public JsonResult GetPurchaseOrderByJob(int jobID)
        {
            var purchaseOrderList = purchaseOrderLogic.GetPurchaseOrderDropDownByJob(jobID);

            return Json(purchaseOrderList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetShipmentByPurchaseOrder(int purchaseOrderID)
        {
            var shipmentList = shipmentLogic.GetShipmentByPurchaseOrder(purchaseOrderID);

            return Json(shipmentList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text");

            return View();
        }


        [HttpPost]
        public ActionResult Create(ExportInvoiceViewModel exportInvoiceVM)
        {
            if (ModelState.IsValid)
            {
                if (exportInvoiceLogic.IsUniqueInvoiceNo(exportInvoiceVM.InvoiceNo))
                {
                    exportInvoiceLogic.CreateExportInvoice(exportInvoiceVM);

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("InvoiceNo", exportInvoiceVM.InvoiceNo + " Already Exists");
                }
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", exportInvoiceVM.JobID);

            return View(exportInvoiceVM);
        }


        public ActionResult Edit(int id)
        {
            var exportInvoiceVM = exportInvoiceLogic.GetExportInvoiceByID(id);
            var shipmentList = shipmentLogic.GetAllShipmentByInvoice(id);
            exportInvoiceVM.ShipmentList = shipmentList;

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", exportInvoiceVM.JobID);
            ViewBag.PurchaseOrder = new SelectList(purchaseOrderLogic.GetPurchaseOrderDropDownByJob(exportInvoiceVM.JobID ?? 0), "Value", "Text");

            return View(exportInvoiceVM);
        }


        [HttpPost]
        public ActionResult Edit(ExportInvoiceViewModel exportInvoiceVM)
        {
            if (ModelState.IsValid)
            {
                exportInvoiceLogic.UpdateExportInvoice(exportInvoiceVM);

                return RedirectToAction("Index");
            }

            ViewBag.Job = new SelectList(jobLogic.GetJobDropDown(), "Value", "Text", exportInvoiceVM.JobID);
            ViewBag.PurchaseOrder = new SelectList(purchaseOrderLogic.GetPurchaseOrderDropDownByJob(exportInvoiceVM.JobID ?? 0), "Value", "Text");

            return View(exportInvoiceVM);
        }
    }
}