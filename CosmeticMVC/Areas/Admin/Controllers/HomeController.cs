using DataLayer.DataAccessObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/
        public ActionResult Index()
        {
            if (Request.Cookies["username"] != null)
            {
                var listCredential = new UserDao().GetListCredential(Request.Cookies["username"].Value.ToString());
                Session.Add(CommonConstant.SESSION_CREDENTIALS, listCredential);
            }
            return View();
        }
	}
}