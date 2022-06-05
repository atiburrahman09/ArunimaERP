using ScopoERP.Common.BLL;
using ScopoERP.Common.ViewModel;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Stackholder.ViewModel;
using ScopoERP.UserManagement.BLL;
using ScopoERP.Web.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

namespace ScopoERP.Web.Areas.Merchandising.Controllers
{
    [Authorize(Roles = "merchant")]
    public class StylesController : Controller
    {
        private StyleLogic styleLogic;
        private BuyerLogic buyerLogic;
        private CustomerLogic customerLogic;
        private DivisionLogic divisionLogic;
        private CostsheetLogic costSheetLogic;
        private ItemCategoryLogic itemCategoryLogic;
        private ItemLogic itemLogic;
        private UserLogic userLogic;
        private ConsumptionUnitLogic consumptionUnitLogic;
        private CommonHelper commonHelper;

        public StylesController(StyleLogic stylelogic, BuyerLogic buyerLogic,
            CustomerLogic customerLogic, DivisionLogic divisionLogic, CostsheetLogic costSheetLogic,
            ItemCategoryLogic itemCategoryLogic, ItemLogic itemLogic, ConsumptionUnitLogic consumptionUnitLogic, UserLogic userLogic , CommonHelper commonHelper)
        {
            this.styleLogic = stylelogic;
            this.buyerLogic = buyerLogic;
            this.customerLogic = customerLogic;
            this.divisionLogic = divisionLogic;
            this.costSheetLogic = costSheetLogic;
            this.itemCategoryLogic = itemCategoryLogic;
            this.itemLogic = itemLogic;
            this.consumptionUnitLogic = consumptionUnitLogic;
            this.userLogic = userLogic;
            this.commonHelper = commonHelper;
        }

        // GET: Orders/Styles
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public JsonResult GetAllStyle(int buyerId)
        {
            Guid userId = userLogic.GetUserID(User.Identity.Name);
            int accountId = userLogic.GetAccountID(userId);
            List<StyleViewModel> stylelist = styleLogic.GetAllStyleByBuyer(accountId, buyerId);
            return Json(stylelist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult getAllBuyer()
        {
            List<BuyerViewModel> buyerList = buyerLogic.GetAllBuyer();
            return Json(buyerList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult getAllCustomer()
        {
            List<DropDownListViewModel> customerList = customerLogic.GetCustomerDropDown();
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult getAllDivision()
        {
            List<DropDownListViewModel> divisionlist = divisionLogic.GetDivisionDropDown();
            return Json(divisionlist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleID"></param>
        /// <returns></returns>
        public JsonResult GetCosheetNoDropDown(int styleID)
        {
            return Json(costSheetLogic.GetCostsheetNoByStyle(styleID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="costsheetNo"></param>
        /// <returns></returns>
        public JsonResult GetCostSheetByCostsheetNo(string costsheetNo)
        {
            return Json(costSheetLogic.GetCostSheetByCostsheetNo(costsheetNo), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemCategoryID"></param>
        /// <returns></returns>
        public JsonResult GetItemDropDown(int itemCategoryID)
        {
            return Json(itemLogic.GetItemDropDown(itemCategoryID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetItemCategoryDropDown()
        {
            return Json(itemCategoryLogic.GetItemCategoryDropDown(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetConsumptionUnitDropDown()
        {
            return Json(consumptionUnitLogic.GetConsumptionUnitDropDown(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleVM"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateStyle(StyleViewModel styleVM)
        {
            try
            {
                Guid userId = userLogic.GetUserID(User.Identity.Name);
                int accountId = userLogic.GetAccountID(userId);
                if (styleLogic.IsUniqueStyle(styleVM.StyleNo.Trim()))
                {
                    styleVM.AccountID = accountId;
                    styleLogic.CreateStyle(styleVM);
                    var result = new { Success = "1", Message = "Style Saved Successfully." };
                    return Json(result);
                }
                else
                {
                    var result = new { Success = "0", Message = "Style No " + styleVM.StyleNo + " already exists." };
                    return Json(result);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", Message = "Style Creation Failed." });
            }

        }

        [HttpPost]
        public JsonResult UpdateStyle(StyleViewModel styleVM)
        {
            try
            {
                Guid userId = userLogic.GetUserID(User.Identity.Name);
                int accountId = userLogic.GetAccountID(userId);

                if (styleLogic.IsUniqueStyle(styleVM.StyleNo.Trim(), styleVM.StyleID))
                {
                    styleVM.AccountID = accountId;
                    styleLogic.UpdateStyle(styleVM);
                    var result = new { Success = "1", Message = "Style Updated Successfully." };
                    return Json(result);
                }
                else
                {
                    var result = new { Success = "0", Message = "Style No " + styleVM.StyleNo + " already exists." };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", Message = "Update Creation Failed." });
            }



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="costSheetVM"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateCostSheet(List<CostsheetViewModel> costSheetVM)
        {

            try
            {
                var result = (from c in costSheetVM
                              group c.ItemID by c.ItemID into g
                              where g.Count() > 1
                              select g.Count()).FirstOrDefault();

                if (result > 0)
                {
                    return Json(new { Success = "2", Message = "Duplicate item is not allowed." });
                }

                string costsheetNo = costSheetLogic.CreateCostsheet(costSheetVM);
                var res = new { Success = "1", Message = "Cost Sheet " + costsheetNo + " Saved Successfully." };
                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "0", Message = "Cost Sheet Creation Failed." });
            }

        }



        //file upload
        [HttpGet]
        [AllowAnonymous]
        [Route("api/documents/upload")]
        public ActionResult Upload(int flowChunkNumber, string flowIdentifier)
        {
            if (commonHelper.ChunkIsHere(flowChunkNumber, flowIdentifier))
            {
                //return Request.CreateResponse(System.Net.HttpStatusCode.OK);
                int statusCode = (int)HttpStatusCode.OK;
                return new HttpStatusCodeResult(statusCode, "OK");

            }
            else
            {
                int id = (int)HttpStatusCode.BadRequest;
                return new HttpStatusCodeResult(id, "Unknown Error");
            }
        }

    }
}