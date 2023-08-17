using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GameStore.Authentication.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (CurrentUser == null) return false;
           
            if (String.IsNullOrWhiteSpace(Roles)) return true;

            string[] rolesList = Roles.Replace(" ", "").Split(',');
            foreach (string role in rolesList)
            {
                if (CurrentUser.IsInRole(Roles)) return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;
            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        controller = "Account",
                        action = "Login",
                        ReturnUrl = HttpContext.Current.Request.Url.AbsolutePath
                    }
                    )) ;
            }
            else
            {
                routeData = new RedirectToRouteResult
                (new System.Web.Routing.RouteValueDictionary
                 (new
                 {
                     controller = "Error",
                     action = "AccessDenied"
                 }
                 ));
            }

            filterContext.Result = routeData;
        }

    }
}