using ScopoERP.Accounts.BLL;
using ScopoERP.Accounts.ViewModel;
using ScopoERP.Common.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Accounts.Controllers
{
    [Authorize]
    public class ProcurementController : Controller
    {
        private ProcurementLogic procurementLogic;
        private StatusLogic statusLogic;

        public ProcurementController(ProcurementLogic procurementLogic, StatusLogic statusLogic)
        {
            this.procurementLogic = procurementLogic;
            this.statusLogic      = statusLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int? id = null)
        {
            ViewBag.statusList = new SelectList(statusLogic.GetProcurementStatusDropDown(), "Value", "Text");
            ViewBag.ProcurementID = id;

            ProcurementViewModel procureVM;

            if(id != null)
            {
                procureVM = procurementLogic.GetByID(id ?? 0);
            }
            else
            {
                procureVM = new ProcurementViewModel();
                procureVM.AssignedDate = DateTime.Now;
            }

            return View(procureVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procurementVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ProcurementViewModel procurementVM)
        {
            if(ModelState.IsValid)
            {
                if(procurementVM.ProcurementID == 0)
                {
                    procurementLogic.Create(procurementVM);
                }
                else
                {
                    procurementLogic.Update(procurementVM);
                }
            }

            ViewBag.statusList = new SelectList(statusLogic.GetProcurementStatusDropDown(), "Value", "Text", procurementVM.Status);
            ViewBag.ProcurementID = procurementVM.ProcurementID;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ViewResult GetAllProcurement()
        {
            return View(new GridModel(procurementLogic.GetAll()));
        }
    }
}