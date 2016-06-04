using DataLayer.Framework;
using DataLayer.ViewModel.Admin.Order;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class OrderDetailDao
    {
        OnlineShopDBContext db = null;
        public OrderDetailDao()
        {
            db = new OnlineShopDBContext();
        }

        /// <summary>
        /// thêm mới chi tiết hóa đơn
        /// </summary>
        /// <param name="orDetail">đối tượng chi tiết hóa đơn truyền vào</param>
        /// <returns>true nếu thêm mới thành công</returns>
        public void Add(int orID, int proID, int quantity)
        {
            OrderDetail obj = new OrderDetail()
            {
                OrderId = orID,
                ProductId = proID,
                Quantity = quantity
            };
            db.OrderDetails.Add(obj);
            db.SaveChanges();
        }

        public List<CartDetailViewModel> GetCartDetail(int orderId)
        {
            var model = (from a in db.Products
                         join b in db.OrderDetails
                         on a.Id equals b.ProductId
                         where b.OrderId == orderId
                         select new CartDetailViewModel
                         {
                             ProductId = a.Id,
                             ProductName = a.Name,
                             Quantity = b.Quantity.Value,
                             Price = a.Price.Value
                         }).ToList();
            return model;

        }

    }
}
