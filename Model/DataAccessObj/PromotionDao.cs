using DataLayer.Framework;
using DataLayer.ViewModel.Admin.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.DataAccessObj
{
    public class PromotionDao
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        public List<Promotion> GetListValue()
        {
            var list = db.Promotions.OrderByDescending(p=>p.Value).ToList();
            return list;
        }

        public List<SelectListItem> GetPromotionList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "None",  Selected = true });
            var model = from a in db.Promotions
                        select new SelectListItem
                        {
                            Text = a.PromotionName,
                            Value = a.PromotionId.ToString()
                        };
            list.AddRange(model);
            return list; 
        }

        public Promotion GetById(int promotionId)
        {
            return db.Promotions.Find(promotionId);
        }


        public string GetPromotionNameByCusId(int customerId)
        {
            var promotionId = db.Users.Find(customerId).PromotionId;
            return db.Promotions.Find(promotionId).PromotionName;
        }

        //public string GetNameByPoint(decimal point)
        //{
        //    string name = null;
        //    var model = from a in db.Promotions
        //                select new
        //                {
        //                    a.PromotionName,
        //                    a.Value
        //                };

        //    foreach (var item in model)
        //    {
        //        if (point >= item.Value)
        //        {
        //            name = item.PromotionName;
        //        }
        //    }
        //    return name = name ?? "Khách lẽ";
        //}

        public int GetPromotionIdByCusId(int customerId)
        {
            var customer = db.Users.Find(customerId);
            int promotionId = 0;
            var model = from a in db.Promotions
                        select new
                        {
                            a.PromotionId,
                            a.PromotionName,
                            a.Value
                        };

            foreach (var item in model)
            {
                if (customer.Point >= item.Value)
                {
                    promotionId = item.PromotionId;
                }
            }
            return promotionId;
        }
    }
}
