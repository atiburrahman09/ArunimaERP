using ScopoERP.OrderManagement.ViewModel;
using ScopoERP.WebUI.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.OrderManagement.Controllers
{
    public class StyleImageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Upload(StyleImageViewModel styleImageVM)
        {
            if (ModelState.IsValid)
            {
                if (styleImageVM.Attachment == null)
                {
                    ModelState.AddModelError("Attachment", "Please upload a image");
                }
                else
                {
                    if (!ImageHelper.IsImage(styleImageVM.Attachment))
                    {
                        ModelState.AddModelError("Attachment", "Please upload a valid image file");
                    }
                    else
                    {
                        string extension = Path.GetExtension(styleImageVM.Attachment.FileName);
                        string fileName = System.Guid.NewGuid().ToString("N") + extension;

                        styleImageVM.ImageUrl = Server.MapPath("~/Content/UploadedImages/" + fileName);

                        //TODO: insert into database
                        
                        return Json(true, JsonRequestBehavior.DenyGet);
                    }
                }
            }
            return Json(false, JsonRequestBehavior.DenyGet);
        }
    }
}