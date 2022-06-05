using ScopoERP.Accounts.BLL;
using ScopoERP.Accounts.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Misc.Controllers
{
    public class ChartOfAccountsController : Controller
    {
        private ChartOfAccountLogic chartOfAccountLogic;

        public ChartOfAccountsController(ChartOfAccountLogic _chartOfAccountLogic)
        {
            chartOfAccountLogic = _chartOfAccountLogic;
        }

        public ActionResult Index()
        {
            var data = chartOfAccountLogic.GetAllAccounts();

            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ChartOfAccountViewModel chartOfAccountVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    chartOfAccountLogic.Create(chartOfAccountVM);

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(chartOfAccountVM);
                }
            }
            return View(chartOfAccountVM);
        }

        public ActionResult Edit(int id)
        {
            var data = chartOfAccountLogic.GetAccountByID(id);

            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(ChartOfAccountViewModel chartOfAccountVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    chartOfAccountLogic.Update(chartOfAccountVM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(chartOfAccountVM);
                }
            }
            return View(chartOfAccountVM);
        }
    }
}