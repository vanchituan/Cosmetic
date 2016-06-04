using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Framework;
using DataLayer.ViewModel;
using MvcPaging;
using DataLayer.ViewModel.Admin.Order;
using System.Globalization;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace DataLayer.DataAccessObj
{
    public class OrderDao
    {
        public ListViewModel GetList(SearchingViewModel search)
        {
            var db = new OnlineShopDBContext();
            try
            {
                var result = new ListViewModel();
                result.OrderBy = search.OrderBy;
                result.SortBy = search.SortBy;
                if (search.FromDate != null)
                {
                    string fromDate = search.FromDate.Value.ToString("dd/MM/yyyy");
                    search.FromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (search.ToDate != null)
                {
                    string toDate = search.ToDate.Value.ToString("dd/MM/yyyy");
                    search.ToDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                var model = (from a in db.Orders
                             join c in db.OrderStatuses on a.Status equals c.OrderStatusId
                             join d in db.Promotions on a.PromotionId equals d.PromotionId
                             join e in db.Users on a.CustomerId equals e.Id
                             where (search.OrderId == null || a.Id == search.OrderId) &&
                             (search.ShipAddress == null || a.ShipAddress == search.ShipAddress) &&
                             (search.ShipMobile == null || a.ShipMobile == search.ShipMobile) &&
                             (search.ShipName == null || a.ShipName == search.ShipName) &&
                             (search.CustomerName == null || e.Name == search.CustomerName) &&
                             (search.PromotionId == null || a.PromotionId == search.PromotionId) &&
                             (search.StatusId == null || a.Status == search.StatusId) &&
                             (search.FromDate == null || a.CreatedDate >= search.FromDate) &&
                             (search.ToDate == null || a.CreatedDate <= search.ToDate) &&
                             (search.ProvinceId == null || a.ProvinceId == search.ProvinceId) &&
                             (search.DistrictId == null || a.DistrictId == search.DistrictId) &&
                             (search.PrecinctId == null || a.PrecinctId == search.PrecinctId)
                             group a by new
                             {
                                 a.Id,
                                 a.CreatedDate,
                                 a.Total,
                                 d.PromotionName,
                                 c.Name,
                                 a.ShipMobile,
                                 a.ShipAddress,
                                 a.ShipName,
                                 a.Status,
                                 e.UserName,
                                 a.ShipEmail,
                                 a.PromotionId,
                                 a.ProvinceId,
                                 a.DistrictId,
                                 a.PrecinctId
                             } into g
                             select new
                             {
                                 OrderId = g.Key.Id,
                                 CreatedDate = g.Key.CreatedDate.Value,
                                 Total = g.Key.Total.Value,
                                 StatusName = g.Key.Name,
                                 ShipAddress = g.Key.ShipAddress,
                                 ShipMobile = g.Key.ShipMobile,
                                 ShipName = g.Key.ShipName,
                                 PromotionName = g.Key.PromotionName,
                                 CustomerName = g.Key.UserName,
                                 ShipEmail = g.Key.ShipEmail,
                                 PromotionId = g.Key.PromotionId,
                                 ProvinceId = g.Key.ProvinceId,
                                 DistrictId = g.Key.DistrictId,
                                 PrecinctId = g.Key.PrecinctId
                             }).AsEnumerable().
                            Select(x => new ItemViewModel
                            {
                                OrderId = x.OrderId,
                                CreatedDate = x.CreatedDate,
                                CustomerName = x.CustomerName,
                                ShipName = x.ShipName,
                                PromotionId = x.PromotionId,
                                PromotionName = x.PromotionName,
                                ShipAddress = x.ShipAddress + "" + new UserDao().GetAddress(x.PrecinctId, x.DistrictId, x.ProvinceId),
                                ShipEmail = x.ShipEmail,
                                ShipMobile = x.ShipMobile,
                                StatusName = x.StatusName,
                                Total = x.Total
                            });

                #region sort by
                if (result.SortBy.Value)
                {
                    switch (result.OrderBy)
                    {
                        case "OrderId":
                            model = model.OrderBy(q => q.OrderId);
                            break;
                        case "ShipName":
                            model = model.OrderBy(q => q.ShipName);
                            break;
                        case "ShipAddress":
                            model = model.OrderBy(q => q.ShipAddress);
                            break;
                        case "CreatedDate":
                            model = model.OrderBy(q => q.CreatedDate);
                            break;
                        case "StatusName":
                            model = model.OrderBy(q => q.StatusName);
                            break;
                        case "PromotionName":
                            model = model.OrderBy(q => q.PromotionName);
                            break;
                        case "Total":
                            model = model.OrderBy(q => q.Total);
                            break;

                    }
                }
                else
                {
                    switch (result.OrderBy)
                    {
                        case "OrderId":
                            model = model.OrderByDescending(q => q.OrderId);
                            break;
                        case "ShipName":
                            model = model.OrderByDescending(q => q.ShipName);
                            break;
                        case "CreatedDate":
                            model = model.OrderByDescending(q => q.CreatedDate);
                            break;
                        case "ShipAddress":
                            model = model.OrderByDescending(q => q.ShipAddress);
                            break;
                        case "StatusName":
                            model = model.OrderByDescending(q => q.StatusName);
                            break;
                        case "PromotionName":
                            model = model.OrderByDescending(q => q.PromotionName);
                            break;
                        case "Total":
                            model = model.OrderByDescending(q => q.Total);
                            break;
                    }
                }
                #endregion
                int pageNumber = search.PageCurrent.HasValue ? search.PageCurrent.Value : 1;
                int pageSize = result.PageSize.HasValue ? search.PageSize : 10;
                result.Items = model.ToPagedList(pageNumber, pageSize);
                result.Total = model.Count();

                return result;
            }
            catch (Exception ex)
            {
                db.Dispose();
                return null;
            }
        }

        public List<ItemViewModel> GetListForExport(SearchingViewModel search)
        {
            var db = new OnlineShopDBContext();
            try
            {
                var model = from a in db.Orders

                            join c in db.OrderStatuses on a.Status equals c.OrderStatusId
                            join d in db.Promotions on a.PromotionId equals d.PromotionId
                            join e in db.Users on a.CustomerId equals e.Id
                            where
                            (search.OrderId == null || a.Id == search.OrderId) &&
                            (search.ShipAddress == null || a.ShipAddress == search.ShipAddress) &&
                            (search.ShipMobile == null || a.ShipMobile == search.ShipMobile) &&
                            (search.ShipName == null || a.ShipName == search.ShipName) &&
                            (search.CustomerName == null || e.Name == search.CustomerName) &&
                            (search.PromotionId == null || a.PromotionId == search.PromotionId) &&
                            (search.StatusId == null || a.Status == search.StatusId) &&
                            (search.FromDate == null || a.CreatedDate >= search.FromDate) &&
                            (search.ToDate == null || a.CreatedDate <= search.ToDate)

                            group a by new
                            {
                                a.Id,
                                a.CreatedDate,
                                a.Total,
                                d.PromotionName,
                                c.Name,
                                a.ShipMobile,
                                a.ShipAddress,
                                a.ShipName,
                                a.Status,
                                e.UserName,
                                a.ShipEmail,
                            } into g
                            select new ItemViewModel
                            {
                                OrderId = g.Key.Id,
                                Total = g.Key.Total.Value,
                                StatusName = g.Key.Name,
                                ShipAddress = g.Key.ShipAddress,
                                ShipMobile = g.Key.ShipMobile,
                                ShipName = g.Key.ShipName,
                                PromotionName = g.Key.PromotionName,
                                CustomerName = g.Key.UserName,
                                ShipEmail = g.Key.ShipEmail,
                                CartList = (from a in db.Products
                                               join c in db.OrderDetails on a.Id equals c.ProductId
                                               join b in db.Orders on c.OrderId equals b.Id
                                               where c.OrderId == g.Key.Id
                                               select new DataLayer.ViewModel.Admin.Order.CartDetailViewModel
                                               {
                                                   ProductName = a.Name,
                                                   Quantity = c.Quantity.Value
                                               }).ToList()
                            };
                return model.ToList();
            }
            catch (Exception)
            {
                return null;
                db.Dispose();
            }
        }

        public int Add(Order order)
        {
            var db = new OnlineShopDBContext();
            try
            {
                db.Orders.Add(order);
                db.SaveChanges();
                int id = order.Id;
                return id;
            }
            catch (Exception ex)
            {
                db.Dispose();
                return 0;
            }

        }

        public string ChangeStatus(int orderID, int selectedValue)
        {
            var db = new OnlineShopDBContext();
            try
            {
                var order = db.Orders.Find(orderID);
                order.Status = selectedValue;
                db.SaveChanges();
                string str = new OrderStatusDao().GetStatusNameById(order.Status);
                return str;
            }
            catch (Exception)
            {
                return null;
                db.Dispose();
            }
        }

        public ItemViewModel GetDetailByOrderId(int orderId)
        {
            var db = new OnlineShopDBContext();
            try
            {
                var model = (from a in db.Orders
                             join c in db.Promotions on a.PromotionId equals c.PromotionId
                             join d in db.OrderStatuses on a.Status equals d.OrderStatusId
                             where a.Id == orderId
                             select new
                             {
                                 OrderId = a.Id,
                                 CreatedDate = a.CreatedDate.Value,
                                 PromotionName = c.PromotionName,
                                 ShipEmail = a.ShipEmail,
                                 ShipMobile = a.ShipMobile,
                                 StatusName = d.Name,
                                 ShipName = a.ShipName,
                                 ShipAddress = a.ShipAddress,
                                 PrecinctId = a.PrecinctId,
                                 DistrictId = a.DistrictId,
                                 ProvinceId = a.ProvinceId,
                                 Discount = c.Discount,
                                 StatusId = a.Status
                             }).GroupBy(k => new
                             {
                                 k.OrderId,
                                 k.CreatedDate,
                                 k.PromotionName,
                                 k.ShipEmail,
                                 k.ShipMobile,
                                 k.StatusName,
                                 k.ShipAddress,
                                 k.ProvinceId,
                                 k.DistrictId,
                                 k.PrecinctId,
                                 k.ShipName,
                                 k.Discount,
                                 k.StatusId
                             }).AsEnumerable()
                                         .Select(g => new ItemViewModel
                                         {
                                             OrderId = g.Key.OrderId,
                                             CreatedDate = g.Key.CreatedDate,
                                             PromotionName = g.Key.PromotionName,
                                             ShipAddress = g.Key.ShipAddress + "" + new UserDao().GetAddress(g.Key.PrecinctId.Value, g.Key.DistrictId.Value, g.Key.ProvinceId.Value),
                                             StatusName = g.Key.StatusName,
                                             ShipEmail = g.Key.ShipEmail,
                                             ShipMobile = g.Key.ShipMobile,
                                             ShipName = g.Key.ShipName,
                                             CartList = (from x in db.OrderDetails
                                                         join y in db.Products on x.ProductId equals y.Id
                                                         where x.OrderId == orderId
                                                         select new CartDetailViewModel
                                                         {
                                                             ProductId = y.Id,
                                                             ProductName = y.Name,
                                                             Price = y.Price.Value,
                                                             Quantity = x.Quantity.Value
                                                         }).ToList(),
                                             Discount = g.Key.Discount.Value,
                                             StatusId = g.Key.StatusId
                                         }).Single();

                return model;
            }
            catch (Exception)
            {
                db.Dispose();
                return null;
            }
        }
    }
}
