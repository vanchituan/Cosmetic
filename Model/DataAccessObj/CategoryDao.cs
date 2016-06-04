using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DataAccessObj;
using System.Web;
using System.Web.Mvc;


namespace DataLayer.DataAccessObj
{
    public class CategoryDao
    {
        OnlineShopDBContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDBContext();
        }

        public List<Category> ListAll()
        {
            return db.Categories.ToList();
        }

        public Category GetByID(int id)
        {
            return db.Categories.Find(id);
        }

        public bool ChangeStatus(long id)
        {
            var category = db.Categories.Find(id);
            category.Status = !category.Status;
            db.SaveChanges();
            return category.Status.Value;
        }

    }
}
