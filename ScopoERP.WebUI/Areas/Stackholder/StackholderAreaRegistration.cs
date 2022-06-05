using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.Stackholder
{
    public class StackholderAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Stackholder";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Stackholder_default",
                "Stackholder/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}