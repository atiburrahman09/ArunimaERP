using ScopoERP.Common.BLL;
using ScopoERP.Common.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Stackholder.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Common.Controllers
{
    [Authorize(Roles = "Admin")]

    public class StackholdersController : Controller
    {
        private BuyerLogic buyerLogic;
        private CustomerLogic customerLogic;
        private SupplierLogic supplierLogic;
        private SampleTypeLogic sampleTypeLogic;


        public StackholdersController(BuyerLogic buyerLogic, CustomerLogic customerLogic, SupplierLogic supplierLogic, SampleTypeLogic sampleTypeLogic)
        {
            this.buyerLogic = buyerLogic;
            this.customerLogic = customerLogic;
            this.supplierLogic = supplierLogic;
            this.sampleTypeLogic = sampleTypeLogic;
        }
        // GET: Stackholder/Stackholders    
        public ActionResult Index()
        {
            return View();
        }

        // Buyer Section
        public JsonResult GetAllBuyer()
        {
            List<BuyerViewModel> buyerVM = buyerLogic.GetAllBuyer();
            return Json(buyerVM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateBuyer(BuyerViewModel buyerVM)
        {
            if(ModelState.IsValid)
            {
                if (!buyerLogic.IsUniqueBuyer(buyerVM.BuyerName.Trim(),buyerVM.BuyerID))
                {
                    ModelState.AddModelError("", buyerVM.BuyerName + " already exists");
                }
                else
                {
                    try
                    {
                        buyerLogic.CreateBuyer(buyerVM);
                        // need to build message here
                        return Json(true);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return Json(false);
        }


        // Customer Section
        public JsonResult GetAllCustomer()
        {
            List<CustomerViewModel> customerVM = customerLogic.GetAllCustomer();
            return Json(customerVM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateCustomer(CustomerViewModel customerVM)
        {
            if (ModelState.IsValid)
            {
                if (!customerLogic.IsUniqueCustomer(customerVM.CustomerName.Trim()))
                {
                    ModelState.AddModelError("", customerVM.CustomerName + " already exists");
                }
                else
                {
                    try
                    {
                        customerLogic.CreateCustomer(customerVM);
                        return Json(true);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return Json(false);
        }


        // Supplier Section
        public JsonResult GetAllSupplier()
        {
            List<SupplierViewModel> supplierVM = supplierLogic.GetAllSupplier();
            return Json(supplierVM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateSupplier(SupplierViewModel supplierVM)
        {
            if (ModelState.IsValid)
            {
                if (!supplierLogic.IsUniqueSupplier(supplierVM.SupplierName.Trim()))
                {
                    ModelState.AddModelError("", supplierVM.SupplierName + " already exists");
                }
                else
                {
                    try
                    {
                        supplierLogic.CreateSupplier(supplierVM);
                        return Json(true);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return Json(false);
        }


        //Sample Type Section
        public JsonResult GetAllSampleType()
        {
            List<SampleTypeViewModel> sampleTypeList = sampleTypeLogic.GetAllSampleType();
            return Json(sampleTypeList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateSampleType(SampleTypeViewModel sampleTypeVM)
        {
            if (ModelState.IsValid)
            {
                if (!sampleTypeLogic.IsUniqueSampleType(sampleTypeVM.SampleTypeName.Trim(),sampleTypeVM.SampleTypeID))
                {
                    ModelState.AddModelError("", sampleTypeVM.SampleTypeName + " already exists");
                }
                else
                {
                    try
                    {
                        sampleTypeLogic.CreateSampleType(sampleTypeVM,User.Identity.Name);
                        return Json(true);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            return Json(false);
        }
    }
}