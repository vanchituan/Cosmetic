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
    public class CredentialController : BaseController
    {
        [HasCredential(RoleID = "view_credential")]
        public ActionResult Index()
        {
            var dao = new CredentialDao();
            var model = dao.ListAllPaging();
            return View(model);
        }

        [HasCredential(RoleID = "view_credential")]
        public ActionResult Create()
        {
            var dao = new RoleDao();
            ViewBag.RoleID = new SelectList(dao.ListRole(), "ID", "Name");
            ViewBag.UserGroupID = new SelectList(dao.ListUserGroup(), "ID", "Name");
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "view_credential")]
        public ActionResult Create(Credential cre)
        {
            if (ModelState.IsValid)
            {
                var dao = new RoleDao();
                var creDao = new CredentialDao();
                if (creDao.CheckExists(cre))        // hợp lệ
                {
                    creDao.Insert(cre.UserGroupId, cre.RoleId);
                    return RedirectToAction("Index");
                }
                else// phân quyền truyền vào trùng
                {
                    ModelState.AddModelError("", "Quyền này đã tồn tại rồi");
                    ViewBag.RoleID = new SelectList(dao.ListRole(), "ID", "Name");
                    ViewBag.UserGroupID = new SelectList(dao.ListUserGroup(), "ID", "Name");
                }
                
            }
            return View(cre);

        }

        [HasCredential(RoleID = "delete_credential")]
        [HttpDelete]
        public ActionResult Delete(string userGroupID, string roleID)
        {
            var res = new CredentialDao().Delete(userGroupID, roleID);
            return View();
        }
    }
}