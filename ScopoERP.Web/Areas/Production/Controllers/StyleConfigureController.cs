using ScopoERP.Common.ViewModel;
using ScopoERP.Domain.Models;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using ScopoERP.Production.BLL;
using ScopoERP.Production.ViewModel;
using ScopoERP.ProductionStatus.BLL;
using ScopoERP.ProductionStatus.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.Stackholder.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Production.Controllers
{
    [Authorize(Roles = "Production")]
    public class StyleConfigureController : Controller
    {
        private BuyerLogic buyerLogic;
        private StyleLogic styleLogic;        
        private MachineLogic machineLogic;        
        private StyleOperationLogic styleOperationLogic;
        private StandardOperationLogic standardOperationLogic;
        private SpecLogic specLogic;


        public StyleConfigureController
            (
                BuyerLogic buyerLogic, 
                StyleLogic styleLogic, 
                MachineLogic machineLogic, 
                StyleOperationLogic styleOperationLogic,
                StandardOperationLogic standardOperationLogic,
                SpecLogic specLogic
            )
        {
            this.buyerLogic = buyerLogic;
            this.styleLogic = styleLogic;            
            this.machineLogic = machineLogic;            
            this.styleOperationLogic = styleOperationLogic;
            this.standardOperationLogic = standardOperationLogic;
            this.specLogic = specLogic;
        }

        // GET: Production/StyleConfigure
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getAllBuyer()
        {
            List<BuyerViewModel> buyerList = buyerLogic.GetAllBuyer();
            return Json(buyerList, JsonRequestBehavior.AllowGet);
        }
                
        public JsonResult GetAllStyle(int accountId, int buyerId)
        {
            List<StyleViewModel> stylelist = styleLogic.GetAllStyleByBuyer(accountId, buyerId);
            return Json(stylelist, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SaveStyleOperation(List<StyleOperationViewModel> operationList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    styleOperationLogic.createStyleOperation(operationList);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json("Data Saved Successfully.");
                }catch(Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    return Json(e.Message);
                }
            }
            Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
            return Json("ModelState Invalid!");
        }

        [HttpGet]
        public JsonResult GetStyleOperationListByStyleID(int styleId)
        {
            List<StyleOperationViewModel> styleOperationList = styleOperationLogic.GetStyleOperationListByStyleID(styleId);
            return Json(styleOperationList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMachineDropDown()
        {
            List<DropDownListViewModel> machineList = machineLogic.GetMachineDropDown();
            return Json(machineList, JsonRequestBehavior.AllowGet);
        }

        //Save Operation
        [HttpGet]
        public JsonResult GetAllStandardOperations()
        {
            return Json(standardOperationLogic.GetAllStandardOperations(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSpec()
        {
            return Json(specLogic.GetSpec(), JsonRequestBehavior.AllowGet);
        }



    }
}