using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Parichay.MVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }


        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[
                     System.Web.Security.FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                //Extract the forms authentication cookie
                System.Web.Security.FormsAuthenticationTicket authTicket =
                       System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);

                string[] udata = authTicket.UserData.Split(new char[] { ';' });

                string usrEmail = "";
                int usrKey = 0;
                string roleStr = "";

                for (int count1 = 0; count1 <= udata.Length - 1; count1++)
                {
                    if (udata[count1].Contains("Email="))
                        usrEmail = udata[count1].Replace("Email=", "");

                    if (udata[count1].Contains("UKeyN="))
                        usrKey = Convert.ToInt32(udata[count1].Replace("UKeyN=", ""));

                    if (udata[count1].Contains("roleStr="))
                        roleStr = udata[count1].Replace("roleStr=", "");
                }

                // Create an Identity object
                //CustomIdentity implements System.Web.Security.IIdentity
                Models.CustomIdentity id = new Models.CustomIdentity(usrKey, authTicket.Name, usrEmail);
  



                Context.User = new System.Security.Principal.GenericPrincipal(id, roleStr.Split(','));
            }
        }

        //protected void Application_BeginRequest()
        //{
        //    //start new NHibernate session on each web request
        //    var session = persistanceManager.OpenSession();
        //    //bind session to the thread so all the code can access it using SessionFactory.GetCurrentSession()
        //    //this relies on "current_session_context_class" property set to "web" in NHibernate.config
        //    CurrentSessionContext.Bind(session);
        //}

        //protected void Application_EndRequest()
        //{
        //    //unbind from the thread
        //    //no need to close the session as it is already automatically closed at this point (not sure why)
        //    CurrentSessionContext.Unbind(SessionFactory);
        //}

        //protected void Application_OnEnd()
        //{
        //    //dispose Castle container and all the stuff it contains
            
        //}

        protected void Application_Error()
        {
            //if there is an exception log it using log4Net

            // At this point we have information about the error
            HttpContext ctx = HttpContext.Current;

            Exception exception = ctx.Server.GetLastError();

            //string errorInfo =
            //   "<br>Offending URL: " + ctx.Request.Url.ToString() +
            //   "<br>Source: " + exception.Source +
            //   "<br>Message: " + exception.Message +
            //   "<br>Stack trace: " + exception.StackTrace;


            try
            {
                if (exception.Message.Contains("File does not exist"))
                {
                    exception = new Exception("Error reading file " + HttpContext.Current.Request.FilePath, exception);
                }

                Parichay.Data.Helper.NHibernateHelper.Log(exception);
            }
            catch
            {
                //throw;
                //TempData["message"] = "Exception:" + exc.Message;
            }

            ////Check if the exception type is Security Exception
            //if (exception is System.Security.SecurityException)
            //{
            //    ctx.Server.ClearError();
            //    Response.Redirect("~/Unauthorized?p=" + Server.UrlEncode(Request.RawUrl));
            //}



        }

    }
}