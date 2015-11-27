using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using GerenciamentoHotel.Models;

public static class CustomExtensions
{
    /// <summary>
    /// return image link
    /// </summary>
    /// <param name="helper"></param>
    /// <param name="id">Id of link control</param>
    /// <param name="controller">target controller name</param>
    /// <param name="action">target action name</param>
    /// <param name="strOthers">other URL parts like querystring, etc</param>
    /// <param name="strImageURL">URL for image</param>
    /// <param name="alternateText">Alternate Text for the image</param>
    /// <param name="strStyle">style of the image like border properties,etc</param>
    /// <returns></returns>
    public static MvcHtmlString ImageLink(this HtmlHelper helper, string id, string link, string title, string strImageURL, string alternateText, string strStyle)
    {
        return ImageLink(helper, id, link, title, strImageURL, alternateText, strStyle, null);
    }



    /// <summary>
    /// return image link
    /// </summary>
    /// <param name="helper"></param>
    /// <param name="id">Id of link control</param>
    /// <param name="controller">target controller name</param>
    /// <param name="action">target action name</param>
    /// <param name="strOthers">other URL parts like querystring, etc</param>
    /// <param name="strImageURL">URL for image</param>
    /// <param name="alternateText">Alternate Text for the image</param>
    /// <param name="strStyle">style of the image like border properties,etc</param>
    /// <param name="htmlAttributes">html attribues for link</param>
    /// <returns></returns>
    public static MvcHtmlString ImageLink(this HtmlHelper helper, string id, string link, string title, string strImageURL, string alternateText, string strStyle, object htmlAttributes)
    {
        // Create tag builder
        var builder = new TagBuilder("a");

        // Create valid id
        builder.GenerateId(id);

        // Add attributes
        builder.MergeAttribute("href", link); //form target URL
        builder.MergeAttribute("title", title); //form target URL

        builder.InnerHtml = "<img src='" + strImageURL + "' alt='" + alternateText + "' style=\"" + strStyle + "\">"; //set the image as inner html
        builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
        // Render tag
        return new MvcHtmlString(builder.ToString(TagRenderMode.Normal)); //to add </a> as end tag
    }

    public static MvcHtmlString Image(this HtmlHelper helper, string src, string parameters = "")
    {
        return new MvcHtmlString("<img src=\"" + new UrlHelper(helper.ViewContext.RequestContext).Content(src) + "\" " + parameters + "/>");
    }

    public static MvcHtmlString UserActionLink(this HtmlHelper helper, string linkText, string actionName)
    {
        return UserActionLink(helper, linkText, actionName, null, null, null);
    }

    public static MvcHtmlString UserActionLink(this HtmlHelper helper, string linkText, string actionName, object routeValues)
    {
        return UserActionLink(helper, linkText, actionName, null, routeValues, null);
    }

    public static MvcHtmlString UserActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName)
    {
        return UserActionLink(helper, linkText, actionName, controllerName, null, null);
    }

    public static MvcHtmlString UserActionLink(this HtmlHelper helper, string linkText, string actionName, object routeValues, object htmlAttributes)
    {
        return UserActionLink(helper, linkText, actionName, null, routeValues, htmlAttributes);
    }

    public static MvcHtmlString UserActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, object htmlAttributes)
    {
        return UserActionLink(helper, linkText, actionName, controllerName, null, htmlAttributes);
    }

    public static MvcHtmlString UserActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
    {
        if (AppUser.Authenticated == null)
            throw new InvalidOperationException("User not connected");
        if (string.IsNullOrEmpty(actionName))
            throw new InvalidOperationException("actionName cannot be empty or null");

        if (routeValues != null)
        {
            var ctrl = routeValues.GetType()
                                  .GetProperties()
                                  .ToList()
                                  .FirstOrDefault(x => x.Name == "controller");
            if (ctrl != null)
                controllerName = ctrl.GetValue(routeValues, null).ToString();
            else
                controllerName = helper.ViewContext.RouteData.Values["controller"].ToString();
        }
        else if (string.IsNullOrEmpty(controllerName))
            controllerName = helper.ViewContext.RouteData.Values["controller"].ToString();


        if (AppUser.Authenticated.HasPermission(controllerName))
        {
            if (string.IsNullOrEmpty(linkText))
                linkText = " ";

            return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        return new MvcHtmlString(string.Empty);
    }

    public static MvcHtmlString ResponseEdit(this HtmlHelper helper)
    {
        var _script = string.Empty;

        if (helper.ViewContext.Controller.ViewBag.Error != null)
            _script = "<script type='text/javascript'> new responseEdit({error:true});</script>";
        else if (helper.ViewContext.Controller.ViewBag.Insert != null)
        {
            var update = string.Empty;
            if (helper.ViewContext.Controller.ViewBag.Update != null)
                update = ", update:'" + helper.ViewContext.Controller.ViewBag.Update + "'";

            _script = string.Format("<script type='text/javascript'> new responseEdit({{insert:{0}{1}}});</script>",
                helper.ViewContext.Controller.ViewBag.Insert ? "true" : "false",
                update);
        }

        return new MvcHtmlString(_script);
    }
}
