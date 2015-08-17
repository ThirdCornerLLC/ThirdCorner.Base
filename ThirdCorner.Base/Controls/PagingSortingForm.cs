using System;
using System.Web.Mvc;


namespace ThirdCorner.Base.Controls
{
    public static class PagingSortingFormExtensions
    {

        public class PagingSortingForm : IDisposable
        {
            protected HtmlHelper Helper;

            public string SearchUrl { get; set; }
            public string ExportExcelUrl { get; set; }
            public string DefaultSortColumn { get; set; }
            public bool DontRenderResultsDiv { get; set; }

            public PagingSortingForm()
            {
            }

            public PagingSortingForm(HtmlHelper helper, string searchUrl, string exportExcelUrl, string defaultSortColumn, bool dontRenderResultsDiv)
            {
                Helper = helper;
                SearchUrl = searchUrl;
                ExportExcelUrl = exportExcelUrl;
                DefaultSortColumn = defaultSortColumn;
                DontRenderResultsDiv = dontRenderResultsDiv;

                Render();
            }
            protected void Render()
            {
                Helper.ViewContext.Writer.Write("<form id='searchForm' onsubmit='return doSearch(true, false);'>");
            }


            public void Dispose()
            {
                Helper.ViewContext.Writer.Write("<input type='hidden' id='pageIndex' name='pageIndex' value='0' />");
                Helper.ViewContext.Writer.Write("<input type='hidden' id='pageCount' name='pageCount' value='0' />");
                Helper.ViewContext.Writer.Write("<input type='hidden' id='sortColumn' name='sortColumn' value='{0}' />", DefaultSortColumn);
                Helper.ViewContext.Writer.Write("<input type='hidden' id='sortDirection' name='sortDirection' value='asc' />");
                Helper.ViewContext.Writer.Write("</form>");
                if (!DontRenderResultsDiv)
                    Helper.ViewContext.Writer.Write("<div id='results'></div>\r\n");
                Helper.ViewContext.Writer.Write("<script>");
                Helper.ViewContext.Writer.Write("var defaultSortColumn = '{0}';\r\n", DefaultSortColumn);
                Helper.ViewContext.Writer.Write("var searchUrl = '{0}';\r\n", System.Web.VirtualPathUtility.ToAbsolute(SearchUrl));
                Helper.ViewContext.Writer.Write("var exportExcelUrl = '{0}';\r\n", System.Web.VirtualPathUtility.ToAbsolute(ExportExcelUrl));
                Helper.ViewContext.Writer.Write("var loadingUrl = '{0}';\r\n", System.Web.VirtualPathUtility.ToAbsolute("~/content/loading.gif"));
                Helper.ViewContext.Writer.Write("</script>");
            }
        }

        public static IDisposable PagingSortingFormGetHtml(this HtmlHelper self, string searchUrl, string exportExcelUrl, string defaultSortColumn, bool dontRenderResultsDiv)
        {
            return new PagingSortingForm(self, searchUrl, exportExcelUrl, defaultSortColumn, dontRenderResultsDiv);
        }
        
        

        

    }
}