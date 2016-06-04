using CosmeticMVC.Common;
using DataLayer.DataAccessObj;
using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        [HasCredential(RoleID = "view_role")]
        // GET: Admin/Role
        public ActionResult Index()
        {
            return View(new RoleDao().ListAll());
        }

        [HasCredential(RoleID = "view_role")]
        public ActionResult Create()
        {
            return View();
        }

        [HasCredential(RoleID = "view_role")]
        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                var dao = new RoleDao();
                if (dao.CheckExist(role.Id))
                {
                    dao.Insert(role);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Vai trò này đã tồn tại rồi.");
                }
            }
            return View(role);
        }

        public JsonResult CheckRole(string Id)
        {
            var getData = db.Roles.FirstOrDefault(m => m.Id == Id.Trim());
            bool flag = getData == null ? true : false;
            return Json(flag);
        }

        [HasCredential(RoleID = "view_role")]
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            new RoleDao().Delete(id);
            return View();
        }

        public ActionResult Edit(string id)
        {
            return View(db.Roles.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(Role r)
        {
            new RoleDao().Edit(r.Id);
            return View();
        }
    }
}