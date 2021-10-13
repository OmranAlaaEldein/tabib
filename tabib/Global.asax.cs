using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
//using tabib.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace tabib
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            createRoles();
        }
        public void createRoles() {
           if (Roles.GetAllRoles().Count() == 0)
                {
                    Roles.CreateRole("adminstration");
                    Roles.CreateRole("patient");
                    Roles.CreateRole("Dotor");

                    //WebSecurity.InitializeDatabaseConnection("TabibConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    //WebSecurity.CreateUserAndAccount("admin", "admin");
                    //Roles.AddUserToRole("admin", "adminstration");

                   
           }
          
        }
    }
}