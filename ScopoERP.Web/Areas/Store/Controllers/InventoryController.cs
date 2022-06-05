using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Production.BLL;
using ScopoERP.Production.ViewModel;
using ScopoERP.Store.BLL;
using ScopoERP.Store.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Store.Controllers
{
    [Authorize(Roles = "Inventory")]
    public class InventoryController : Controller
    {
        private UnitOfWork unitOfWork;
        private BLDetailsLogic blDetailsLogic;
        private BLLogic blLogic;
        private PILogic piLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private ItemLogic itemLogic;
        private InventoryIssueLogic inventoryIssueLogic;
        private ProductionFloorLogic productionFloorLogic;

        public InventoryController(UnitOfWork unitOfWork, BLDetailsLogic blDetailsLogic, BLLogic blLogic, PILogic piLogic, PurchaseOrderLogic purchaseOrderLogic, ItemLogic itemLogic, InventoryIssueLogic inventoryIssueLogic, ProductionFloorLogic productionFloorLogic)
        {
            this.unitOfWork = unitOfWork;
            this.blDetailsLogic = blDetailsLogic;
            this.blLogic = blLogic;
            this.piLogic = piLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.itemLogic = itemLogic;
            this.inventoryIssueLogic = inventoryIssueLogic;
            this.productionFloorLogic = productionFloorLogic;
        }

        public ActionResult Index()
        {
            ViewBag.PI = new SelectList(piLogic.GetPIDropDown(), "Value", "Text");

            return View();
        }

        public JsonResult GetAllBL()
        {
            var blList = blLogic.GetAllBL();

            return Json(blList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllChalan()
        {
            var blList = blLogic.GetAllChalan();
            return Json(blList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateChalan(BLViewModel blVM)
        {
            if (ModelState.IsValid)
            {
                if (blVM.BLID == 0)
                {
                    int blID = blLogic.CreateChalan(blVM);
                    BLViewModel result = blLogic.GetBLByID(blID);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    blLogic.UpdateBL(blVM);
                    return Json(true);
                }
            }
            return Json(blVM, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPIDropdown()
        {
            List<DropDownListViewModel> piList = piLogic.GetPIDropDownByDateRange();
            return Json(piList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPIDropdownByBL(int blID)
        {
            List<DropDownListViewModel> piList = piLogic.GetPIDropDownByBLUsingBooking(blID);
            return Json(piList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBLDetails(int blID, int? piID, int? itemID)
        {
            List<BLDetailsViewModel> blDetails = blDetailsLogic.GetBLDetailsForInventory(blID, piID, itemID);
            return Json(blDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBLDetailsForChalan(int blID, int piID)
        {
            List<BLDetailsViewModel> blDetails = blDetailsLogic.GetBLDetailsForChalan(blID, piID);
            return Json(blDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdateBLDetails(List<BLDetailsViewModel> blDetailsVM)
        {
            if (ModelState.IsValid)
            {
                blDetailsLogic.UpdateBLDetails(blDetailsVM);
                return Json(true);
            }
            return Json(false);
        }

        public JsonResult GetBLByPI(int piID)
        {
            var blList = blLogic.GetBLDropDownByPIID(piID);

            return Json(blList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemByBL(int blID)
        {
            return Json(blDetailsLogic.GetItemDropDownByBL(blID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveInventory(List<BLDetailsViewModel> blDetailsVM)
        {
            try
            {
                blDetailsLogic.UpdateBLDetails(blDetailsVM);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }

        }

        public ActionResult InventoryIssue()
        {
            return View();
        }


        [HttpPost]
        public JsonResult SaveIssuedInventories(SrViewModel srVM)
        {
            if (!ModelState.IsValid)
            {
                var err = ModelState.Values
                            .SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
                            .ToList();

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(err, JsonRequestBehavior.AllowGet);
            }

            try
            {
                if (inventoryIssueLogic.IsUnique(srVM))
                {
                    srVM.CreatedBy = User.Identity.Name;
                    var srId = inventoryIssueLogic.SaveSr(srVM);
                    inventoryIssueLogic.SaveInventoryIssue(srVM, srId);
                    return Json("Successfully saved", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("Duplicate serial number", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetIssues()
        {
            var data = inventoryIssueLogic.GetIssues();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIssueById(int id)
        {
            var data = inventoryIssueLogic.GetIssueById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIssueBySR(string id)
        {
            var data = inventoryIssueLogic.GetIssueBySR(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseOrderDropDown()
        {
            List<DropDownListViewModel> poList = purchaseOrderLogic.GetPurchaseOrderDropDown();
            return Json(poList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllItemByPOID(int purchaseOrderID)
        {
            List<DropDownListViewModel> itemList = itemLogic.GetItemDropDownByPurchaseOrder(purchaseOrderID);
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllFloorLine()
        {
            List<ProductionFloorViewModel> floorLineList = productionFloorLogic.GetAllProductionFloor();
            return Json(floorLineList, JsonRequestBehavior.AllowGet);
        }



    }
}