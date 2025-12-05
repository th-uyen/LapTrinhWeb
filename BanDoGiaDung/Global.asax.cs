using BanDoGiaDung.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace BanDoGiaDung
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            

        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    var authTicket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);

                    var roles = authTicket.UserData.Split(',');

                    var userPrincipal = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authTicket.Name), roles);
                    HttpContext.Current.User = userPrincipal;
                }
                catch
                {
                }
            }
        }

    }
}
