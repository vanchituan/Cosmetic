using Common;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModel.Admin.Order
{
    public class ListViewModel
    {
        public ListViewModel()
        {
            this.PageSize = CommonConstants.PAGESIZE;
        }
        public IPagedList<ItemViewModel> Items { get; set; }
        public int? PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool? SortBy { get; set; }
        public int? Total { get; set; }
    }
}
