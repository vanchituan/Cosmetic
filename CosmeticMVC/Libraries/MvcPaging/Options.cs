using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosmeticMVC.Libraries.MvcPaging
{
    public class Options
    {
        public string ActionName;
        public int CurrentPage;
        public int PageSize;
        public int TotalItemCount;
        public string Link = "";
        public string OnClick = "";

        public Options()
        {

        }

        public int? LimitPage = 5;
        public string CssClass { get; set; }
        public bool IsShowControls { get; set; }
        public bool IsShowFirstLast { get; set; }
        public bool IsShowPages { get; set; }
        //public ItemIcon ItemIcon { get; set; }
        //public ItemTexts ItemTexts { get; set; }
        //public TooltipTitles TooltipTitles { get; set; }
    }
}