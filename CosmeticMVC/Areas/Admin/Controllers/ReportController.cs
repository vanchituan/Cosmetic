using CosmeticMVC.Common;
using DataLayer.DataAccessObj;
using DataLayer.ViewModel.Admin.Report;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Admin/Report
        [HasCredential(RoleID ="view_report")]
        public ActionResult Profit()
        {
            var model = new ReportChartViewModel
            {
                FromDate = DateTime.Now.AddMonths(-6).Date,
                ToDate = DateTime.Now.Date
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult GetDataLine(DateTime f, DateTime t)
        {
            var model = new ReportDao().GetDataLine(f, t);
            return Json(model);
        }

        public JsonResult GetDateDetail(string date)
        {
            DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var model = new ReportDao().GetDateDetail(dt);
            return Json(model);
        }

        public ActionResult TotalProfit()
        {
            return View();
        }

    }
}