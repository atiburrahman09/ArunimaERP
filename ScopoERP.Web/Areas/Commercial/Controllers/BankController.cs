using ScopoERP.Domain.Repositories;
using ScopoERP.LC.BLL;
using ScopoERP.LC.ViewModel;
using ScopoERP.Web.Helper;
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
   
    public class BankController : Controller
    {
        private BankLogic bankLogic;

        public BankController(BankLogic bankLogic)
        {
            this.bankLogic = bankLogic;
        }

        public ActionResult Index()
        {
            List<BankViewModel> data = bankLogic.GetAllBank();

            return View(data);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BankViewModel bankVM)
        {
            if (ModelState.IsValid)
            {
                if (!bankLogic.IsUniqueBank(bankVM.BankName.Trim()))
                {
                    ModelState.AddModelError("", bankVM.BankName + " already exists");
                }
                else
                {
                    try
                    {
                        bankVM.UserID = CurrentUser.UserID;
                        bankLogic.CreateBank(bankVM);

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            return View(bankVM);
        }

        public ActionResult Edit(int id)
        {
            BankViewModel bankVM = bankLogic.GetBankByID(id);

            if (bankVM == null)
            {
                //it will actually return to 404 page
                return RedirectToAction("NotFound404", "Error");
            }
            return View(bankVM);
        }

        [HttpPost]
        public ActionResult Edit(BankViewModel bankVM)
        {
            if (ModelState.IsValid)
            {
                if (!bankLogic.IsUniqueBank(bankVM.BankName.Trim(), bankVM.BankID))
                {
                    ModelState.AddModelError("", @"This Bank No is already exists");
                }
                else
                {
                    try
                    {
                        bankVM.UserID = CurrentUser.UserID;
                        bankLogic.UpdateBank(bankVM);

                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", @"Unable to save changes. Try again, and if 
                                        the problem persists, Contact with Entitas Technologia.");
                    }
                }
            }
            return View(bankVM);
        }
    }
}