using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ThirdCorner.Base.Models;

namespace ThirdCorner.Base.Controls
{
    public static class MenuExtensions
    {
        private static XmlNamespaceManager _xmlnsManager;

        public static MvcHtmlString Menu(this HtmlHelper helper, bool isMobile = false)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var sb = new StringBuilder();

            if (isMobile)
            {
                sb.Append("<select onchange='location.href=this.value;'>");
                sb.Append("<option value=''></option>");
                sb.AppendFormat("<option {0} value='{1}'>{2}</option>",
                    IsCurrentPage(urlHelper.Content("~/"), HttpContext.Current.Request) ? "selected='selected'" : "",
                    urlHelper.Content("~/"), "Dashboard");
            }
            else
                sb.Append("<ul id='menu'>");
            var siteMap = new FileInfo(HttpContext.Current.Server.MapPath("~/web.sitemap"));
            if (siteMap.Exists)
            {
                var smXDoc = new XmlDocument();
                smXDoc.Load(siteMap.FullName);
                _xmlnsManager = new XmlNamespaceManager(smXDoc.NameTable);
                _xmlnsManager.AddNamespace("mi", "http://schemas.microsoft.com/AspNet/SiteMap-File-1.0");

                var nodes = smXDoc.SelectNodes("/mi:siteMap/mi:siteMapNode/mi:siteMapNode", _xmlnsManager);
                WriteNodes(sb, nodes, urlHelper, isMobile);
            }
            if (isMobile)
                sb.Append("</select>");
            else
                sb.Append("</ul>");


            return new MvcHtmlString(sb.ToString());
        }
        private static void WriteNodes(StringBuilder sb, XmlNodeList nodes, UrlHelper urlHelper, bool isMobile)
        {
            if (nodes != null)
            {
                for (var i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    if (!CanViewNode(node)) continue;


                    if (node.Attributes != null)
                    {
                        var url = urlHelper.Content(node.Attributes["url"].Value);
                        if (isMobile)
                        {
                            sb.AppendFormat("<option {0} value='{1}'>{2}</option>",
                                            IsCurrentPage(url, HttpContext.Current.Request) ? "selected='selected'" : "",
                                            url, node.Attributes["title"].Value);
                        }
                        else
                        {
                            sb.AppendFormat("<li class='{0} {1}'>",
                                            IsCurrentPath(url, HttpContext.Current.Request) ? "current" : "",
                                            IsCurrentPage(url, HttpContext.Current.Request) ? "currentPage" : "");
                            sb.AppendFormat("<a href='{0}'>{1}</a>",
                                            url,
                                            node.Attributes["title"].Value
                                );
                        }

                        var childNodes = node.SelectNodes("mi:siteMapNode", _xmlnsManager);
                        if (childNodes != null && childNodes.Count > 0)
                        {
                            if (!isMobile)
                                sb.Append("<ul class='submenu'>");
                            WriteNodes(sb, childNodes, urlHelper, isMobile);
                            if (!isMobile)
                                sb.Append("</ul>");
                        }
                        if (!isMobile)
                            sb.Append("</li>");
                    }
                }
            }
        }
        private static bool IsCurrentPath(string url, HttpRequest request)
        {
            return request.Url.AbsolutePath.ToLower().Contains(url.ToLower());
        }
        private static bool IsCurrentPage(string url, HttpRequest request)
        {
            return request.Url.AbsolutePath.ToLower() == url.ToLower();
        }
        private static bool CanViewNode(XmlNode node)
        {
            if (node.Attributes == null)
                return true;
            if (node.Attributes["roles"] == null)
                return true;
            var hasRole = false;
            if (node.Attributes["roles"] != null)
            {
                // to check an AD Role, uncomment line below
                //hasRole = node.Attributes["roles"].Value.Split(',').Any(role => HttpContext.Current.User.IsInRole(role));
                hasRole = node.Attributes["roles"].Value == "admin" && LoggedInUser.CurrentUser.IsAdmin;
            }
            return hasRole;
        }
    }
}
