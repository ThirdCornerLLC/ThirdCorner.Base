using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace ThirdCorner.Base.Controllers
{
    public class BaseController<T> : Controller where T : DataContext, new()
    {
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(BaseController<T>));

        protected bool SubmitChanges = true;

        #region DataContext stuff

        private T _dataContext = null;
        protected T DataContext
        {
            get
            {
                if (_dataContext == null)
                    _dataContext = GetDataContext();
                return _dataContext;
            }
        }

        protected virtual T GetDataContext()
        {
            // This is OK, but Model.BaseModelDataContext.GetDataContext() would be better
            return new T();
        }

        #endregion

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // auto commit transaction on successful action completion
            if (filterContext.Exception == null)
            {
                if (SubmitChanges && (DataContext != null && (
                    DataContext.GetChangeSet().Deletes.Any() ||
                    DataContext.GetChangeSet().Inserts.Any() ||
                    DataContext.GetChangeSet().Updates.Any())))
                    DataContext.SubmitChanges();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["m"]))
                ViewData["Message"] = Decode(Request.QueryString["m"]);
            if (!string.IsNullOrEmpty(Request.QueryString["mt"]))
                ViewData["MessageType"] = Decode(Request.QueryString["mt"]);

            base.OnActionExecuted(filterContext);
        }
        

        #region Error Handling
        protected override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.ExceptionHandled = true;
                filterContext.Result = LogError(filterContext.Exception);
            }
        }

        public ActionResult LogError(Exception e)
        {
            if (e is ThreadAbortException)
                throw e;

            Logger.Error(string.Format("An error occured on {0} at ({1})", GetType().Name,
                (Request == null || Request.Url == null) ? "" : Request.Url.ToString()),
                e);
            TempData["exception"] = e;

            return RedirectToAction("Error");
        }

        public ActionResult LogError(string message, Exception e)
        {
            if (e is ThreadAbortException)
                throw e;

            Logger.Error(string.Format("{0}: {1} at ({2})", GetType().Name, message,
                (Request == null || Request.Url == null) ? "" : Request.Url.ToString()),
                e);
            TempData["exception"] = e;
            return RedirectToAction("Error");
        }

        public ActionResult Error()
        {
            return View();
        }

        #endregion

        #region string encode/decode

        protected string Encode(string str)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(str));
        }

        protected string Decode(string str)
        {
            try
            {
                return Encoding.ASCII.GetString(Convert.FromBase64String(str));
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region Export to CSV Helpers
        protected string CsvFriendly(string s)
        {
            return string.IsNullOrEmpty(s) ? string.Empty : s.Replace("\"", "\"\"");
        }
        protected string GetCSVLine(IEnumerable<string> values)
        {
            return string.Format("\"{0}\"",
                string.Join("\",\"", values.Select(i => (i ?? "").Replace("\"", "\"")).ToArray()));
        }
        #endregion
        
    }
}