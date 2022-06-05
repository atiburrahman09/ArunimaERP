using CodeWarriors.API.App_Start;
using ScopoERP.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ScopoERP.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //GlobalFilters.Filters.Add(new MinifyHtmlFilterAttribute());

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SimpleInjectorWebApiInitializer.Initialize();
        }
    }
}
