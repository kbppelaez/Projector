using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Text;

namespace Projector.Controllers
{
    public static class ControllerExtensions
    {
        public static string GetRouteAbsoluteUrl(this ControllerBase controllerBase, string action, string controller, object routeValues = null)
        {
            var url = new StringBuilder();
            url.Append(controllerBase.Request.Scheme + "://");
            url.Append(controllerBase.Request.Host.Host + ":");
            url.Append(controllerBase.Request.Host.Port);
            url.Append(controllerBase.Url.Action(action, controller, routeValues));

            return url.ToString();
        }
    }
}
