using CosmeticMVC.Common;
using DataLayer.DataAccessObj;
using DataLayer.Framework;
using DataLayer.ViewModel.Admin.Product;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        OnlineShopDBContext db = new OnlineShopDBContext();

        [HasCredential(RoleID = "view_product")]
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ProductList(SearchingViewModel search)
        {
            var model = new ProductDao().GetList(search);
            return PartialView("ProductList", model);
        }

        public ActionResult ProductListForCustomOrder(SearchingViewModel search)
        {
            var model = new ProductDao().GetList(search);
            return PartialView("/Areas/Admin/Views/Order/CreateOrder/Products/ProductListForCustomOrder.cshtml", model);
        }

        [HasCredential(RoleID ="update_warehouse")]
        [HttpGet]
        public ActionResult UpdateWareHouse()
        {
            ViewBag.ddlCategory = new SelectList(new ProductCategoryDao().ListAll(), "ID", "Name");
            return View();
        }

        [HasCredential(RoleID = "create_product")]
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product model)
        {
                new ProductDao().Create(model);
                return RedirectToAction("Index");
        }


        [HasCredential(RoleID = "create_product")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new ProductDao();
            var product = dao.GetByID(id);
            SetViewBag();
            return View(product);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Product model)
        {
            new ProductDao().Edit(model);
            return RedirectToAction("Index");
        }

        public void SetViewBag(int? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }

        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HasCredential(RoleID = "delete_product")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ProductDao().Delete(id);
            return RedirectToAction("Index");
        }

        #region update warehouse
        public JsonResult LoadProductsByCateId(int cateId)
        {
            List<Product> list = new ProductDao().LoadProductsByCateId(cateId);
            return Json(list);
        }

        public JsonResult GetProductById(int productId)
        {
            var model = db.Products.Find(productId);
            return Json(model);
        }

        public ActionResult UpdateWareHouse(int productId, int quantity)
        {
            var model = new ProductDao().UpdateWareHouse(productId, quantity);
            return Json(new
            {
                product = model
            });
        }
        #endregion
    }
}