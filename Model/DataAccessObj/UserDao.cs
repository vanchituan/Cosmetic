using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Framework;

using MvcPaging;

using DataLayer.ViewModel;
using Common;
using DataLayer.ViewModel.Admin.User;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Web;

namespace DataLayer.DataAccessObj
{
    public class UserDao
    {
        OnlineShopDBContext db = null;
        public UserDao()
        {
            db = new OnlineShopDBContext();
        }

        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.Id;
            }
            else
            {
                return user.Id;
            }

        }

        /// <summary>
        /// thêm mới user
        /// </summary>
        /// <param name="entity">đối tượng user</param>
        /// <returns>tra ve id của user dang thêm vào</returns>
        public long Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        /// <summary>
        /// Sữa 
        /// </summary>
        /// <param name="entity">Đối tượng user truyền vào</param>
        /// <returns>tra ve true neu sua thanh cong</returns>
        public bool Update(User entity)
        {
            var userDao = new UserDao().GetByUsername(entity.UserName);
            var user = db.Users.Find(userDao.Id);
            user.UserName = entity.UserName;
            user.Name = entity.Name;
            if (!string.IsNullOrEmpty(entity.Password))
            {
                user.Password = entity.Password;
            }
            user.Email = entity.Email;
            user.Phone = entity.Phone;
            user.Address = entity.Address;
            user.Image = entity.Image;
            db.SaveChanges();
            return true;
        }


        /// <summary>
        /// xoa doi tuong user
        /// </summary>
        /// <param name="id">id cua doi tuong user muon xoa</param>
        /// <returns>tra ve true neu xoa thanh cong</returns>
        public bool Delete(int id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// xem chi tiết 1 user
        /// </summary>
        /// <param name="id">id cua user muốn xem chi tiết</param>
        /// <returns>tra ve 1 record của user co id truyen vao</returns>
        public User GetByID(int id)
        {
            return db.Users.Find(id);
        }



        /// <summary>
        /// 1 : ok, 0 : k tồn tại, -1 : bị khóa, -2 : pass k dung, -3 : k co quyen
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="isLoginAdmin"></param>
        /// <returns></returns>
        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupId == CommonConstants.ADMIN_GROUP || result.GroupId == CommonConstants.MOD_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;       
                        else
                            return -2;
                    }
                }
            }
        }


        /// <summary>
        /// trả về các quyền mà user này có, với user là tham số truyền vào
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupId equals b.Id
                        join c in db.Roles on a.RoleId equals c.Id
                        where b.Id == user.GroupId
                        select new
                        {
                            RoleID = a.RoleId,// VIEW_CONTENT
                            UserGroupID = a.UserGroupId
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleId = x.RoleID,
                            UserGroupId = x.UserGroupID
                        });
            return data.Select(x => x.RoleId).ToList();
        }






        /// <summary>
        /// lấy ra user có username truyền vào
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>1 record của user co username truyền vào</returns>
        public User GetByUsername(string username)
        {
            return db.Users.SingleOrDefault(x => x.UserName == username);
        }


        /// <summary>
        /// thay đỗi trạng thái của người dùng
        /// </summary>
        /// <param name="id">id của người dùng muốn thay đỗi trạng thái</param>
        /// <returns>nêu status cua nguoi dung dang true thi tra ve false</returns>
        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }


        /// <summary>
        /// mod la true, mem la false
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChangeUserGroup(long id)
        {
            bool result = true;
            var user = db.Users.Find(id);
            if (user.GroupId == "MEMBER")
            {
                user.GroupId = "MOD";
            }
            else if(user.GroupId == "MOD")
            {
                user.GroupId = "MEMBER";
                result = false;
            }
            db.SaveChanges();
            return result;
        }


        /// <summary>
        /// Cập nhật điểm tích lũy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="total"></param>
        public void UpdatePoint(int id, long total)
        {
            var user = db.Users.Find(id);
            user.Point += total;
            db.SaveChanges();
        }

        public void UpdatePromotionId(int userId)
        {
            var customer = db.Users.Find(userId);

        }

        public List<OrderHisViewModel> OrderHistory(int userID)
        {
            var model = (from a in db.Users
                         join b in db.Orders on a.Id equals b.CustomerId
                         join c in db.Promotions on b.PromotionId equals c.PromotionId
                         where b.CustomerId == userID
                         select new
                         {
                             createddate = b.CreatedDate,
                             address = b.ShipAddress,
                             phone = b.ShipMobile,
                             email = b.ShipEmail,
                             shipname = b.ShipName,
                             total = b.Total,
                             promotionName = c.PromotionName,
                             orderID = b.Id
                         }).AsEnumerable().Select(q => new OrderHisViewModel()
                         {
                             OrderID = q.orderID,
                             CreatedDate = q.createddate.Value,
                             ShipAddress = q.address,
                             ShipEmail = q.email,
                             ShipPhone = q.phone,
                             PromotionName = q.promotionName,
                             ShipName = q.shipname,
                             Total = q.total.Value
                         }).ToList();
            return model;
        }

        public ListViewModel GetList(SearchingViewModel search)
        {
            var result = new ListViewModel();
            result.OrderBy = search.OrderBy;
            result.SortBy = search.SortBy;

            var model = (from a in db.Users
                        join b in db.Promotions on a.PromotionId equals b.PromotionId
                        join c in db.UserGroups on a.GroupId equals c.Id
                        where a.Id != 0 &&
                        (search.UserId == null || a.Id == search.UserId) &&
                        (string.IsNullOrEmpty(search.Name) || a.Name.Contains(search.Name)) &&
                        (string.IsNullOrEmpty(search.Username) || a.Name.Contains(search.Username)) &&
                        (string.IsNullOrEmpty(search.Mobile) || a.Phone.Contains(search.Mobile)) &&
                        (string.IsNullOrEmpty(search.Address) || a.Address.Contains(search.Address)) &&
                        (string.IsNullOrEmpty(search.Email) || a.Name.Contains(search.Email)) &&
                        (search.PromotionId == null || a.PromotionId == search.PromotionId) &&
                        (string.IsNullOrEmpty(search.UserGroupId) || c.Id == search.UserGroupId)&&
                        (search.Status == null || a.Status == search.Status) &&
                        (search.ProvinceId == null || a.ProvinceId == search.ProvinceId) &&
                        (search.DistrictId == null || a.DistrictId == search.DistrictId) &&
                        (search.Precinctid == null || a.PrecinctId == search.Precinctid)
                        select new
                        {
                            Id = a.Id,
                            Email = a.Email,
                            CustomerName = a.Name,
                            Username = a.UserName,
                            Address = a.Address,
                            PrecinctId = a.PrecinctId.Value,
                            DistrictId = a.DistrictId.Value,
                            ProvinceId = a.ProvinceId.Value,
                            Mobile = a.Phone,
                            PromotionName = b.PromotionName,
                            Status = a.Status,
                            GroupName = c.Name
                        }).AsEnumerable().
                        Select(x=> new ItemViewModel
                        {
                            Id = x.Id,
                            Email = x.Email,
                            CustomerName = x.CustomerName,
                            Username = x.Username,
                            FullAddress = x.Address + GetAddress(x.PrecinctId,x.DistrictId,x.ProvinceId),
                            Address = x.Address,
                            Mobile = x.Mobile,
                            PromotionName = x.PromotionName,
                            Status = x.Status,
                            GroupName = x.GroupName,
                            ProvinceId = x.ProvinceId,
                            DistrictId = x.DistrictId,
                            PrecinctId = x.PrecinctId
                        });

            #region sort
            if (result.SortBy.Value)
            {
                switch (result.OrderBy)
                {
                    case "Name":
                        model = model.OrderBy(q => q.CustomerName);
                        break;
                    case "PromotionName":
                        model = model.OrderBy(q => q.PromotionName);
                        break;
                    default:
                        model = model.OrderBy(q => q.CustomerName);
                        break;
                }
            }
            else
            {
                switch (result.OrderBy)
                {
                    case "PromotionName":
                        model = model.OrderByDescending(q => q.PromotionName);
                        break;
                    case "Name":
                        model = model.OrderByDescending(q => q.CustomerName);
                        break;
                    default:
                        model = model.OrderByDescending(q => q.CustomerName);
                        break;
                }
            }
            #endregion

            int pageIndex = search.PageCurrent.HasValue ? search.PageCurrent.Value : 1;
            int pageSize = result.PageSize.HasValue ? search.PageSize : 10;
            result.Items = model.ToPagedList(pageIndex, pageSize);
            result.Total = model.Count();
            return result;
        }

        public List<SelectListItem> GetStatusUser()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "None", Selected = true });
            list.Add(new SelectListItem { Text = "Kích hoạt", Value = "true" });
            list.Add(new SelectListItem { Text = "Khóa", Value = "false" });
            return list;
        }

        public List<ProvinceViewModel> LoadProvince()
        {
            var xmlDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(@"~/Assets/Client/Data/Provinces_Data.xml"));
            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            var list = new List<ProvinceViewModel>();
            ProvinceViewModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceViewModel();
                province.ProvinceId = int.Parse(item.Attribute("id").Value);
                province.ProvinceName = item.Attribute("value").Value;
                list.Add(province);

            }
            return list;
        }

        public List<DistrictViewModel> LoadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));
            var xElement = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);
            var list = new List<DistrictViewModel>();
            DistrictViewModel district = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictViewModel();
                district.DistrictId = int.Parse(item.Attribute("id").Value);
                district.DistrictName = item.Attribute("value").Value;
                district.ProvinceId = int.Parse(xElement.Attribute("id").Value);
                list.Add(district);
            }
            return list;
        }

        public List<PrecinctViewModel> LoadPrecincts(int provinceId, int districtId)
        {
            var xmlDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));
            var xElement = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceId);
            var precinctList = xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == districtId);
            var resultList = new List<PrecinctViewModel>();
            PrecinctViewModel result = null;
            foreach (var item in precinctList.Elements("Item").Where(x => x.Attribute("type").Value == "precinct"))
            {
                result = new PrecinctViewModel();
                result.PrecinctId = int.Parse(item.Attribute("id").Value);
                result.PrecinctName = item.Attribute("value").Value;
                resultList.Add(result);
            }
            return resultList;
        }

        public string GetAddress(int? precinctId, int? districtId, int? provinceId)
        {
            var xmlDoc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(@"~/Assets/Client/Data/Provinces_Data.xml"));
            var xElement = xmlDoc.Element("Root").Elements("Item").Single(
                x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceId);

            string cityName = xElement.LastAttribute.Value;
            string districtName = xElement.Elements("Item").
                Single(q => q.Attribute("type").Value == "district" && int.Parse(q.Attribute("id").Value) == districtId).LastAttribute.Value;
            string precintName = xElement.Elements("Item").Single(a => a.Attribute("type").Value == "district" &&
            int.Parse(a.Attribute("id").Value) == districtId).Elements("Item").
            Single(v => v.Attribute("type").Value == "precinct" &&
            int.Parse(v.Attribute("id").Value) == precinctId).LastAttribute.Value;
            string result = ", " + precintName + ", " + districtName + ", " + cityName;
            return result;
        }
    }
}
