using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Utils.Controllers
{
    public class MailController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SendMail(MailViewModel mailVM)
        {
            MailHelper mailHelper = new MailHelper();
            await mailHelper.SendMessage("jamir@arunima-group.net", "Test", "Test");

            return RedirectToAction("Index");
        }
    }
}