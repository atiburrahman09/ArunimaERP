using System.Web.Mvc;

namespace ScopoERP.WebUI.Areas.MaterialManagement
{
    public class MaterialManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MaterialManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MaterialManagement_default",
                "MaterialManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}