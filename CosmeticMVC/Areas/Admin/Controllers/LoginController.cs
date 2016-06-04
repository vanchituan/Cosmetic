using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticMVC.Models;
using CosmeticMVC;
using DataLayer;
using DataLayer.DataAccessObj;
using DataLayer.Framework;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Admin/Login/
        public ActionResult Index()
        {
            DataLayer.Framework.User user = CheckCookie();
            if (user == null)
            {
                return View();
            }
            else// đã add vào cookie r
            {
                SetUserLogin(Request.Cookies["username"].Value);
                return RedirectToAction("Index","Home");
            }
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, model.Password, true);
                if (result == 1)// login thanh cong
                {
                    if (model.RememberMe)
                    {
                        HttpCookie ckUsername = new HttpCookie("username");
                        ckUsername.Value = model.UserName;
                        ckUsername.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(ckUsername);

                        HttpCookie ckPassword= new HttpCookie("password");
                        ckPassword.Value = model.Password;
                        ckUsername.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(ckPassword);
                    }
                    SetUserLogin(model.UserName);
                    
                    return RedirectToAction("Index", "Home");
                }
                #region Invalid
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập.");
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng.");
                }

                #endregion

            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Remove(CommonConstant.UserSession);
            Session.Remove(CommonConstant.SESSION_CREDENTIALS);
            //if (Response.Cookies["username"] != null)
            {
                HttpCookie ckUsername = new HttpCookie("username");
                ckUsername.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(ckUsername);
            }

            //if (Response.Cookies["password"] != null)
            {
                HttpCookie ckPassword = new HttpCookie("password");
                ckPassword.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(ckPassword);
            }
            return View("Index");
        }

        public User CheckCookie()
        {
            DataLayer.Framework.User user = null;
            string username = string.Empty, password = string.Empty;
            if (Request.Cookies["username"] != null)
            {
                username = Request.Cookies["username"].Value;
            }
            if (Request.Cookies["password"] != null)
            {
                password = Request.Cookies["password"].Value;
            }
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                user = new User()
                {
                    UserName = username,
                    Password = password
                };
            }
            return user;
        }

        public void SetUserLogin(string username)
        {
            var dao = new UserDao();
            var user = dao.GetByUsername(username);
            var userSession = new UserLogin()
            {
                Username = user.UserName,
                UserID = user.Id,
                GroupID = user.GroupId
            };
            var listCredentials = dao.GetListCredential(username);

            Session.Add(CommonConstant.SESSION_CREDENTIALS, listCredentials);
            Session.Add(CommonConstant.UserSession, userSession);
        }
    }
}