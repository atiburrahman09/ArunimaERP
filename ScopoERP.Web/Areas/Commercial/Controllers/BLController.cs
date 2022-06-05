using ScopoERP.Commercial.BLL;
using ScopoERP.Commercial.ViewModel;
using ScopoERP.Common.BLL;
using ScopoERP.Domain.Repositories;
using ScopoERP.LC.BLL;
using ScopoERP.LC.ViewModel;
using ScopoERP.MaterialManagement.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.Web.Areas.Commercial.Controllers
{
    [Authorize(Roles = "Commercial")]    
    public class BLController : Controller
    {
        private BackToBackLCLogic backToBackLCLogic;
        private BLLogic blLogic;
        private PILogic piLogic;
        private BookingLogic bookingLogic;
        private BLDetailsLogic blDetailsLogic;
        private ItemLogic itemLogic;
        private JobLogic jobLogic;

        public BLController(BLLogic blLogic, BackToBackLCLogic backToBackLCLogic, PILogic piLogic, BookingLogic bookingLogic, BLDetailsLogic blDetailsLogic, ItemLogic itemLogic, JobLogic jobLogic)
        {
            this.blLogic = blLogic;
            this.backToBackLCLogic = backToBackLCLogic;
            this.piLogic = piLogic;
            this.bookingLogic = bookingLogic;
            this.blDetailsLogic = blDetailsLogic;
            this.itemLogic = itemLogic;
            this.jobLogic = jobLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetAllBL()
        {
            List<BLViewModel> data = blLogic.GetAllBL();

            return View(new GridModel(data));
        }

        public ActionResult Create()
        {
            ViewBag.BackToBackLC = new SelectList(backToBackLCLogic.GetBackToBackLCDropDown(null), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(BLViewModel blVM)
        {
            if (ModelState.IsValid)
            {
                if (!blLogic.IsUniqueBL(blVM.BLNo.Trim()))
                {
                    ModelState.AddModelError("", blVM.BLNo + " already exists");
                }
                else
                {
                    try
                    {
                        var b2bLC = backToBackLCLogic.GetBackToBackLCByID(blVM.BackToBackLCID);

                        if(b2bLC.LCTypeID != null)
                        {
                            if(b2bLC.LCTypeID == 1 || b2bLC.LCTypeID == 3)
                            {
                                blVM.MaturityDate = blVM.BLDate.Value.AddDays(b2bLC.SightDays ?? 0);
                            }
                            else if (b2bLC.LCTypeID == 2)
                            {
                                blVM.MaturityDate = blVM.AcceptanceDate.Value.AddDays(b2bLC.SightDays ?? 0);
                            }
                        }

                        blVM.IsChalan = false;
                        blLogic.CreateBL(blVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            ViewBag.BackToBackLC = new SelectList(backToBackLCLogic.GetBackToBackLCDropDown(null), "Value", "Text", blVM.BackToBackLCID);

            return View(blVM);
        }

        public ActionResult Edit(int id)
        {
            BLViewModel blVM = blLogic.GetBLByID(id);

            if (blVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.BackToBackLC = new SelectList(backToBackLCLogic.GetBackToBackLCDropDown(null), "Value", "Text", blVM.BackToBackLCID);

            return View(blVM);
        }

        [HttpPost]
        public ActionResult Edit(BLViewModel blVM)
        {
            if (ModelState.IsValid)
            {
                if (!blLogic.IsUniqueBL(blVM.BLNo.Trim(), blVM.BLID))
                {
                    ModelState.AddModelError("", @"This BL No is already exists");
                }
                else
                {
                    try
                    {
                        var b2bLC = backToBackLCLogic.GetBackToBackLCByID(blVM.BackToBackLCID);

                        if (b2bLC.LCTypeID != null)
                        {
                            if (b2bLC.LCTypeID == 1 || b2bLC.LCTypeID == 3)
                            {
                                blVM.MaturityDate = blVM.BLDate.Value.AddDays(b2bLC.SightDays ?? 0);
                            }
                            else if (b2bLC.LCTypeID == 2)
                            {
                                blVM.MaturityDate = blVM.AcceptanceDate.Value.AddDays(b2bLC.SightDays ?? 0);
                            }
                        }

                        blVM.IsChalan = false;
                        blLogic.UpdateBL(blVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }
            }

            ViewBag.BackToBackLC = new SelectList(backToBackLCLogic.GetBackToBackLCDropDown(null), "Value", "Text", blVM.BackToBackLCID);

            return View(blVM);
        }

        public ActionResult AddBLDetails(int id)
        {
            ViewBag.PIDropDown = new SelectList(piLogic.GetPIDropDownByBL(id), "Value", "Text");
            ViewBag.HiddenBLID = id;

            return View();
        }

        //[HttpPost]
        //public ActionResult SaveBLDetails(List<BLDetailsViewModel> blDetailsVMList)
        //{
        //    try
        //    {
        //        if (blDetailsVMList[0].BLDetailsID != 0)
        //        {
        //            blDetailsLogic.UpdateBLDetails(blDetailsVMList);
        //        }
        //        else {
        //            blDetailsLogic.CreateBLDetails(blDetailsVMList);
        //        }
        //        return Json(true);
        //    }
        //    catch (Exception ex)
        //    { 

        //    }
        //    return Json(false);
        //}

        public ActionResult GetItemByPI(int piID) {
            return Json(itemLogic.GetItemDropDownByPI(piID), JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetBLDetails(int blID, int piID, int itemID)
        //{
        //    return PartialView(blDetailsLogic.GetBLDetails(blID, piID, itemID));
        //}



        //-----------New Methods 

        public JsonResult GetJobDropDown()
        {
            var data = jobLogic.GetJobDropDown();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBackToBackLCDropDownByJobID(int jobID)
        {
            var data = backToBackLCLogic.GetBackToBackLCDropDown(jobID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBLAndPIDropDownByBackToBackLCID(int backToBackLCID)
        {
            var BL = blLogic.GetBLDropDownByBackToBackLCID(backToBackLCID);
            var PI = piLogic.GetPIDropDownByBackToBackLCID(backToBackLCID);
            return Json(new {BL = BL, PI = PI }, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult GetBLByJobID(int jobID)
        {
            var data = blLogic.GetAllBL(jobID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBL(BLViewModel blVM)
        {
            if(!ModelState.IsValid)
            {
                return Json(new { Error = "Data model invalid" });
            }
            

            if (blVM.BLID == 0)
            {
                try
                {
                    blLogic.CreateBL(blVM);
                    return Json(new { Success = "Successfully Created!" });
                }
                catch
                {
                    return Json(new { Error = "Can not Create BL" });
                }
            }
            else
            {
                try
                {
                    blLogic.UpdateBL(blVM);
                    return Json(new { Success = "Successfully Updated!" });
                }
                catch
                {
                    return Json(new { Error = "Can not Update BL" });
                }
            }
        }

        public JsonResult GetBLByID(int id)
        {            
            var data = blLogic.GetBLByID(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult GetItemDropDownByPIID(int id)
        {
            var data = itemLogic.GetItemDropDownByPI(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBLDetails(int blID, int piID, int itemID)
        {
            var data = blDetailsLogic.GetBLDetails(blID, piID, itemID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveBLDetails(List<BLDetailsViewModel> blDetailsVMList)
        {
            try
            {
                if (blDetailsVMList[0].BLDetailsID != 0)
                {
                    blDetailsLogic.UpdateBLDetails(blDetailsVMList);
                }
                else
                {
                    blDetailsLogic.CreateBLDetails(blDetailsVMList);
                }
                return Json(new { Success = "Successfully saved!"});
            }
            catch (Exception ex)
            {

            }
            return Json(new { Error = "Failed to save!"});
        }

    }


}