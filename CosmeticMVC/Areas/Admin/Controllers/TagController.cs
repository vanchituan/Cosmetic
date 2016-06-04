using CosmeticMVC.Common;
using DataLayer.DataAccessObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class TagController : BaseController
    {
        [HasCredential(RoleID = "view_tag")]
        // GET: Admin/Tag
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            return View(new TagDao().ListAllPaging(page,pageSize));
        }

        [HasCredential(RoleID = "delete_tag")]
        public ActionResult Detail(string ID)
        {
            var dao = new TagDao().GetByID(ID);
            ViewBag.TagName = dao.Name;
            return View(new TagDao().GetDetail(ID));
        }
    }
}