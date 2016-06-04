using DataLayer.Framework;
using DataLayer.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class TagDao
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        public IEnumerable<Tag> ListAllPaging(int page, int pageSize)
        {
            return db.Tags.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public List<TagDetailViewModel> GetDetail(string TagID)
        {
            var model = (from a in db.ContentTags
                         join b in db.Tags on a.TagId equals b.ID
                         join c in db.Contents on a.ContentId equals c.Id
                         where b.ID == TagID
                         select new
                         {
                             contentID = a.ContentId,
                             tagID = b.ID,
                             tagName = b.Name,
                             contentName = c.Name
                         }).AsEnumerable()
                        .Select(q => new TagDetailViewModel()
                        {
                            ContentID = q.contentID,
                            ContentName = q.contentName,
                            TagID = q.tagID,
                            TagName = q.tagName
                        }).OrderByDescending(q => q.ContentID).ToList();
            return model;
        }

        public Tag GetByID(string id)
        {
            return db.Tags.SingleOrDefault(x=>x.ID == id);
             
        }
    }
}
