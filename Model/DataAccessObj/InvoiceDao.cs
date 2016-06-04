using DataLayer.Framework;
using DataLayer.ViewModel.Admin.Invoice;
using DataLayer.ViewModel.Admin.Order;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class InvoiceDao
    {
        OnlineShopDBContext db;
        public InvoiceDao()
        {
            db = new OnlineShopDBContext();
        }

        public decimal GetProfitFromProductId(int productId, int quantity)
        {
            var currentProduct = db.Products.Find(productId);
            decimal profit = (currentProduct.Price.Value - currentProduct.PriceImport.Value) * quantity;
            return profit;
        }

        public int Insert(Invoice invoice)
        {
            db.Invoices.Add(invoice);
            db.SaveChanges();
            return invoice.InvoiceId;
        }

        public ViewModel.Admin.Invoice.ListViewModel GetList(ViewModel.Admin.Invoice.SearchingViewModel search)
        {
            var result = new ViewModel.Admin.Invoice.ListViewModel();
            result.OrderBy = search.OrderBy;
            result.SortBy = search.SortBy;

            var model = (from a in db.Invoices
                        join d in db.Promotions on a.PromotionId equals d.PromotionId
                        join e in db.Users on a.CustomerId equals e.Id
                        where (search.InvoiceId == null || a.InvoiceId == search.InvoiceId) &&
                        (search.ShipAddress == null || a.ShipAddress == search.ShipAddress) &&
                        (search.ShipMobile == null || a.ShipMobile == search.ShipMobile) &&
                        (search.ShipName == null || a.ShipName == search.ShipName) &&
                        (search.CustomerName == null || e.Name == search.CustomerName) &&
                        (search.PromotionId == null || a.PromotionId == search.PromotionId) &&
                        (search.FromDate == null || a.CreatedDate >= search.FromDate) &&
                        (search.ToDate == null || a.CreatedDate <= search.ToDate)
                        group a by new
                        {
                            a.InvoiceId,
                            a.CreatedDate,
                            a.Total,
                            d.PromotionName,
                            a.ShipMobile,
                            a.ShipAddress,
                            a.ShipName,
                            e.UserName,
                            a.ShipEmail,
                            a.OrderId,
                            a.ProvinceId,
                            a.DistrictId,
                            a.PrecinctId
                        } into g
                        select new 
                        {
                            InvoiceId = g.Key.InvoiceId,
                            CreatedDate = g.Key.CreatedDate.Value,
                            Total = g.Key.Total.Value,
                            ShipAddress = g.Key.ShipAddress,
                            ShipMobile = g.Key.ShipMobile,
                            ShipName = g.Key.ShipName,
                            PromotionName = g.Key.PromotionName,
                            CustomerName = g.Key.UserName,
                            ShipEmail = g.Key.ShipEmail,
                            OrderId = g.Key.OrderId,
                            ProvinceId = g.Key.ProvinceId.Value,
                            DistrictId = g.Key.DistrictId.Value,
                            PrecinctId = g.Key.PrecinctId.Value
                        }).AsEnumerable().
                        Select(m=> new ViewModel.Admin.Invoice.ItemViewModel
                        {
                            InvoiceId = m.InvoiceId,
                            OrderId = m.OrderId,
                            CustomerName = m.CustomerName,
                            CreatedDate = m.CreatedDate,
                            PromotionName = m.PromotionName,
                            Total = m.Total,
                            ShipAddress = m.ShipAddress + "" + new UserDao().GetAddress(m.PrecinctId,m.DistrictId,m.ProvinceId),
                            ShipEmail = m.ShipEmail,
                            ShipMobile = m.ShipMobile,
                            ShipName = m.ShipName,
                            
                        });

            #region sort by
            if (result.SortBy.Value)
            {
                switch (result.OrderBy)
                {
                    case "InvoiceId":
                        model = model.OrderBy(q => q.InvoiceId);
                        break;
                    case "ShipName":
                        model = model.OrderBy(q => q.ShipName);
                        break;
                    case "OrderId":
                        model = model.OrderBy(q => q.OrderId);
                        break;
                    case "CreatedDate":
                        model = model.OrderBy(q => q.CreatedDate);
                        break;
                    case "ShipMobile":
                        model = model.OrderBy(q => q.ShipMobile);
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
                    case "InvoiceId":
                        model = model.OrderByDescending(q => q.InvoiceId);
                        break;
                    case "ShipName":
                        model = model.OrderByDescending(q => q.ShipName);
                        break;
                    case "OrderId":
                        model = model.OrderByDescending(q => q.OrderId);
                        break;
                    case "CreatedDate":
                        model = model.OrderByDescending(q => q.CreatedDate);
                        break;
                    case "ShipMobile":
                        model = model.OrderByDescending(q => q.ShipMobile);
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

        public ViewModel.Admin.Invoice.ItemViewModel GetDetailById(int invoiceId)
        {
            var db = new OnlineShopDBContext();
            try
            {
                var model = (from a in db.Invoices
                             join c in db.Promotions on a.PromotionId equals c.PromotionId
                             where a.InvoiceId == invoiceId
                             select new
                             {
                                 OrderId = a.OrderId,
                                 CreatedDate = a.CreatedDate.Value,
                                 PromotionName = c.PromotionName,
                                 ShipEmail = a.ShipEmail,
                                 ShipMobile = a.ShipMobile,
                                 ShipName = a.ShipName,
                                 ShipAddress = a.ShipAddress,
                                 PrecinctId = a.PrecinctId,
                                 DistrictId = a.DistrictId,
                                 ProvinceId = a.ProvinceId,
                                 Discount = c.Discount,
                             }).GroupBy(k => new
                             {
                                 k.OrderId,
                                 k.CreatedDate,
                                 k.PromotionName,
                                 k.ShipEmail,
                                 k.ShipMobile,
                                 k.ShipAddress,
                                 k.ProvinceId,
                                 k.DistrictId,
                                 k.PrecinctId,
                                 k.ShipName,
                                 k.Discount,
                             }).AsEnumerable()
                                         .Select(g => new ViewModel.Admin.Invoice.ItemViewModel
                                         {
                                             OrderId = g.Key.OrderId,
                                             CreatedDate = g.Key.CreatedDate,
                                             PromotionName = g.Key.PromotionName,
                                             ShipAddress = g.Key.ShipAddress + "" + new UserDao().GetAddress(g.Key.PrecinctId.Value, g.Key.DistrictId.Value, g.Key.ProvinceId.Value),
                                             ShipEmail = g.Key.ShipEmail,
                                             ShipMobile = g.Key.ShipMobile,
                                             ShipName = g.Key.ShipName,
                                             CartList = (from x in db.InvoiceDetails
                                                         join y in db.Products on x.ProductId equals y.Id
                                                         where x.InvoiceId == invoiceId
                                                         select new CartDetailViewModel
                                                         {
                                                             ProductId = y.Id,
                                                             ProductName = y.Name,
                                                             Price = y.Price.Value,
                                                             Quantity = x.Quantity.Value
                                                         }).ToList(),
                                             Discount = g.Key.Discount.Value
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
