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
    public class ExpensesController : Controller
    {
        private ExpenseLogic expenseLogic;
        private ChartOfAccountLogic chartOfAccountLogic;

        public ExpensesController(ExpenseLogic _expenseLogic, ChartOfAccountLogic _chartOfAccountLogic)
        {
            this.expenseLogic = _expenseLogic;
            this.chartOfAccountLogic = _chartOfAccountLogic;
        }

        public ActionResult Index()
        {
            var data = expenseLogic.GetAllExpense();

            return View(data);
        }

        public ActionResult Create()
        {
            ViewBag.accounts = new SelectList(chartOfAccountLogic.GetAccountDropDown(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult Create(ExpenseViewModel expenseVM)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    expenseLogic.Create(expenseVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.accounts = new SelectList(chartOfAccountLogic.GetAccountDropDown(), "Value", "Text", expenseVM.ChartOfAccountID);
            return View(expenseVM);
        }

        public ActionResult Edit(int id)
        {
            var data = expenseLogic.GetExpenseByID(id);

            ViewBag.accounts = new SelectList(chartOfAccountLogic.GetAccountDropDown(), "Value", "Text", data.ChartOfAccountID);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(ExpenseViewModel expenseVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    expenseLogic.Update(expenseVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.accounts = new SelectList(chartOfAccountLogic.GetAccountDropDown(), "Value", "Text", expenseVM.ChartOfAccountID);
            return View(expenseVM);
        }
    }
}