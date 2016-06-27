using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CosmeticMVC
{
    public class BaseController : Controller
    {
        
        //initilizing culture on controller initialization
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    //base.Initialize(requestContext);
        //    //if (Session[CommonConstant.CurrentCulture] != null)
        //    //{
        //    //    Thread.CurrentThread.CurrentCulture = new CultureInfo(Session[CommonConstant.CurrentCulture].ToString());
        //    //    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session[CommonConstant.CurrentCulture].ToString());
        //    //}
        //    //else
        //    //{
        //    //    Session[CommonConstant.CurrentCulture] = "vi";
        //    //    Thread.CurrentThread.CurrentCulture = new CultureInfo("vi");
        //    //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi");
        //    //}
        //}

        //// changing culture
        //public ActionResult ChangeCulture(string ddlCulture, string returnUrl)
        //{
        //    //Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
        //    //Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

        //    //Session[CommonConstant.CurrentCulture] = ddlCulture;
        //    //return Redirect(returnUrl);
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[CommonConstant.UserSession];
            if (session == null)
            {
                //filterContext.Result = new RedirectToRouteResult
                //    (new RouteValueDictionary(new
                //{ controller = "User", action = "Login",area=""}));

                var obj = new
                {
                    controller = "User",
                    action = "Login",
                    area = ""
                };
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(obj));

            }
            base.OnActionExecuting(filterContext);
        }
    }

}