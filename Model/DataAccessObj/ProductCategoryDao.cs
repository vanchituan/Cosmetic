using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.DataAccessObj
{
    public class ProductCategoryDao
    {
        OnlineShopDBContext db = null;
        public ProductCategoryDao()
        {
            db = new OnlineShopDBContext();
        }


        public List<SelectListItem> GetListForDdl()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "None", Selected = true });
            var model =  (from a in db.ProductCategories
                                    select new SelectListItem
                                    {
                                        Text = a.Name,Value = a.Id.ToString()
                                    }).ToList();
            list.AddRange(model);
            return list;
        }

        /// <summary>
        /// danh sách tất cả loại sản phẩm sort based on displayOrder 
        /// </summary>
        /// <returns></returns>
        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.OrderBy(x => x.DisplayOrder).ToList();
        }


        /// <summary>
        /// xem trong loại sản phẩm này có những sp nào
        /// trả về những record co id là id truyen vào
        /// </summary>
        /// <param name="id">mã loại</param>
        /// <returns></returns>
        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public bool ChangeStatus(long id)
        {
            var proCate = db.ProductCategories.Find(id);
            proCate.Status = !proCate.Status;
            db.SaveChanges();
            return proCate.Status.Value;
        }
    }
}
