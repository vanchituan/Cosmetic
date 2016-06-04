using DataLayer.Framework;
using DataLayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using Common;
using DataLayer.ViewModel.Admin.Product;
using System.Web.Mvc;

namespace DataLayer.DataAccessObj
{
    public class ProductDao
    {
        OnlineShopDBContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDBContext();
        }

        public ListViewModel GetList(SearchingViewModel search)
        {
            var result = new ListViewModel();
            result.OrderBy = search.OrderBy;
            result.SortBy = search.SortBy;

            var model = from a in db.Products
                        join b in db.ProductCategories on a.CategoryId equals b.Id
                        where (search.Status == null || a.Status == search.Status) && 
                        (search.ProductId == null || a.Id == search.ProductId) &&
                        (string.IsNullOrEmpty(search.ProductName) || a.Name.Contains(search.ProductName)) &&
                        (search.CategoryId == null || a.CategoryId == search.CategoryId) &&
                        (search.FromQuantity == null || a.Quantity >= search.FromQuantity) &&
                        (search.ToQuantity == null || a.Quantity <= search.ToQuantity) &&
                        (search.FromPrice == null || a.Price >= search.FromPrice) &&
                        (search.ToPrice == null || a.Price <= search.ToPrice) && 
                        (search.FromDate == null || a.CreatedDate >= search.FromDate) &&
                        (search.ToDate == null || a.CreatedDate <= search.ToDate) 
                        select new ItemViewModel
                        {
                            Id = a.Id,
                            CategoryName = b.Name,
                            Price = a.Price,
                            ProductName = a.Name,
                            CategoryId = b.Id,
                            Image = a.Image,
                            Quantity = a.Quantity,
                            Status = a.Status.Value
                        };
            #region sort by
            if (result.SortBy.Value)
            {
                switch (result.OrderBy)
                {
                    case "Id":
                        model = model.OrderBy(q => q.Id);
                        break;
                    case "ProductName":
                        model = model.OrderBy(q => q.ProductName);
                        break;
                    case "Price":
                        model = model.OrderBy(q => q.Price);
                        break;
                    case "Quantity":
                        model = model.OrderBy(q => q.Quantity);
                        break;
                    case "Status":
                        model = model.OrderBy(q => q.Status);
                        break;
                    default:
                        model = model.OrderBy(q => q.Id);
                        break;
                }
            }
            else
            {
                switch (result.OrderBy)
                {
                    case "Id":
                        model = model.OrderByDescending(q => q.Id);
                        break;
                    case "ProductName":
                        model = model.OrderByDescending(q => q.ProductName);
                        break;
                    case "Price":
                        model = model.OrderByDescending(q => q.Price);
                        break;
                    case "Quantity":
                        model = model.OrderByDescending(q => q.Quantity);
                        break;
                    case "Status":
                        model = model.OrderByDescending(q => q.Status);
                        break;
                    default:
                        model = model.OrderByDescending(q => q.Id);
                        break;

                }
            }
            #endregion

            result.Items = model.ToPagedList(search.PageCurrent.HasValue ? search.PageCurrent.Value : 1, result.PageSize.HasValue ? search.PageSize : 10);
            result.Total = model.Count();
            return result;
        }



        public List<SelectListItem> GetStatusProduct()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "None", Selected = true });
            list.Add(new SelectListItem { Text = "Đang kinh doanh", Value = "true" });
            list.Add(new SelectListItem { Text = "Ngừng kinh doanh", Value = "false" });
            return list;
        }


        /// <summary>
        /// danh sách tên sản phẩm có từ khóa là tham số truyền vào
        /// </summary>
        /// <param name="keyword">Tên sản phẩm muốn tìm</param>
        /// <returns></returns>
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword) && x.Status == true).Select(x => x.Name).ToList();
        }

        public bool Delete(int id)
        {
            var pro = db.Products.Find(id);
            db.Products.Remove(pro);
            db.SaveChanges();
            return true;
        }


        /// <summary>
        /// trả về danh sách các sản phẩm với top là số sản phẩm hiển thị
        /// </summary>
        /// <param name="top">sô sản phẩm muốn lấy ra</param>
        /// <returns>danh sách các sản phẩm mới</returns>
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(q=>q.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }


        /// <summary>
        /// danh sách các sản phẩm hot với top số sản phẩm hiển thị
        /// </summary>
        /// <param name="top">số sp muốn lấy ra</param>
        /// <returns>danh sách các sản phẩm hot</returns>
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null /*&& x.CreatedDate > DateTime.Now*/&& x.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }


        /// <summary>
        /// danh sách các sản phẩm liên quan với sản phẩm hiện hành
        /// </summary>
        /// <param name="proIDCurrent">mã sp hiện hành</param>
        /// <returns>danh sách các sản phẩm liên quan với sản phẩm hiện hành</returns>
        public List<Product> ListRelatedProducts(long proIDCurrent, int top)
        {
            var product = db.Products.Find(proIDCurrent);
            return db.Products.Where(x => x.Id != proIDCurrent && x.CategoryId == product.CategoryId && x.Status == true).Take(top).ToList();
        }


        /// <summary>
        /// danh sách sp theo mã loại
        /// </summary>
        /// <param name="categoryID">mã loại</param>
        /// <param name="totalRecord">tổng số dòng</param>
        /// <param name="pageIndex">vị trí trang sẽ display khi load</param>
        /// <param name="pageSize">số item trên 1 trang</param>
        /// <returns>danh sách mã loại với mã loại là tham số truyền vào</returns>
        public List<ProductViewModel> ListByCategoryID(long categoryID, ref int totalRecord, int pageIndex, int pageSize)
        {
            totalRecord = db.Products.Where(x => x.CategoryId == categoryID).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryId equals b.Id
                         where a.CategoryId == categoryID && a.Status == true
                         select new
                         {

                             cateMetaTitle = b.MetaTitle,
                             cateName = b.Name,
                             createdDate = a.CreatedDate,
                             id = a.Id,
                             image = a.Image,
                             name = a.Name,
                             metaTitle = a.MetaTitle,
                             price = a.Price,
                             categoryID = a.CategoryId
                         }).AsEnumerable().Select(q => new ProductViewModel()
                         {
                             ProCateMetaTitle = q.metaTitle,
                             ProCateName = q.cateName,
                             CreatedDate = q.createdDate,
                             ID = q.id,
                             Images = q.image,
                             Name = q.name,
                             MetaTitle = q.metaTitle,
                             Price = q.price,
                             CategoryID = int.Parse(q.categoryID.ToString())
                         }).OrderByDescending(p => p.CreatedDate).ToPagedList(pageIndex, pageSize).ToList();
            return model;
            //List<ProductViewModel> result = new List<ProductViewModel>();
            //foreach (var item in result)
            //{
            //    ProductViewModel model = new ProductViewModel();
            //    model.Products = result.
            //}

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="totalRecord"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Name == keyword).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryId equals b.Id
                         where a.Name.Contains(keyword) && a.Status == true
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.Id,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             CategoryID = a.CategoryId
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             ProCateMetaTitle = x.MetaTitle,
                             ProCateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             CategoryID = x.CategoryID.Value
                         });
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }


        /// <summary>
        /// xem chi tiết sản phẩm
        /// </summary>
        /// <param name="id">mã sản phẩm muốn xem chi tiết</param>
        /// <returns></returns>
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

        public Product GetByMetaTitle(string metaTitle)
        {
            return db.Products.SingleOrDefault(x => x.MetaTitle == metaTitle);
        }

        public Product GetByID(int id)
        {
            return db.Products.Find(id);
        }

        public void UpdateQuantity(int productID, int quantity)
        {
            var pro = db.Products.Find(productID);
            pro.Quantity = pro.Quantity - quantity;
            db.SaveChanges();
        }

        public void Create(Product entity)
        {
            Product product = new Product()
            {
                Name = entity.Name,
                CategoryId = entity.CategoryId,
                Description = entity.Description,
                Detail = entity.Detail,
                Image = entity.Image,
                Price = entity.Price,
                Quantity = entity.Quantity,
                Status = entity.Status,
                MetaTitle = StringHelper.ToUnsignString(entity.Name)
            };
            db.Products.Add(product);
            db.SaveChanges();
        }

        public void Edit(Product entity)
        {
            var p = db.Products.Find(entity.Id);
            p.Name = entity.Name;
            p.CategoryId = entity.CategoryId;
            p.Description = entity.Description;
            p.Detail = entity.Detail;
            p.Image = entity.Image;
            p.Price = entity.Price;
            p.PriceImport = entity.PriceImport;
            p.Quantity = entity.Quantity;
            db.SaveChanges();
        }

        public Product UpdateWareHouse(int proID, int quantity)
        {
            var product = db.Products.Find(proID);
            product.Quantity += quantity;
            db.SaveChanges();
            return product;
        }

        public bool ChangeStatus(long id)
        {
            var obj = db.Products.Find(id);
            obj.Status = !obj.Status;
            db.SaveChanges();
            return obj.Status.Value;
        }

        public List<Product> LoadProductsByCateId(int cateId)
        {
            return db.Products.Where(x => x.CategoryId == cateId).ToList();
        }
    }
}
