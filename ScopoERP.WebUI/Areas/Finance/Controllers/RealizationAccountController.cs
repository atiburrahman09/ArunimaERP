using ScopoERP.Finance.BLL;
using ScopoERP.Finance.ViewModel;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Finance.Controllers
{
    public class RealizationAccountController : Controller
    {
        private RealizationAccountLogic realizationAccountLogic;

        public RealizationAccountController(RealizationAccountLogic realizationAccountLogic)
        {
            this.realizationAccountLogic = realizationAccountLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ViewResult GetAllRealizationAccount()
        {
            List<RealizationAccountViewModel> realizationAccountVM = realizationAccountLogic.GetAllRealizationAccount();

            return View(new GridModel(realizationAccountVM));
        }

        public ActionResult Create()
        {
            ViewBag.accountTypeList = new SelectList(realizationAccountLogic.GetRealizationAccountTypeDropDown(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(RealizationAccountViewModel realizationAccountVM)
        {
            if (ModelState.IsValid)
            {
                if (!realizationAccountLogic.IsUniqueRealizationAccount(realizationAccountVM.RealizationAccountNo.Trim(),
                    realizationAccountVM.RealizationAccountName.Trim()))
                {
                    ModelState.AddModelError("", realizationAccountVM.RealizationAccountNo + " already exists");
                }
                else
                {
                    try
                    {
                        realizationAccountVM.UserID = CurrentUser.UserID;
                        realizationAccountVM.SetDate = DateTime.Now;
                        realizationAccountLogic.CreateRealizationAccount(realizationAccountVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            ViewBag.accountTypeList = new SelectList(realizationAccountLogic.GetRealizationAccountTypeDropDown(), "Value", "Text", realizationAccountVM.RealizationAccountType);

            return View(realizationAccountVM);
        }

        public ActionResult Edit(int id)
        {
            RealizationAccountViewModel realizationAccountVM = realizationAccountLogic.GetRealizationAccountByID(id);

            if (realizationAccountVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            ViewBag.accountTypeList = new SelectList(realizationAccountLogic.GetRealizationAccountTypeDropDown(), "Value", "Text", realizationAccountVM.RealizationAccountType);

            return View(realizationAccountVM);
        }

        [HttpPost]
        public ActionResult Edit(RealizationAccountViewModel realizationAccountVM)
        {
            if (ModelState.IsValid)
            {

                if (!realizationAccountLogic.IsUniqueRealizationAccount(realizationAccountVM.RealizationAccountNo.Trim(),
                    realizationAccountVM.RealizationAccountName.Trim(), realizationAccountVM.RealizationAccountID))
                {
                    ModelState.AddModelError("", @"This RealizationAccount No is already exists");
                }
                else
                {
                    try
                    {
                        realizationAccountVM.UserID = CurrentUser.UserID;
                        realizationAccountVM.SetDate = DateTime.Now;
                        realizationAccountLogic.UpdateRealizationAccount(realizationAccountVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }
            }
            ViewBag.accountTypeList = new SelectList(realizationAccountLogic.GetRealizationAccountTypeDropDown(), "Value", "Text", realizationAccountVM.RealizationAccountType);

            return View(realizationAccountVM);
        }
    }
}