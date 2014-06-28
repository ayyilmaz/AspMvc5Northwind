using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspMvc5Northwind.Models;

namespace AspMvc5Northwind
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DbInterception.Add(new NorthwindInterceptorLogging());

            // Uncomment the following line to throw fake SQL errors when 
            // the search term "Throw" is entered.
            // DbInterception.Add(new NorthwindInterceptorTransientErrors());
        }
    }
}
