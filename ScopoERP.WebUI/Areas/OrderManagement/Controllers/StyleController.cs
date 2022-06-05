using ScopoERP.Common.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.OrderManagement.ViewModel;
using ScopoERP.Stackholder.BLL;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.OrderManagement.Controllers
{
    [Authorize(Roles = "merchant")]
    public class StyleController : Controller
    {
        private StyleLogic styleLogic;
        private BuyerLogic buyerLogic;
        private CustomerLogic customerLogic;
        private DivisionLogic divisionLogic;

        public StyleController(StyleLogic styleLogic, BuyerLogic buyerLogic, CustomerLogic customerLogic, DivisionLogic divisionLogic)
        {
            this.styleLogic = styleLogic;
            this.buyerLogic = buyerLogic;
            this.customerLogic = customerLogic;
            this.divisionLogic = divisionLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetAllStyle()
        {
            List<StyleViewModel> data = styleLogic.GetAllStyle(CurrentUser.AccountID);

            return View(new GridModel(data));
        }

        public ActionResult Create()
        {
            ViewBag.Buyer = new SelectList(buyerLogic.GetBuyerDropDown(), "Value", "Text");
            ViewBag.Customer = new SelectList(customerLogic.GetCustomerDropDown(), "Value", "Text");
            ViewBag.Division = new SelectList(divisionLogic.GetDivisionDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(StyleViewModel styleVM)
        {
            if (ModelState.IsValid)
            {
                if (!styleLogic.IsUniqueStyle(styleVM.StyleNo.Trim()))
                {
                    ModelState.AddModelError("", styleVM.StyleNo + " already exists");
                }
                else
                {
                    try
                    {
                        styleVM.AccountID = CurrentUser.AccountID;
                        styleLogic.CreateStyle(styleVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            ViewBag.Buyer = new SelectList(buyerLogic.GetBuyerDropDown(), "Value", "Text", styleVM.BuyerID);
            ViewBag.Customer = new SelectList(customerLogic.GetCustomerDropDown(), "Value", "Text", styleVM.CustomerID);
            ViewBag.Division = new SelectList(divisionLogic.GetDivisionDropDown(), "Value", "Text", styleVM.DivisionID);

            return View(styleVM);
        }

        public ActionResult Edit(int id)
        {
            StyleViewModel styleVM = styleLogic.GetStyleByID(id);

            if (styleVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Buyer = new SelectList(buyerLogic.GetBuyerDropDown(), "Value", "Text", styleVM.BuyerID);
            ViewBag.Customer = new SelectList(customerLogic.GetCustomerDropDown(), "Value", "Text", styleVM.CustomerID);
            ViewBag.Division = new SelectList(divisionLogic.GetDivisionDropDown(), "Value", "Text", styleVM.DivisionID);

            return View(styleVM);
        }

        [HttpPost]
        public ActionResult Edit(StyleViewModel styleVM)
        {
            if (ModelState.IsValid)
            {
                if (!styleLogic.IsUniqueStyle(styleVM.StyleNo.Trim(), styleVM.StyleID))
                {
                    ModelState.AddModelError("", @"This Style No is already exists");
                }
                else
                {
                    try
                    {
                        styleVM.AccountID = CurrentUser.AccountID;
                        styleLogic.UpdateStyle(styleVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }
            }

            ViewBag.Buyer = new SelectList(buyerLogic.GetBuyerDropDown(), "Value", "Text", styleVM.BuyerID);
            ViewBag.Customer = new SelectList(customerLogic.GetCustomerDropDown(), "Value", "Text", styleVM.CustomerID);
            ViewBag.Division = new SelectList(divisionLogic.GetDivisionDropDown(), "Value", "Text", styleVM.DivisionID);

            return View(styleVM);
        }

        public ActionResult Copy(int id)
        {
            StyleViewModel styleVM = styleLogic.GetStyleByID(id);

            if (styleVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }

            ViewBag.Buyer = new SelectList(buyerLogic.GetBuyerDropDown(), "Value", "Text", styleVM.BuyerID);
            ViewBag.Customer = new SelectList(customerLogic.GetCustomerDropDown(), "Value", "Text", styleVM.CustomerID);
            ViewBag.Division = new SelectList(divisionLogic.GetDivisionDropDown(), "Value", "Text", styleVM.DivisionID);

            return View("Create", styleVM);
        }
    }
}