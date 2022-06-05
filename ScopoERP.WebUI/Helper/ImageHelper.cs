using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScopoERP.WebUI.Helper
{
    public class ImageHelper
    {
        public static bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}