using ScopoERP.Accounts.BLL;
using ScopoERP.Accounts.ViewModel;
using ScopoERP.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Misc.Controllers
{
    public class BudgetsController : Controller
    {
        private BudgetLogic budgetLogic;
        private ChartOfAccountLogic chartOfAccountLogic;
        private NumberHelper numberHelper;

        public BudgetsController(BudgetLogic _budgetLogic, ChartOfAccountLogic _chartOfAccountLogic)
        {
            this.budgetLogic = _budgetLogic;
            this.chartOfAccountLogic = _chartOfAccountLogic;
            this.numberHelper = new NumberHelper();
        }

        public ActionResult Index()
        {
            var data = budgetLogic.GetAllBudget();

            return View(data);
        }

        public ActionResult Create()
        {
            ViewBag.accounts = new SelectList(chartOfAccountLogic.GetAccountDropDown(), "Value", "Text");
            ViewBag.months = new SelectList(numberHelper.GetAllMonths(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult Create(BudgetViewModel budgetVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    budgetLogic.Create(budgetVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.accounts = new SelectList(chartOfAccountLogic.GetAccountDropDown(), "Value", "Text", budgetVM.ChartOfAccountID);
            ViewBag.months = new SelectList(numberHelper.GetAllMonths(), "Value", "Text", budgetVM.Month);
            return View(budgetVM);
        }

        public ActionResult Edit(int id)
        {
            var data = budgetLogic.GetBudgetByID(id);

            ViewBag.accounts = new SelectList(chartOfAccountLogic.GetAccountDropDown(), "Value", "Text", data.ChartOfAccountID);
            ViewBag.months = new SelectList(numberHelper.GetAllMonths(), "Value", "Text", data.Month);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(BudgetViewModel budgetVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    budgetLogic.Update(budgetVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.accounts = new SelectList(chartOfAccountLogic.GetAccountDropDown(), "Value", "Text", budgetVM.ChartOfAccountID);
            ViewBag.months = new SelectList(numberHelper.GetAllMonths(), "Value", "Text", budgetVM.Month);
            return View(budgetVM);
        }
    }
}