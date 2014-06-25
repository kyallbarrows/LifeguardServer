using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Parichay.Security;

namespace Parichay.MVC
{
    /// <summary>
    /// HTTPmodule which intervenes to the normal process execution once "AuthorizeRequest" event is thrown
    /// and then  validates the permissions for the particular (Request.Location) against the Permissions configured by us into Permissions table
    /// </summary>
    public class ADHPermissionsModuleMVC : IHttpModule
    {
        public void Dispose()
        {

        }

        /// <summary>
        /// Registers the EventHandler (CheckPermissions) to the AuthorizeRequest event at the Init event of the request.
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.AuthorizeRequest += new EventHandler(CheckPermissions);
        }

        /// <summary>
        /// Here we perform the Validatiion for the current request.
        /// If any of the exclusive access has been configured into the permissions table for the specified location (Folder/File.aspx),
        /// then only the roles permitted in the Rule are allowed access. All other Users are denied access.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void CheckPermissions(object sender, EventArgs args)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            RouteData routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));

            string controllerName = "", actionName = "";

            if (routeData != null)
            {
                controllerName = routeData.Values["controller"].ToString();
                actionName = routeData.Values["action"].ToString();
            }
            else { return; }

            HttpRequest httpReq = context.Request;
            HttpResponse httpResp = context.Response;


            if (httpReq.IsAuthenticated == false)
            {
                //Utilities.TextLog("User is unauthenticated. Exiting Authorization.");
                return;
            }


            //Only if the current User Is authenticated perform the custom authorization.
            //If the current user is not authenticated, custom security stays Idle.
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                bool HasUserPermission = false;
                //Utilities.TextLog("Checking permissions for folder: " + reqFolder + ". And File: " + reqFile);


                string[] permissions = ADHSecurityHelper.GetRolesForControllerAction(context.User.Identity.Name, out HasUserPermission, controllerName, actionName);


                //If neither the user has a User Level Permission to the resource
                //And also the Configured Permissions for the resource are not empty.
                //Then check for Role Level Access
                if ((!HasUserPermission) && (permissions.Length != 0))
                    if (!((permissions.Length == 1) && (string.IsNullOrEmpty(permissions[0]))))  //Sometimes a singular string with blank value was returned.
                    {
                        //If User has none of the Allowed Roles for a particular resource. Access will be denied. 
                        if (!MatchUserRolesToPermissions(HttpContext.Current.User, permissions))
                        {
                            string message = string.Format("User {0} does not have permission to access the Action Name: {1} located in Controller {2}"
                                , HttpContext.Current.User.Identity.Name, actionName, controllerName);

                            System.Diagnostics.Trace.TraceInformation(message);

                            //.TextLog(message);

                            throw new System.Security.SecurityException(message);
                        }
                    }
            }
        }

        /// <summary>
        /// If any of the roles for the current user match with the Allowed roles for a particular resource (Folder\File.aspx) the user is allowed access.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">List of allowed Roles (configured for a particular resource{Folder\File.aspx}) to be compared with Input user's roles.</param>
        /// <returns>
        /// 	<c>true</c> if any of User's roles match with Allowed roles for a particular resource; otherwise, <c>false</c>.
        /// </returns>
        private bool MatchUserRolesToPermissions(System.Security.Principal.IPrincipal user, string[] roles)
        {
            if (user == null)
                return false;

            foreach (string role in roles)
            {
                if (user.IsInRole(role))
                {
                    return true;
                }
            }

            return false;
        }

    }
}