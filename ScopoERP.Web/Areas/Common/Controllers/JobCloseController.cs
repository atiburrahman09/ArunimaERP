using ScopoERP.LC.BLL;
using ScopoERP.LC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.Web.Areas.Common.Controllers
{
    public class JobCloseController : Controller
    {
        private JobLogic jobLogic;

        public JobCloseController(JobLogic jobLogic)
        {
            this.jobLogic = jobLogic;
        }
        // GET: Common/JobClose
        public ActionResult Index()
        {
            List<JobViewModel> data = jobLogic.GetAllJob();
            return View(data);
        }

        public ActionResult CloseJob(int id)
        {

            jobLogic.Closejob(id);
            List<JobViewModel> data = jobLogic.GetAllJob();
            return RedirectToAction("Index");

        }
    }
}
