using Common;
using DataLayer.DataAccessObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.ViewModel.Admin.Product
{
    public class SearchingViewModel
    {
        public SearchingViewModel()
        {
            this.PageCurrent = CommonConstants.PAGECURRENT;
            this.CategoryName = new ProductCategoryDao().GetListForDdl();
            this.OrderBy = "ProductName";
            this.FromDate = null;
            this.ToDate = null;
            this.Status = null;
            this.StatusName = new ProductDao().GetStatusProduct();
            this.SortBy = false;
            this.PageSize = CommonConstants.PAGESIZE;
        }

        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int? FromQuantity { get; set; }
        public int? ToQuantity { get; set; }
        public int? CategoryId { get; set; }
        public List<SelectListItem> CategoryName { get; set; }
        public bool? Status { get; set; }
        public List<SelectListItem> StatusName { get; set; }

        public int? PageCurrent { get; set; }
        public string OrderBy { get; set; }
        public bool? SortBy { get; set; }
        public int PageSize { get; set; }
    }
}
