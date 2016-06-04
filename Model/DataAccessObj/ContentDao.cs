using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using DataLayer.ViewModel;
using Common;

namespace DataLayer.DataAccessObj
{
    public class ContentDao
    {
        OnlineShopDBContext db = null;
        public static string USER_SESSION = "USER_SESSION";
        public ContentDao()
        {
            db = new OnlineShopDBContext();
        }

        public bool Delete(int id)
        {
            var content = db.Contents.Find(id);
            db.Contents.Remove(content);
            db.SaveChanges();
            return true;
        }

        public List<ContentViewModel> ListAll(int top)
        {
            var allCate = db.Categories.ToList();
            List<ContentViewModel> result = new List<ContentViewModel>();
            foreach (var cate in allCate)
            {

                ContentViewModel model = new ContentViewModel();
                model.Category = cate;
                model.Contents = db.Contents.Where(x => x.CategoryId == cate.Id).Take(top).ToList();
                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentNewID"></param>
        /// <param name="currentCateID"></param>
        /// <returns></returns>
        public List<Content> ListProContentRelated(int currentNewID, int currentCateID, int top)
        {
            return db.Contents.Where(x => x.Id != currentNewID && x.CategoryId == currentCateID).Take(top).ToList();
        }


        /// <summary>
        /// trả về danh sách tin tức theo metattile với metattile là tham số truyền vào
        /// </summary>
        /// <param name="id">CateID</param>
        /// <returns></returns>
        public List<Content> ContentCategory(int cateID, ref int totalRecord, int page = 1, int pageSize = 4)
        {

            totalRecord = db.Contents.Where(x => x.CategoryId == cateID).Count();
            var model = db.Contents.Where(x => x.CategoryId == cateID).OrderByDescending(p => p.CreatedDate).ToPagedList(page, pageSize).ToList();
            return model;
        }


        /// <summary>
        /// trả về danh sách ContentViewModel có cateMetaTitle la tham so truyen vao
        /// </summary>
        /// <param name="cateMetaTitle">CateMetaTitle</param>
        /// <returns></returns>
        public List<Content> ListOther(int currentCateID, int top)
        {
            var model = db.Contents.Where(x => x.CategoryId != currentCateID).Take(top).ToList();
            return model;
        }

        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);


        }


        /// <summary>
        /// List all content for client
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Content> ListAllPaging(int page, int pageSize)
        {
            //IQueryable<Content> model = db.Contents;
            return db.Contents.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContentTags
                         on a.Id equals b.ContentId
                         where b.TagId == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreatedDate,
                             CreatedBy = a.CreatedBy,
                             ID = a.Id,
                             CategoryID = a.CategoryId

                         }).AsEnumerable().Select(x => new Content()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreatedDate = x.CreatedDate,
                             CreatedBy = x.CreatedBy,
                             Id = x.ID,
                             CategoryId = x.CategoryID
                         });
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// trả về content có metaTitle là tham số truyền vào
        /// </summary>
        /// <param name="metaTitle">MetaTitle</param>
        /// <returns></returns>
        public Content GetByMetaTitle(string metaTitle)
        {
            return db.Contents.SingleOrDefault(x => x.MetaTitle == metaTitle);
        }

        public Content GetByID(long id)
        {
            return db.Contents.Find(id);

        }

        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }

        public void Create(Content content)
        {
            //Xử lý alias
            content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            content.CreatedDate = DateTime.Now;
            content.ViewCount = 0;
            db.Contents.Add(content);
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(content.Id, tagId);

                }
            }


        }
        public long Edit(Content entity)
        {

            var content = db.Contents.Find(entity.Id);
            //Xử lý alias
            content.Name = entity.Name;
            content.Description = entity.Description;
            content.Detail = entity.Detail;
            content.Image = entity.Image;
            content.MetaTitle = StringHelper.ToUnsignString(entity.Name);
            content.CreatedDate = DateTime.Now;
            content.Status = entity.Status;
            content.CategoryId = entity.CategoryId;
            db.SaveChanges();

            //Xử lý tag
            if (!string.IsNullOrEmpty(entity.Tags))
            {
                this.RemoveAllContentTag(entity.Id);
                string[] tags = entity.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    //insert to to tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    //insert to content tag
                    this.InsertContentTag(entity.Id, tagId);

                }
            }

            return entity.Id;
        }
        public void RemoveAllContentTag(long contentId)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentId == contentId));
            db.SaveChanges();
        }
        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }

        public void InsertContentTag(int contentId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentId = contentId;
            contentTag.TagId = tagId;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }

        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContentTags
                         on a.ID equals b.TagId
                         where b.ContentId == contentId
                         select new
                         {
                             ID = a.ID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }


        public bool ChangeStatus(long id)
        {
            var obj = db.Contents.Find(id);
            obj.Status = !obj.Status;
            db.SaveChanges();
            return obj.Status;
        }
    }
}
