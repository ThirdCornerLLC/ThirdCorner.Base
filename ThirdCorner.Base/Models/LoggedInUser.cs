using System.Web;

namespace ThirdCorner.Base.Models
{
    public class LoggedInUser
    {
        public const string SESSION_KEY = "user";
        public static ThirdCorner.Base.Models.User CurrentUser
        {
            get
            {
                return HttpContext.Current.Session == null
                           ? HttpContext.Current.Items[SESSION_KEY] as ThirdCorner.Base.Models.User
                           : HttpContext.Current.Session[SESSION_KEY] as ThirdCorner.Base.Models.User;
            }
        }

    }
}