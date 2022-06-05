using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ScopoERP.WebUI.Helper
{
    public class CurrentUser
    {
        public static int UserID
        {
            get
            {
                return 0; //Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            }
        }

        public static int AccountID
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["AccountID"]);
            }
        }
    }
}
