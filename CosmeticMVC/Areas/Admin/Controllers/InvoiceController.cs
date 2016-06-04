using CosmeticMVC.Common;
using DataLayer.DataAccessObj;
using DataLayer.Framework;
using DataLayer.ViewModel.Admin.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class InvoiceController : BaseController
    {
        // GET: Admin/Invoice
        [HasCredential(RoleID ="view_invoice")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InvoiceList(SearchingViewModel search)
        {
            var model = new InvoiceDao().GetList(search);
            return PartialView("InvoiceList", model);
        }

        [HttpPost]
        public JsonResult GetDetail(int invoiceId)
        {
            var model = new InvoiceDao().GetDetailById(invoiceId);
            return Json(model);
        }
    }
}