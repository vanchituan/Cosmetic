using BotDetect.Web;
using BotDetect.Web.UI.Mvc;
using CosmeticMVC.Models;
using DataLayer.DataAccessObj;
using DataLayer.Framework;
using DataLayer.ViewModel.Admin.User;
using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebMatrix.WebData;

namespace CosmeticMVC.Controllers
{
    public class UserController : Controller
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        // GET: User
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.UserName = email;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.Username = user.UserName;
                    userSession.UserID = user.Id;
                    Session.Add(CommonConstant.UserSession, userSession);
                }
            }
            return Redirect("/");
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {

            //Session.Remove(CommonConstant.UserSession);
            //Session.Remove(CommonConstant.SESSION_CREDENTIALS);

            HttpCookie ckUsername = new HttpCookie("username");
            ckUsername.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(ckUsername);

            HttpCookie ckPassword = new HttpCookie("password");
            ckPassword.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(ckPassword);

            Session.Abandon();
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {

                var dao = new UserDao();
                var result = dao.Login(model.UserName, model.Password);
                if (result == 1)
                {
                    if (model.RememberMe)
                    {
                        HttpCookie ckUsername = new HttpCookie("username");
                        ckUsername.Value = model.UserName;
                        ckUsername.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(ckUsername);

                        HttpCookie ckPassword = new HttpCookie("password");
                        ckPassword.Value = model.Password;
                        ckUsername.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(ckPassword);
                    }

                    var user = dao.GetByUsername(model.UserName);
                    var userSession = new UserLogin()
                    {
                        Username = user.UserName,
                        UserID = user.Id,
                        GroupID = user.GroupId
                    };
                    var listCredentials = dao.GetListCredential(model.UserName);

                    Session.Add(CommonConstant.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstant.UserSession, userSession);

                    return Redirect("/");
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
                    ModelState.AddModelError("", "Đăng nhập không đúng.");
                }
                #endregion
            }
            return View(model);
        }

        [HttpPost]
        //[CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        public JsonResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.GroupId = "MEMBER";
                user.Status = false;
                user.CreatedDate = DateTime.Now;
                user.Point = 0;
                var result = new UserDao().Insert(user);
                Session["CurrentReg"] = user;
            }
            return Json(new
            {
                status = true
            });
        }

        public ActionResult UserInfo(int UserID)
        {
            var user = new UserDao().GetByID(UserID);
            return View(user);
        }

        public JsonResult Update(string username, string password, string name, string phone, string email, string address, string photoPath)
        {
            var user = new User()
            {
                UserName = username,
                Password = password,
                Name = name,
                Address = address,
                Phone = phone,
                Email = email,
                Image = photoPath
            };
            UserDao u = new UserDao();
            u.Update(user);
            return Json(new
            {
                status = true
            });

        }

        /// <summary>
        /// kiểm tra user truyền vào đã tồn tại chưa ?
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public JsonResult CheckUsername(string userName)
        {
            var getData = db.Users.FirstOrDefault(m => m.UserName == userName.Trim());
            bool flag = getData == null ? true : false;
            return Json(new
            {
                status = flag
            });
        }

        public JsonResult CheckCaptcha(string captchaId, string instanceId,
          string userInput)
        {
            bool ajaxValidationResult = CaptchaControl.AjaxValidate(captchaId, userInput, instanceId);
            return Json(ajaxValidationResult, JsonRequestBehavior.AllowGet);
        }

        #region Load Dropdown district, province

        public JsonResult LoadProvince()
        {
            var list = new UserDao().LoadProvince();
            return Json(new
            {
                data = list,
                status = true
            });
        }
        public JsonResult LoadDistrict(int provinceID)
        {
            var model = new UserDao().LoadDistrict(provinceID);
            return Json(new
            {
                data = model,
                status = true
            });
        }

        public JsonResult LoadPrecincts(int provinceId, int districtId)
        {
            var model = new UserDao().LoadPrecincts(provinceId, districtId);
            return Json(model);
        }
        #endregion

        public ActionResult OrderHistory()
        {
            var session = (UserLogin)Session[CommonConstant.UserSession];
            var listOrderHis = new UserDao().OrderHistory(session.UserID);
            return View(listOrderHis);
        }

        [HttpPost]
        public JsonResult GetOrderDetail(int orID)
        {
            var model = new OrderDetailDao().GetCartDetail(orID);
            return Json(new
            {
                status = true,
                data = model
            });
        }

        public ActionResult UserConfirm()
        {
            var currentReg = (DataLayer.Framework.User)Session["CurrentReg"];
            return View(currentReg);
        }

    }
}