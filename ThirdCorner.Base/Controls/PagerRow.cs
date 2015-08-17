using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThirdCorner.Base.Models;

namespace ThirdCorner.Base.Controls
{

    public static class PagerRowExtensions
    {
        public static string PagerRowGetHtml(this HtmlHelper self, int colSpan, bool asDiv)
        {
            var sb = new StringBuilder();
            var pagerData = self.ViewData.Model as PagingSortingViewModel;
            if (pagerData == null || pagerData.PageCount <= 0) return "";
            if (asDiv)
                sb.Append("<div class='pager'>");
            else
                sb.AppendFormat("<tr><td class='pager' colspan='{0}' valign='top'>", colSpan);
            if (pagerData.ShowBack)
                sb.AppendFormat("<a href='javascript:gotoPage({0})'>Previous</a>",
                    pagerData.PageIndex - 1);
            else
                sb.Append("Previous");
            for (var i = pagerData.PageIndex <= 2 ? 0 : pagerData.PageIndex - 2;
                i <=
                (pagerData.PageCount <= pagerData.PageIndex + 2 ? pagerData.PageCount - 1 : pagerData.PageIndex + 2);
                i++)
            {
                sb.Append("&nbsp;");
                if (pagerData.PageIndex != i)
                {
                    sb.AppendFormat("<a href='javascript:gotoPage({0})'>{1}</a>", i, i + 1);
                }
                else
                {
                    sb.Append(i + 1);
                }
                sb.Append("&nbsp;");
            }
            if (pagerData.ShowNext)
            {
                sb.Append("<a href='javascript:nextPage()'>Next</a>");
            }
            else
            {
                sb.Append("Next");
            }
            sb.Append("&nbsp; Jump To&nbsp;");
            sb.Append("<select onchange='gotoPage(this.value);' style='width:auto'>");
            for (var i = 0; i < pagerData.PageCount; i++)
            {
                sb.AppendFormat("<option value='{0}' {1}>{2}</option>", i,
                    pagerData.PageIndex == i ? "selected='selected'" : "", i + 1);
            }
            sb.Append("</select>");
            sb.Append(asDiv ? "</div>" : "</td></tr>");
            return sb.ToString();
        }

    }
}