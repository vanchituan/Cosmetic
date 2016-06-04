using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Framework;
using DataLayer.DataAccessObj;
using CosmeticMVC.Common;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        CategoryModel model = new CategoryModel();
        [HasCredential(RoleID = "view_category")]
        public ActionResult Index()
        {
            return View(new CategoryDao().ListAll());
        }

        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new CategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }




    }
}