using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CosmeticMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
      new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
            

            //  giới thiệu
            routes.MapRoute(
                name: "About",
                url: "gioi-thieu",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //  đăng nhập
            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            //  liên hệ
            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            // site is maintaining
            routes.MapRoute(
                name: "Maintaining",
                url: "xin-loi-vi-su-bat-tien-nay",
                defaults: new { controller = "Home", action = "Maintainance", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            //  đăng ký
            routes.MapRoute(
                name: "Register",
                url: "dang-ky",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            // xác nhận tài khoản
            routes.MapRoute(
                name: "User Confirm",
                url: "xac-nhan",
                defaults: new { controller = "User", action = "UserConfirm", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //  tin tức
            routes.MapRoute(
                name: "News",
                url: "tin-tuc",
                defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //Loại tin tức
            routes.MapRoute(
                name: "Content Category",
                url: "tin-tuc/{id}",
                defaults: new { controller = "Content", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            //  chi tiết tin tức
            routes.MapRoute(
                name: "News Detail",
                url: "tin-tuc/{cateID}/{metaTitle}",
                defaults: new { controller = "Content", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            //  Tag
            routes.MapRoute(
                name: "Tags",
                url: "tag/{tagId}",
                defaults: new { controller = "Content", action = "Tag", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            // thong tin ca nhan
            routes.MapRoute(
                name: "User Info",
                url: "thong-tin-ca-nhan/{UserID}",
                defaults: new { controller = "User", action = "UserInfo", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            // lịch sử mua hàng
            routes.MapRoute(
                name: "Order Histoty",
                url: "lich-su-mua-hang",
                defaults: new { controller = "User", action = "OrderHistory", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            //  sản phẩm
            routes.MapRoute(
                name: "Products",
                url: "san-pham",
                defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //  loại sản phẩm
            routes.MapRoute(
                name: "Product Category",
                url: "san-pham/{cateID}",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            //  chi tiết sản phẩm
            routes.MapRoute(
                name: "Product Detail",
                url: "san-pham/{cateID}/{metatitle}",
                defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //  tìm kiếm sản phẩm
            routes.MapRoute(
                name: "Search",
                url: "tim-kiem",
                defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //  giỏ hàng
            routes.MapRoute(
                name: "Cart",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //thêm giỏ hàng
            routes.MapRoute(
                name: "Add Cart",
                url: "them-gio-hang",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //thanh toán
            routes.MapRoute(
                name: "Payment",
                url: "thanh-toan",
                defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //huy don hang tu Ngan Luong
            routes.MapRoute(
                name: "Cancel Order",
                url: "huy-don-hang",
                defaults: new { controller = "Cart", action = "CancelOrder", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );


            //Xac nhan thanh toan don hang tu ngan luong
            routes.MapRoute(
                name: "Confirm Order",
                url: "xac-nhan-don-hang",
                defaults: new { controller = "Cart", action = "ConfirmOrder", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //mua hàng hoàn thành
            routes.MapRoute(
                name: "Payment Success",
                url: "hoan-thanh",
                defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );

            //default phải để dưới cùng, và khi không có patern nao đúng sẽ chạy vào default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticMVC.Controllers" }
            );
        }
    }
}
