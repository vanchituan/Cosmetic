using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.DataAccessObj
{
    public class UserGroupDao
    {
        OnlineShopDBContext db = null;
        public UserGroupDao()
        {
            db = new OnlineShopDBContext();
        }

        public List<SelectListItem> GetListForDdl()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "None", Value = "", Selected = true });
            var model = from a in db.UserGroups
                        where a.Id != "ADMIN"
                        select new SelectListItem
                        {
                            Text = a.Name,
                            Value = a.Id
                        };
            list.AddRange(model);
            return list;
        }
    }
}
