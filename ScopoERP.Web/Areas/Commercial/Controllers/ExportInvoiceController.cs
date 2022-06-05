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

namespace ScopoERP.Web.Areas.Commercial.Controllers
{
    [Authorize(Roles = "Commercial")]    
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
        
        //------------------------New Methods-----------//

        public JsonResult GetExportInvoiceByJobID(int jobID)
        {

            var data = exportInvoiceLogic.GetAllExportInvoiceByJob(jobID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJobDropDown()
        {
            var data = jobLogic.GetJobDropDown();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
          
        [HttpPost]
        public JsonResult SaveExportInvoice(ExportInvoiceViewModel exportInvoiceVM)
        {
            if (ModelState.IsValid)
            {
                if(exportInvoiceVM.InvoiceId == 0)
                {
                    if (!exportInvoiceLogic.IsUniqueInvoiceNo(exportInvoiceVM.InvoiceNo))
                        return Json(new { Error = "Invoice Already Exists!" });
                    try
                    {
                        exportInvoiceLogic.CreateExportInvoice(exportInvoiceVM);
                        return Json(new { Success = "Created Successfully!"});
                    }                  
                    catch(Exception ex)
                    {
                        return Json(new { Error = "An unexpected error occured !" });
                    }
                }
                try
                {
                    exportInvoiceLogic.UpdateExportInvoice(exportInvoiceVM);
                    return Json(new { Success = "Updated Successfully!"});
                }
                catch (Exception ex)
                {
                    return Json(new { Error = "An unexpected error occured !" });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(x => "<li>" + x.ErrorMessage + "</li>");
            
            return Json(new { Error = errors });
        }

        //Existing method
        public JsonResult GetPurchaseOrderByJob(int jobID)
        {
            var purchaseOrderList = purchaseOrderLogic.GetUnAssignedPurchaseOrderDropDownByJob(jobID);
            return Json(purchaseOrderList, JsonRequestBehavior.AllowGet);
        }

        //Existing method
        public JsonResult GetShipmentByPurchaseOrder(int purchaseOrderID)
        {
            var shipment = shipmentLogic.GetShipmentByPurchaseOrder(purchaseOrderID);

            return Json(shipment, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShipmentByInvoiceID(int invoiceID)
        {
            var data = shipmentLogic.GetAllShipmentByInvoice(invoiceID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetAllExportInvoiceByJobID(int jobID)
        {
            var data = exportInvoiceLogic.GetAllExportInvoiceByJob(jobID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}