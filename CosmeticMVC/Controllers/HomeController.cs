using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Framework;
using DataLayer.DataAccessObj;
using CosmeticMVC.Models;


namespace CosmeticMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDao().ListAll();
            ViewBag.NewProducts = new ProductDao().ListNewProduct(4);
            ViewBag.FeatureProducts = new ProductDao().ListFeatureProduct(4);
            return View();
        }


        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupID(1);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            var model = new MenuDao().ListByGroupID(2);
            if (Request.Cookies["username"] != null)
            {
                var user = new UserDao().GetByUsername(Request.Cookies["username"].Value);
                var userLogin = new UserLogin()
                {
                    UserID = user.Id,
                    Username = user.UserName,
                    GroupID = user.GroupId
                };
                Session.Add(CommonConstant.UserSession, userLogin);
            }
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstant.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }

        public ActionResult Maintainance()
        {
            return View();
        }

    }
}