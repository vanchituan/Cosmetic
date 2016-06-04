using DataLayer.DataAccessObj;
using DataLayer.Framework;
using DataLayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticMVC.Controllers
{
    public class ContentController : Controller
    {
        OnlineShopDBContext db = new OnlineShopDBContext();

        // GET: Content
        public ActionResult Index()
        {
            return View(new ContentDao().ListAll(3));
        }

        public ActionResult Category(int id,int page = 1, int pageSize = 4)
        {
            var dao = new CategoryDao().GetByID(id);
            ViewBag.CateName = dao.Name;
            ViewBag.CateID = id;
            int totalRecord = 0;
            var model = new ContentDao().ContentCategory(id, ref totalRecord, page, pageSize);
            ViewBag.ListOther = new ContentDao().ListOther(id, 8);


            int totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.Page = page; // trang hiện hành
            ViewBag.Total = totalRecord;
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = 5;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }

        public ActionResult Detail(int cateID,string metaTitle)
        {
            //model là tin hiện hành
            var model = new ContentDao().GetByMetaTitle(metaTitle);

            ViewBag.NewRelated = new ContentDao().ListProContentRelated(int.Parse(model.Id.ToString()),cateID,5);
            ViewBag.Other = new ContentDao().ListOther(cateID,2);
            ViewBag.Tags = new ContentDao().ListTag(model.Id);
            return View(model);
        }

        public ActionResult Tag(string tagId, int page = 1, int pageSize = 10)
        {
            var model = new ContentDao().ListAllByTag(tagId, page, pageSize);
            int totalRecord = 0;
            
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            ViewBag.Tag = new ContentDao().GetTag(tagId);
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

        
    }
}