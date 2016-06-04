using Common;
using DataLayer.DataAccessObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.ViewModel.Admin.Order
{
    public class SearchingViewModel
    {
        public SearchingViewModel()
        {
            this.PageCurrent = CommonConstants.PAGECURRENT;
            this.OrderBy = "OrderId";
            this.FromDate = null;
            this.ToDate = null;
            this.PromotionName = new PromotionDao().GetPromotionList();
            this.OrderStatus = new OrderStatusDao().GetListForDdl();
            this.SortBy = false;
            this.IsExported = false;
            this.PageSize = CommonConstants.PAGESIZE;
        }

        public int? OrderId { get; set; }
        public string ShipName { get; set; }
        public string CustomerName { get; set; }
        public string ShipMobile { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? PrecinctId { get; set; }
        public int? PromotionId { get; set; }
        public int? StatusId { get; set; }
        public List<SelectListItem> PromotionName { get; set; }
        public List<SelectListItem> OrderStatus{ get; set; }
        public bool IsExported { get; set; }
        public int? PageCurrent { get; set; }
        public string OrderBy { get; set; }
        public bool? SortBy { get; set; }
        public int PageSize { get; set; }
    }
}
