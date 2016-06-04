using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Framework;

namespace DataLayer
{
    public class CategoryModel
    {
        public OnlineShopDBContext myDB = null;
        public CategoryModel()
        {
            myDB = new OnlineShopDBContext();
        }

        public List<ProductCategory> ListAll()
        {
            var list =(from c in myDB.ProductCategories
                       select c).ToList();
            return list;
        }

        public void Insert(string name)
        {
            var paras = new SqlParameter[]
            {
                new SqlParameter("@Name",name)
            };
            myDB.Database.ExecuteSqlCommand("sp_insertProCate @Name", paras);

        }

        public void Delete(long id)
        {
            var paras = new SqlParameter[]
            {
                new SqlParameter("@ID",id)
            };
            myDB.Database.ExecuteSqlCommand("sp_deleteProCate @ID",paras);
        }



    }
}
