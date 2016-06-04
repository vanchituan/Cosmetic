using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Framework;
using DataLayer.DataAccessObj;
using PagedList;
using PagedList.Mvc;
using CosmeticMVC.Common;
using DataLayer.ViewModel.Admin.User;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {

        

        // GET: Admin/User
        [HasCredential(RoleID = "view_user")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserList(SearchingViewModel search)
        {
            var model = new UserDao().GetList(search);
            return PartialView("UserList", model);
        }

        public ActionResult UserListForCustomOrder(SearchingViewModel search)
        {
            var model = new UserDao().GetList(search);
            return PartialView("~/Areas/Admin/Views/Order/CreateOrder/ModalCustomer/UserListForCustomOrder.cshtml", model);
        }

        [HasCredential(RoleID = "view_user")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //[HasCredential(RoleID = "view_user")]
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                long id = dao.Insert(user);
                if (id > 0)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Them user thanh cong");
                }
            }
            return View("Index");
        }

        

        //[HasCredential(RoleID = "view_user")]
        public ActionResult Edit(int id)
        {
            var user = new UserDao().GetByID(id);
            return View(user);
        }

        //[HasCredential(RoleID = "view_user")]
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                //if (!string.IsNullOrEmpty(user.Password))
                //{
                //    var encryptedMd5Pass = Encryptor.MD5Hash(user.Password);
                //    user.Password = encryptedMd5Pass;
                //}

                bool result = dao.Update(user);
                if (result)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Sua user thanh cong");
                }
            }
            return View("Index");
        }

        [HasCredential(RoleID = "delete_user")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new UserDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpPost]
        public JsonResult ChangeGroup(long id)
        {
            bool result = new UserDao().ChangeUserGroup(id);
            return Json(new
            {
                status = result
            });
        }

        public JsonResult GetDiscount(int promotionId)
        {
            var dao = new PromotionDao();
            int discount = dao.GetById(promotionId).Discount.Value;
            return Json(discount);
        }


    }
}