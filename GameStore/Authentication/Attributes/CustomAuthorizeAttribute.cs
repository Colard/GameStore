using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = "NotAuthorized",
                        LoginUrl = urlHelper.Action("Login", "Account", new { ReturnUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery })
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                return;
            }

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
                filterContext.Result = routeData;
            }

        }

    }
}