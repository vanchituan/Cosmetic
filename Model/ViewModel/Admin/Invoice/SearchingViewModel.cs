using Common;
using DataLayer.DataAccessObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.ViewModel.Admin.Invoice
{
    public class SearchingViewModel
    {
        public SearchingViewModel()
        {
            this.PageCurrent = CommonConstants.PAGECURRENT;
            this.OrderBy = "InvoiceId";
            this.FromDate = null;
            this.ToDate = null;
            this.PromotionName = new PromotionDao().GetPromotionList();
            this.SortBy = false;
            this.PageSize = CommonConstants.PAGESIZE;
        }

        public int? InvoiceId { get; set; }
        public string ShipName { get; set; }
        public string CustomerName { get; set; }
        public string ShipMobile { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PromotionId { get; set; }
        public List<SelectListItem> PromotionName { get; set; }
        public int? PageCurrent { get; set; }
        public string OrderBy { get; set; }
        public bool? SortBy { get; set; }
        public int PageSize { get; set; }
    }
}
