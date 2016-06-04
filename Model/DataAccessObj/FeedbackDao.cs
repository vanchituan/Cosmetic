using DataLayer.Framework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class FeedbackDao
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        public IEnumerable<Feedback> ListAllPaging(int page, int pageSize)
        {
            return db.Feedbacks.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public bool ChangeStatus(long id)
        {
            var obj = db.Feedbacks.Find(id);
            obj.Status = !obj.Status;
            db.SaveChanges();
            return obj.Status.Value;
        }
    }
}
