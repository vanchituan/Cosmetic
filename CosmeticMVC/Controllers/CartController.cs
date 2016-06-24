using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticMVC.Models;
using DataLayer.DataAccessObj;
using System.Web.Script.Serialization;
using DataLayer.Framework;
using System.Configuration;
using CosmeticMVC.Common;
using Common;
using CosmeticMVC.Libraries.NganLuongAPI;
using DataLayer.ViewModel.Client.Cart;

namespace CosmeticMVC.Controllers
{
    public class CartController : Controller
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        // danh sách các sản phẩm trong giỏ hàng
        private const string CartSession = "CartSession";
        //danh sach cac san pham trong gio hang
        private string merchantId = "46680";
        private string merchantPassword = "c86fbc08994c0b05a79d141bd25d3838";
        private string merchantEmail = "vanchituan@gmail.com";


        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public JsonResult Update(string cartModel)
        {
            decimal total = 0;
            // string cartModel là chuoi JSON  cartModel : quantity và object product
            // vd jsonCart :    [{"Quantity":"2","Product":{"ID":32}},{"Quantity":"3","Product":{"ID":1}}]
            //JavaScriptSerializer chuyễn đổi 1 đối tương .NET  bất kì thành một chuỗi JSON và ngược lại.
            // jsonCart là 1 đối tượng anonymous type được chuyen doi tu JSON
            // lúc này jsonCart là 1 đối tượng List<CartItem>
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(p => p.Product.Id == item.Product.Id);
                if (jsonItem != null)//
                {
                    item.Quantity = jsonItem.Quantity;
                }
                total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
            }
            Session["Total"] = total.ToString("N0");
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult AddItem(long productID, int quantity)
        {
            var product = new ProductDao().ViewDetail(productID);
            var cart = Session[CartSession];
            if (cart != null)//có sản phẩm r
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(p => p.Product.Id == productID))    //có sản phẩm trùng
                {
                    foreach (var item in list)
                    {
                        if (item.Product.Id == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else//không có sản phẩm trùng
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CartSession] = list;
            }
            else// chưa có sản phẩm nào trong giỏ hàng
            {
                var item = new CartItem()
                {
                    Product = product,
                    Quantity = quantity

                };
                var list = new List<CartItem>();
                list.Add(item);
                Session[CartSession] = list;
            }
            return Json(true);
        }

        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });

        }

        public JsonResult Delete(long id)
        {
            decimal total = 0;
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(p => p.Product.Id == id);
            Session[CartSession] = sessionCart;
            foreach (var x in sessionCart)
            {
                total += (x.Product.Price.GetValueOrDefault(0) * x.Quantity);
            }
            Session["Total"] = total.ToString("N0");
            return Json(new
            {
                status = true
            });

        }

        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            ViewBag.list = list;
            return View();
        }
        
        [HttpPost]
        public ActionResult Payment(OrderViewModel orderVm)
        {
            Order order = new Order();
            //{
            //    OrderDate = orderVm.OrderDate,
            //    CreatedDate = orderVm.CreatedDate,
            //    CustomerId = orderVm.CustomerId,
            //    DistrictId = orderVm.DistrictId,
            //    PrecinctId = orderVm.PrecinctId,
            //    ProvinceId = orderVm.ProvinceId,
            //    ShipAddress = orderVm.ShipAddress,
            //    ShipEmail = orderVm.ShipEmail,
            //    ShipMobile = orderVm.ShipMobile,
            //    PromotionId = orderVm.PromotionId,
            //    ShipName = orderVm.ShipName,
            //    Status = orderVm.Status
            //};
            
            decimal total = 0;// total value of invoice
            int promoID = 0;
            var cart = (List<CartItem>)Session[CartSession];

            foreach (var x in cart)
            {
                total += (x.Product.Price.GetValueOrDefault(0) * x.Quantity);
            }

            Session["amount"] = total.ToString("N0");
            if (Session[CommonConstant.UserSession] != null)    //purchase get to accumulate
            {
                var userCurrent = (UserLogin)Session[CommonConstant.UserSession];
                var user = new UserDao().GetByID(userCurrent.UserID);
                var point = user.Point;

                int discount = 0;
                foreach (var item in new PromotionDao().GetListValue())
                {
                    if (point >= item.Value)
                    {
                        discount = item.Discount.Value;
                        promoID = item.PromotionId;
                        break;
                    }
                }
                Session["promotion"] = (total * discount / 100).ToString("N0");
                Session["discount"] = discount;
                total -= total * discount / 100;

                // update stock point
                new UserDao().UpdatePoint(userCurrent.UserID, long.Parse(total.ToString()));
                orderVm.CustomerId = user.Id;
                orderVm.PromotionId = promoID;
                orderVm.Total = total;
                orderVm.CreatedDate = DateTime.Now;
                orderVm.ProvinceId = user.ProvinceId;
                orderVm.DistrictId = user.DistrictId;
                orderVm.PrecinctId = user.PrecinctId;
                orderVm.ShipEmail = user.Email;
                orderVm.ShipMobile = user.Phone;
                orderVm.ShipAddress = user.Address;
                Session["idCurrentOrder"] = new OrderDao().Add(order);

            }
            else //purchase don't get to accumulate
            {
                orderVm.CustomerId = 0;
                orderVm.PromotionId = promoID;
                orderVm.Total = total;
                orderVm.CreatedDate = DateTime.Now;
                Session["idCurrentOrder"] = new OrderDao().Add(order);
            }
            foreach (var item in cart)
            {
                new ProductDao().UpdateQuantity(item.Product.Id, item.Quantity);
                new OrderDetailDao().Add(int.Parse(Session["idCurrentOrder"].ToString()), item.Product.Id, item.Quantity);

            }

            RequestInfo info = new RequestInfo();
            info.Merchant_id = merchantId;
            info.Merchant_password = merchantPassword;
            info.Receiver_email = merchantEmail;



            info.cur_code = "vnd";
            info.bank_code = orderVm.BankCode;

            info.Order_code = order.Id.ToString();
            info.Total_amount = total.ToString();
            info.fee_shipping = "0";
            info.Discount_amount = "0";
            info.order_description = "Thanh toán đơn hàng tại online shop";

            info.return_url = "xac-nhan-don-hang";
            info.cancel_url = "huy-don-hang";

            info.Buyer_fullname = order.ShipName;
            info.Buyer_email = "vanngoctu1112@gmail.com";
            info.Buyer_mobile = order.ShipMobile;

            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseInfo result = objNLChecout.GetUrlCheckout(info, orderVm.PaymentMethod);
            if (result.Error_code == "00")
            {
                return Json(new
                {
                    status = true,
                    urlCheckout = result.Checkout_url,
                    message = result.Description
                });
            }
            else
                return Json(new
                {
                    status = false,
                    message = result.Description
                });
        }

        public ActionResult CancelOrder()
        {
            return View();
        }

        public ActionResult ConfirmOrder()
        {
            string token = Request["token"];
            RequestCheckOrder info = new RequestCheckOrder();
            info.Merchant_id = merchantId;
            info.Merchant_password = merchantPassword;
            info.Token = token;
            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseCheckOrder result = objNLChecout.GetTransactionDetail(info);
            if (result.errorCode == "00")
            {
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất.";
            }
            else
            {
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Có lỗi xảy ra. Vui lòng liên hệ admin.";
            }
            return View();
        }

        public ActionResult Success()
        {
            var model = db.Orders.Find(int.Parse(Session["idCurrentOrder"].ToString()));
            ViewBag.CartSession = Session[CartSession];
            Session[CartSession] = null;
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ConfirmBox()
        {
            return PartialView();
        }
    }
}