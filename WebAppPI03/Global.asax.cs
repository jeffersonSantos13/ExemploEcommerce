using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using WebAppPI03.Models;
using System.Web.Helpers;
using System.Security.Claims;

namespace WebAppPI03
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<EntidadesEcommerce>(null);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;

            Database.SetInitializer<EntidadesEcommerce>(null);
        }
    }
}
