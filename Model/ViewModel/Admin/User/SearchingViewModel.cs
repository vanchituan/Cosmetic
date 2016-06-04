using Common;
using DataLayer.DataAccessObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.ViewModel.Admin.User
{
    public class SearchingViewModel
    {
        public SearchingViewModel()
        {
            this.UserGroupList = new UserGroupDao().GetListForDdl();
            this.PromotionList = new PromotionDao().GetPromotionList();
            this.StatusName = new UserDao().GetStatusUser();
            this.Status = null;


            this.SortBy = false;
            this.OrderBy = "UserId";
            this.PageCurrent = CommonConstants.PAGECURRENT;
            this.PageSize = CommonConstants.PAGESIZE;
        }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int? ProvinceId { get; set; }
        public List<SelectListItem> ProvinceList { get; set; }
        public int? DistrictId { get; set; }
        public List<SelectListItem> DistrictList { get; set; }
        public int? Precinctid { get; set; }
        public List<SelectListItem> PrecinctList { get; set; }
        public string UserGroupId { get; set; }
        public List<SelectListItem> UserGroupList { get; set; }

        public int? PromotionId { get; set; }
        public List<SelectListItem> PromotionList { get; set; }
        public bool? Status { get; set; }
        public List<SelectListItem> StatusName { get; set; }

        public int? PageCurrent { get; set; }
        public string OrderBy { get; set; }
        public bool? SortBy { get; set; }
        public int PageSize { get; set; }
    }
}
