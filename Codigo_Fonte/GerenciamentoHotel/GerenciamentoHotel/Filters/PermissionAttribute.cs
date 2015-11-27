using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GerenciamentoHotel.Models;

namespace Timesheet.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PermissionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            if (AppUser.Authenticated != null)
            {
                if (!AppUser.Authenticated.HasPermission(controller))
                    NewRoute(filterContext);
            }
            else
            {
                NewRoute(filterContext);
            }
        }

        private static void NewRoute(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "AccessDenied" }));
            }
            else
            {
                filterContext.Controller.TempData["denied"] = 1;

                if (filterContext.RequestContext.HttpContext.Request.UrlReferrer == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                    return;
                }

                var routeReferrer = ExtractRoute(filterContext);

                filterContext.Result = new RedirectToRouteResult(routeReferrer.Values);
            }
        }

        private static RouteData ExtractRoute(ActionExecutingContext filterContext)
        {
            string url = null;
            string query = null;
            string referrer = filterContext.RequestContext.HttpContext.Request.UrlReferrer.AbsoluteUri;

            if (referrer.IndexOf("?") != -1)
            {
                url = referrer.Remove(referrer.IndexOf("?"));
                query = referrer.Substring(referrer.IndexOf("?") + 1);
            }
            else
                url = referrer;

            var request = new HttpRequest(null, url, query);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);
            var routeReferrer = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            return routeReferrer;
        }
    }
}
