using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.DataAccessObj;
using DataLayer.Framework;

namespace CosmeticMVC.Controllers
{
    public class ProductController : Controller
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        // GET: Product
        public ActionResult Index(string page = "1")
        {
            int pageSize = 8;
            int totalProduct = db.Products.Count();
            ViewBag.TotalProduct = totalProduct - pageSize * int.Parse(page);
            int tak = pageSize * int.Parse(page);
            var query = db.Products.OrderBy(p => p.Id)
                         .Take(tak);
            return Request.IsAjaxRequest() ?
                (ActionResult)PartialView("ProductList", query)
                : View(query);
        }

        [HttpPost]
        public JsonResult Remain(int page)
        {
            int remain = db.Products.Count() - (page * 8);
            return Json(new
            {
                data = remain
            });
        }

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }


        /// <summary>
        /// trả về danh sách các sản phẩm có mã loại truyền vào
        /// </summary>
        /// <param name="cateID">Mã loại sản phẩm muốn xem</param>
        /// <param name="page">Bắt đâu ở trang nào</param>
        /// <param name="pageSize">Số sản phẩm hiển thị trên 1 trang</param>
        /// <returns></returns>
        public ActionResult Category(long cateID, int page = 1, int pageSize = 4)
        {
            var category = new ProductCategoryDao().ViewDetail(cateID); // trả về record

            int totalRecord = 0;
            var model = new ProductDao().ListByCategoryID(cateID, ref totalRecord, page, pageSize);
            int totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));

            ViewBag.Page = page; // trang hiện hành
            ViewBag.Category = category;
            ViewBag.Total = totalRecord;
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = 5;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        {
            int totalRecord = 0;
            var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        public ActionResult Detail(int cateID, string metaTitle)
        {
            var product = new ProductDao().GetByMetaTitle(metaTitle);
            var dao = db.ProductCategories.Find(cateID);
            ViewBag.CategoryName = dao.Name;
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(product.Id, 4);
            return View(product);
        }

        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}