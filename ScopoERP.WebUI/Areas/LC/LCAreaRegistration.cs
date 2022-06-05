using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.LC
{
    public class LCAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LC_default",
                "LC/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}