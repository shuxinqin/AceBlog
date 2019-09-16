using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ace.Web.Mvc
{
    public class PagingHelper
    {
        public static string Paging(int totalCount, int pageSize, int currentPage, string hrefFormat, int showPageCount = 10)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"fx-page\">");

            sb.Append("<span class=\"fx-page-btns\">");

            if (currentPage != 1)
            {
                sb.Append(BuildPageBtn(hrefFormat, currentPage - 1, "上一页"));
            }

            int totalPage = 0;
            if (totalCount > 0)
            {
                if ((totalCount % pageSize) == 0)
                {
                    totalPage = totalCount / pageSize;
                }
                else
                {
                    totalPage = (totalCount / pageSize) + 1;
                }
            }

            int min = currentPage - (showPageCount / 2);

            if (min < 1)
                min = 1;

            int max = min + showPageCount;

            if (max > totalPage)
            {
                max = totalPage;
                min = max - showPageCount;
                if (min < 1)
                    min = 1;
            }

            if (min != 1)
            {
                string firstPageBtn = BuildPageBtn(hrefFormat, 1);
                sb.Append(firstPageBtn);
                sb.Append("<span class=\"fx-page-btn\">...</span>");
            }

            for (var i = min; i <= max; i++)
            {
                if (i == currentPage)
                {
                    sb.Append("<span class=\"fx-page-btn fx-page-curr\">" + i + "</span>");
                    continue;
                }

                sb.Append(BuildPageBtn(hrefFormat, i));
            }

            if (max != totalPage)
            {
                sb.Append("<span class=\"fx-page-btn\">...</span>");
                sb.Append(BuildPageBtn(hrefFormat, totalPage));
            }

            if (currentPage < totalPage)
            {
                sb.Append(BuildPageBtn(hrefFormat, currentPage + 1, "下一页"));
            }

            sb.Append("</span>");


            sb.Append("</div>");

            return sb.ToString();
        }

        static string BuildPageBtn(string hrefFormat, int pageTo, string btnText = null)
        {
            string href = string.Format(hrefFormat, pageTo);
            string btn = $"<a class=\"fx-page-btn\" href=\"{href}\">{btnText ?? pageTo.ToString()}</a>";
            return btn;
        }
    }
}
