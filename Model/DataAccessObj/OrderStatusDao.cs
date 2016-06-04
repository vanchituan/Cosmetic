using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.DataAccessObj
{
    public class OrderStatusDao
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        public List<SelectListItem> GetListForDdl()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "None", Selected = true });
            var model = from a in db.OrderStatuses
                        select new SelectListItem
                        {
                            Text = a.Name,
                            Value = a.OrderStatusId.ToString()
                        };
            list.AddRange(model);
            return list;
        }

        public string GetStatusNameById(int id)
        {
            return db.OrderStatuses.Find(id).Name;
        }
    }
}
