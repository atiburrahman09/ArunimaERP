using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.Domain.Repositories;
using ScopoERP.MaterialManagement.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Store.Controllers
{
    public class InventoryController : Controller
    {
        private UnitOfWork unitOfWork;
        private BLDetailsLogic blDetailsLogic;
        private BLLogic blLogic;
        private PILogic piLogic;

        public InventoryController(UnitOfWork unitOfWork, BLDetailsLogic blDetailsLogic, BLLogic blLogic, PILogic piLogic)
        {
            this.unitOfWork = unitOfWork;
            this.blDetailsLogic = blDetailsLogic;
            this.blLogic = blLogic;
            this.piLogic = piLogic;
        }

        public ActionResult Index()
        {
            ViewBag.PI = new SelectList(piLogic.GetPIDropDown(), "Value", "Text");

            return View();
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

        public ActionResult GetBLDetails(int piID, int blID, int itemID)
        {
            return PartialView("_BLDetails", blDetailsLogic.GetBLDetailsForInventory(piID, blID, itemID));
        }

        [HttpPost]
        public ActionResult SaveInventory(List<BLDetailsViewModel> blDetailsVM)
        {
            try
            {
                blDetailsLogic.UpdateBLDetails(blDetailsVM);
                return Json(true);
            }
            catch (Exception ex) {
                return Json(false);
            }
            
        }
    }
}