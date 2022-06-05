using ScopoERP.Commercial.BLL;
using ScopoERP.Common.ViewModel;
using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using ScopoERP.Stackholder.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ScopoERP.Web.Areas.Merchandising.Controllers
{
    [Authorize(Roles = "merchant")]
    public class BookingController : Controller
    {

        private JobLogic _jobLogic;
        private PILogic _piLogic;
        private BackToBackLCLogic _backToBackLCLogic;
        private SupplierLogic _suplierLogic;
        private BookingLogic _bookingLogic;
        private PurchaseOrderLogic _purchaseOrderLogic;
        private SizeColorLogic _sizeColorLogic;
        private WorksheetLogic _woksheetLogic;

        public BookingController(
            JobLogic jobLogic,
            PILogic piLogic,
            BackToBackLCLogic backToBackLCLogic,
            SupplierLogic supplierLogic,
            BookingLogic bookingLogic,
            PurchaseOrderLogic purchaseOrderLogic,
            SizeColorLogic sizeColorLogic,
            WorksheetLogic woksheetLogic
            )
        {
            this._jobLogic = jobLogic;
            this._piLogic = piLogic;
            this._backToBackLCLogic = backToBackLCLogic;
            this._suplierLogic = supplierLogic;
            this._bookingLogic = bookingLogic;
            this._purchaseOrderLogic = purchaseOrderLogic;
            this._sizeColorLogic = sizeColorLogic;
            this._woksheetLogic = woksheetLogic;
        }

        // GET: Merchandising/Booking
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetJobDropDown()
        {
            try
            {
                return Json(_jobLogic.GetJobDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetPIDropDownByJob(int jobId)
        {
            if (jobId < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Job Provided!", JsonRequestBehavior.AllowGet);
            }
            try
            {
                return Json(_piLogic.GetPIDropDownByJob(jobId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetReferenceDropDownByJob(int jobId)
        {
            if (jobId < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Job Provided!", JsonRequestBehavior.AllowGet);
            }
            try
            {
                return Json(_piLogic.GetReferenceDropDownByJob(jobId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPIById(int piId)
        {
            if (piId < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Job Provided!", JsonRequestBehavior.AllowGet);
            }

            try
            {
                return Json(_piLogic.GetPIByID(piId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBackToBackLCByJobId(int jobId)
        {
            if (jobId < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Job Selected!", JsonRequestBehavior.AllowGet);
            }

            try
            {
                return Json(_backToBackLCLogic.GetBackToBackLCDropDown(jobId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSupplierDropDown()
        {
            try
            {
                return Json(_suplierLogic.GetSupplierDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult UpdatePI(PIViewModel piVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                _piLogic.UpdatePI(piVM);
                return Json("Updated Successfully!");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetBookingByPIId(int piId)
        {
            if (piId < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid PI Selected!", JsonRequestBehavior.AllowGet);
            }

            try
            {
                return Json(_bookingLogic.GetBookingByPIID(piId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CreatePI(PIViewModel piVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                _piLogic.CreatePI(piVM);
                return Json("PI Created Successfully!");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult SaveBookings(List<BookingViewModel> bookingList)
        {
            //if (!ModelState.IsValid)
            //{
            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    return Json("Invalid Data Submitted!");
            //}

            //bookingList.ForEach(x => x.SetDate = DateTime.Now);

            try
            {
                if (bookingList != null)
                {
                    var PIID = bookingList[0].PIID;
                    return Json(_bookingLogic.CreateBooking(PIID ?? 0, bookingList), JsonRequestBehavior.AllowGet);
                }
                else { return Json("Invalid data submission."); }

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ReviseBookings(List<BookingViewModel> bookingList)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                _bookingLogic.ReviseBooking(bookingList);
                return Json("Data has been revised.");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetPurchaseOrderListByJob(int jobId)
        {
            try
            {
                return Json(_purchaseOrderLogic.GetPurchaseOrderDropDownByJob(jobId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsSizeColorByPurchaseOrder(int poId)
        {
            try
            {
                return Json(_sizeColorLogic.IsSizeColorExists(poId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //unnecessary
        public JsonResult GetItemList(List<int> poIdList)
        {
            try
            {
                return Json(_bookingLogic.GetItemFromCostSheet(poIdList.ToArray()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFormulaDropDown()
        {
            try
            {
                return Json(_bookingLogic.GetFormulaDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult GetItemByMultiplePO(string poIDs)
        {
            int[] poID = poIDs.Split(',').Select(n => int.Parse(n)).ToArray();

            return Json(_woksheetLogic.GetItemFromWorksheetByPO(poID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBookingList(BookingGenerateViewModel bookingGenerateVM)
        {
            try
            {
                return Json(_bookingLogic.GetBookingByItemFormula(bookingGenerateVM.poList.ToArray(), bookingGenerateVM.itemFormulaList), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeletePI(int PIID)
        {
            _bookingLogic.DeletePI(PIID);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemByAllPO(List<DropDownListViewModel> poList)
        {
            //int[] poID = poIDs.Split(',').Select(n => int.Parse(n)).ToArray();

            return Json(_woksheetLogic.GetItemByAllPO(poList), JsonRequestBehavior.AllowGet);
        }

    }
}