using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.DataAccessObj;
using DataLayer.Framework;
using CosmeticMVC.Common;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {

        // GET: Admin/Content
        [HasCredential(RoleID = "view_content")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Content model)
        {
            if (ModelState.IsValid)
            {
                //var session = (UserLogin)Session[CommonConstant.UserSession];
                //model.CreatedBy = session.Username;
                //var culture = Session[CommonConstant.CurrentCulture];
                //model.Language = culture.ToString();
                new ContentDao().Create(model);
                return RedirectToAction("Index");
            }
            SetViewBag(1);
            return View();
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new ContentDao();
            var content = dao.GetByID(id);
            SetViewBag();
            return View(content);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Content model)
        {
            if (ModelState.IsValid)
            {
                new ContentDao().Edit(model);
            }
            SetViewBag(model.CategoryId);
            return RedirectToAction("Index");
        }

        public void SetViewBag(int? selectedId = null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }

        public JsonResult ChangeStatus(long id)
        {
            var result = new ContentDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ContentDao().Delete(id);
            return RedirectToAction("Index");
        }
    }
}