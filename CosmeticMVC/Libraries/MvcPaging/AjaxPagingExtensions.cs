using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace CosmeticMVC.Libraries.MvcPaging
{
    public static class AjaxPagingExtensions
    {
        public static string Pager(this AjaxHelper ajaxHelper, Options options, AjaxOptions ajaxOptions, object values)
        {

            //Create the new label.
            var div = new TagBuilder("div");
            div.AddCssClass("pagination pagination-normal pagination-right");

            var ul = new TagBuilder("ul");

            var totalPage = options.TotalItemCount <= options.PageSize ? 1 : options.TotalItemCount % options.PageSize == 0 ? options.TotalItemCount / options.PageSize : options.TotalItemCount / options.PageSize + 1;
            if (options.LimitPage != null)
            {
                var from = 1;
                var to = options.TotalItemCount;
                if (totalPage - options.CurrentPage >= options.LimitPage / 2)
                {
                    if (options.CurrentPage > options.LimitPage.Value / 2)
                    {
                        from = options.CurrentPage - (options.LimitPage.Value / 2);
                        to = (options.LimitPage.Value - (options.LimitPage.Value / 2)) + options.CurrentPage;
                    }
                    else
                    {
                        from = 1;
                        to = options.LimitPage.Value + 1;
                    }

                }
                else
                {
                    from = totalPage - options.LimitPage.Value + 1;
                    to = totalPage + 1;
                }
                if (from < 1) from = 1;
                if (to > totalPage) to = totalPage + 1;

                if (options.IsShowFirstLast && from > 1)
                {
                    var link = String.Format(options.Link, 1);
                    var onclick = !string.IsNullOrEmpty(options.OnClick) ? String.Format(options.OnClick, 1) : "";
                    var isActive = 1 == options.CurrentPage ? "active" : "";
                    ul.InnerHtml += String.Format("<li class='{0}'><a href='{1}' data-page='1' onclick='{2}'>{3}</a></li>", isActive, link, onclick, "<span class='glyphicon glyphicon-fast-backward'></span>");
                }
                for (var i = from; i < to; i++)
                {
                    var li = new TagBuilder("li");
                    var isActive = i == options.CurrentPage ? "active" : "";
                    var link = String.Format(options.Link, i);
                    var onclick = !string.IsNullOrEmpty(options.OnClick) ? String.Format(options.OnClick, i) : "";
                    ul.InnerHtml += String.Format("<li class='{0}'><a href='{1}' data-page='{3}' onclick='{2}'>{3}</a></li>", isActive, link, onclick, i);
                }
                if (options.IsShowFirstLast && to - 1 < totalPage)
                {
                    var link = String.Format(options.Link, totalPage);
                    var onclick = !string.IsNullOrEmpty(options.OnClick) ? String.Format(options.OnClick, totalPage) : "";
                    var isActive = totalPage == options.CurrentPage ? "active" : "";
                    ul.InnerHtml += String.Format("<li class='{0}'><a href='{1}' data-page='{4}' onclick='{2}'>{3}</a></li>", isActive, link, onclick, "<span class='glyphicon glyphicon-fast-forward'></span>", totalPage);
                }
            }

            div.InnerHtml = ul.ToString();
            return div.ToString();
        }
    }
}