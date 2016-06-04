using CosmeticMVC.Common;
using DataLayer.DataAccessObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        [HasCredential(RoleID = "view_productcategory")]
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var model = new ProductCategoryDao().ListAll();
            return View(model);
        }

        [HasCredential(RoleID = "change_productcategory")]
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductCategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }


    }
}