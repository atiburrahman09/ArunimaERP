using ScopoERP.Common.ViewModel;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Stackholder.BLL;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.MaterialManagement.Controllers
{
    [Authorize(Roles = "Merchant")]
    public class BookingController : Controller
    {
        private StyleLogic styleLogic;
        private PILogic piLogic;
        private BookingLogic bookingLogic;
        private PurchaseOrderLogic purchaseOrderLogic;
        private WorksheetLogic woksheetLogic;
        private SupplierLogic supplierLogic;

        public BookingController(StyleLogic styleLogic, PILogic piLogic, WorksheetLogic woksheetLogic, SupplierLogic supplierLogic,
                                    BookingLogic bookingLogic, PurchaseOrderLogic purchaseOrderLogic)
        {
            this.styleLogic = styleLogic;
            this.piLogic = piLogic;
            this.woksheetLogic = woksheetLogic;
            this.supplierLogic = supplierLogic;
            this.bookingLogic = bookingLogic;
            this.purchaseOrderLogic = purchaseOrderLogic;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult GetPIDropDown() 
        {
            return Json(piLogic.GetPIDropDown(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMultipleStyleDropDown() 
        {
            return Json(styleLogic.GetStyleDropDown(CurrentUser.AccountID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMultiplePODropDownByMultipleStyles(string styleIDs) 
        {
            int[] styleID = styleIDs.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            return Json(purchaseOrderLogic.GetPurchaseOrderDropDown(styleID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemByMultiplePO(string poIDs)
        {
            int[] poID = poIDs.Split(',').Select(n => int.Parse(n)).ToArray();

            return Json(woksheetLogic.GetItemFromWorksheetByPO(poID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBookingByPIID(int piID) 
        {
            return Json(bookingLogic.GetBookingByPIID(piID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReviseBooking(List<BookingViewModel> bookingViewModels)
        {
            try
            {
                bookingViewModels.ForEach(x => { x.UserID = CurrentUser.UserID; x.SetDate = DateTime.Now; });

                bookingLogic.ReviseBooking(bookingViewModels);

                return Json(bookingLogic.GetBookingByPIID((int)bookingViewModels[0].PIID));
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        public ActionResult GetBookingFromWorksheet(string poStyleIDs, int itemID)
        {
            int[] postyleID = poStyleIDs.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            return Json(bookingLogic.GetBookingFromWorksheet(postyleID, itemID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateBooking(List<BookingViewModel> bookingVMs)
        {
            try
            {
                int? piID = bookingVMs[0].PIID;

                bookingVMs.ForEach(x => { x.UserID = CurrentUser.UserID; x.SetDate = DateTime.Now; });
                string referenceNo = bookingLogic.CreateBooking(piID, bookingVMs);

                return Json(new { success = true, successMsg = "Booking is successfully created with "+referenceNo+" refernce no" });
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        public ActionResult GetSupplierDropDown()
        {
            var result = supplierLogic.GetSupplierDropDown();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReferenceNoDropDown()
        {
            var result = piLogic.GetReferenceDropDown();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnmapBooking(int BookingID)
        {
            try
            {
                bookingLogic.UnmapBooking(BookingID);
                return Json(new { success = true, successMsg = "Item is deleted successfully from booking" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}