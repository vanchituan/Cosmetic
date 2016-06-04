using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class ContactDao
    {
        OnlineShopDBContext db = null;
        public ContactDao()
        {
            db = new OnlineShopDBContext();
        }


        /// <summary>
        /// danh sach các contact co status la true
        /// </summary>
        /// <returns>trả về record co status la true</returns>
        public Contact GetActiveContact()
        {
            return db.Contacts.Single(p => p.Status == true);
        }


        /// <summary>
        /// thêm feedback
        /// </summary>
        /// <param name="fb">đối tượng feedback muốn thêm</param>
        /// <returns>trả về id của đối tượng feedback truyền vào</returns>
        public int InsertFeedBack(Feedback fb)
        {
            db.Feedbacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }

    }
}
