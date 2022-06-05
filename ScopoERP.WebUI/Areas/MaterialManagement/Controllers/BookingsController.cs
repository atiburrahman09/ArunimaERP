using ScopoERP.LC.BLL;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.MaterialManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.MaterialManagement.Controllers
{
    public class BookingsController : Controller
    {
        private BookingLogic bookingLogic;
        private JobLogic joblogic;
        private ItemCategoryLogic itemCategoryLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingLogic"></param>
        /// <param name="joblogic"></param>
        /// <param name="itemCategoryLogic"></param>
        public BookingsController(BookingLogic bookingLogic, JobLogic joblogic, ItemCategoryLogic itemCategoryLogic)
        {
            this.bookingLogic      = bookingLogic;
            this.joblogic          = joblogic;
            this.itemCategoryLogic = itemCategoryLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.jobList = new SelectList(joblogic.GetJobDropDown(), "Value", "Text");
            ViewBag.itemCategoryList = new SelectList(itemCategoryLogic.GetItemCategoryDropDown(), "Value", "Text");

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobID"></param>
        /// <param name="itemCategoryID"></param>
        /// <returns></returns>
        public ActionResult GetBillOfMaterials(int jobID, int itemCategoryID)
        {
            List<BookingSelectionViewModel> data = bookingLogic.GetBillOfMaterialForBooking(jobID, itemCategoryID);

            return PartialView("~/Areas/MaterialManagement/Views/Bookings/BookingSelection.cshtml", data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bokkingSelectionList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBillOfMaterials(List<BookingSelectionViewModel> bokkingSelectionList)
        {
            return View();
        }
    }
}