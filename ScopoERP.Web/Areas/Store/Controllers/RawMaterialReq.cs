using ScopoERP.Common.ViewModel;
using ScopoERP.MaterialManagement.BLL;
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
    [Authorize(Roles = "Dpr-Entry")]
    public class RawMaterialReqController : Controller
    {
        private InventoryIssueLogic inventoryIssueLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private ItemLogic itemLogic;
        private ProductionFloorLogic productionFloorLogic;

        public RawMaterialReqController(InventoryIssueLogic inventoryIssueLogic, PurchaseOrderLogic purchaseOrderLogic, ItemLogic itemLogic, ProductionFloorLogic productionFloorLogic)
        {
            this.inventoryIssueLogic = inventoryIssueLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
            this.itemLogic = itemLogic;
            this.productionFloorLogic = productionFloorLogic;
        }
        // GET: Store/InventoryIssueRequest
        public ActionResult Index()
        {
            return View();
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
                    if (srVM.SRID == 0)
                    {
                        srVM.SRNo = inventoryIssueLogic.GetAutoSRNo();
                    }                    
                    var srId = inventoryIssueLogic.SaveSr(srVM);
                    inventoryIssueLogic.SaveInventoryIssue(srVM, srId);
                    return Json(new {id=srId,msg= "Successfully saved",srNo=srVM.SRNo} , JsonRequestBehavior.AllowGet);
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


    }
}