using System;
using System.Web.Mvc;

namespace XcConnect
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string controller2 = null, string controller3 = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass)) 
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = string.Empty;

            if (String.IsNullOrEmpty(controller2))
                controller2 = string.Empty;

            if (String.IsNullOrEmpty(controller3))
                controller3 = string.Empty;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return (controller == currentController || controller2 == currentController || controller3 == currentController) && action == currentAction ? cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

	}
}
