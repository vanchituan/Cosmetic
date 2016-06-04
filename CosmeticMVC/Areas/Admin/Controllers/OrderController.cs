using CosmeticMVC.Common;
using DataLayer.DataAccessObj;
using DataLayer.Framework;
using DataLayer.ViewModel;
using DataLayer.ViewModel.Admin.Order;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CosmeticMVC.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        // GET: Admin/Order
        [HasCredential(RoleID = "view_order")]
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult OrderList(SearchingViewModel search)
        {
            var model = new OrderDao().GetList(search);
            return PartialView("OrderList", model);
        }

        public ActionResult ExportOrderList(SearchingViewModel search)
        {
            string fileName = "DanhSachHoaDon" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            var excelExport = new Libraries.ExportExcel.OrderTemplate();
            Stream stream = new MemoryStream();
            var items = new OrderDao().GetListForExport(search);
            excelExport.CreatePackage(stream, items, search);
            stream.Position = 0;
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName
            };
        }

        [HttpPost]
        public JsonResult GetDetail(int orderID)
        {
            var model = new OrderDao().GetDetailByOrderId(orderID);
            return Json(model);
        }


        [HttpPost]
        public JsonResult ChangeStatus(int orderId, int selectedValue)
        {
            var result = "";
            if (selectedValue == 5) // "giao hàng thành công" is insert to invoice but don't change status
            {
                var invoiceDao = new InvoiceDao();
                var invoiceDetailDao = new InvoiceDetailDao();
                var currentOrder = db.Orders.Find(orderId);
                decimal profit = 0;
                foreach (var item in new OrderDetailDao().GetCartDetail(orderId))
                {
                    profit += invoiceDao.GetProfitFromProductId(item.ProductId, item.Quantity);
                }
                Invoice invoice = new Invoice
                {
                    CreatedDate = currentOrder.CreatedDate,
                    CustomerId = currentOrder.CustomerId,
                    OrderId = currentOrder.Id,
                    ShipAddress = currentOrder.ShipAddress,
                    ShipMobile = currentOrder.ShipMobile,
                    ShipName = currentOrder.ShipName,
                    ShipEmail = currentOrder.ShipEmail,
                    ShippedDate = DateTime.Now,
                    Total = currentOrder.Total,
                    Profit = profit,
                    ProvinceId = currentOrder.ProvinceId,
                    DistrictId = currentOrder.DistrictId,
                    PrecinctId = currentOrder.PrecinctId
                };
                int invoiceId = invoiceDao.Insert(invoice);

                //insert to invoide detail
                foreach (var item in new OrderDetailDao().GetCartDetail(orderId))
                {
                    invoiceDetailDao.Insert(invoiceId, item.ProductId, item.Quantity);
                }
            }

            result = new OrderDao().ChangeStatus(orderId, selectedValue);

            return Json(result);
        }


        [HasCredential(RoleID ="create_order")]
        public ActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrder(Order order, string orderDetail)
        {
            var dao = new OrderDao();
            var detailDao = new OrderDetailDao();
            var productDao = new ProductDao();
            var userDao = new UserDao();
            var promotionDao = new PromotionDao();
            var cartDetail = new JavaScriptSerializer().Deserialize<List<DataLayer.ViewModel.Admin.Order.CartDetailViewModel>>(orderDetail);
            if (order.CustomerId == null) // khách lẽ
            {
                order.CustomerId = 0;
                int currentId = dao.Add(order);
                foreach (var item in cartDetail)
                {
                    productDao.UpdateQuantity(item.ProductId, item.Quantity);
                    detailDao.Add(currentId, item.ProductId, item.Quantity);
                }
            }
            else// khách quen
            {
                var customer = db.Users.Find(order.CustomerId);
                int promotionId = promotionDao.GetPromotionIdByCusId(customer.Id);

                order.PromotionId = promotionId;
                int currentId = dao.Add(order);
                foreach (var item in cartDetail)
                {
                    productDao.UpdateQuantity(item.ProductId, item.Quantity);
                    detailDao.Add(currentId, item.ProductId, item.Quantity);
                }
                userDao.UpdatePoint(customer.Id, Convert.ToInt32(order.Total.Value));
            }
            return View();
        }

    }
}