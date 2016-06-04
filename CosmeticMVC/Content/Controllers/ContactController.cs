using DataLayer.DataAccessObj;
using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticMVC.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var model = new ContactDao().GetActiveContact();
            return View(model);
        }

        [HttpPost]
        public JsonResult Send(string name, string mobile, string address, string email, string content)
        {
            var feedback = new Feedback()
            {
                Name = name,
                Email = email,
                CreatedDate = DateTime.Now,
                Phone = mobile,
                Content = content,
                Address = address,
                Status = false
            };
            int id = new ContactDao().InsertFeedBack(feedback);

            return Json(new
            {
                status = true
            });
        }
    }
}