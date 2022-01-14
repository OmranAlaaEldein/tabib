using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TabibV1.OtherClass
{
    public class Log: ActionFilterAttribute
    {
          public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        myLog("OnActionExecuted", filterContext.RouteData);
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        myLog("OnActionExecuting", filterContext.RouteData);
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        myLog("OnResultExecuted", filterContext.RouteData);
    }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        myLog("OnResultExecuting ", filterContext.RouteData);
    }

    private void myLog(string methodName, RouteData routeData)
    {
        var controllerName = routeData.Values["controller"];
        var actionName = routeData.Values["action"];
        var message = String.Format("{0}- controller:{1} action:{2}", methodName,
                                                                    controllerName,
                                                                    actionName);
        Debug.WriteLine(message);
    }
    }
}
