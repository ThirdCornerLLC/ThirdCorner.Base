using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ThirdCorner.Base.Models;

namespace ThirdCorner.Base.Controllers
{
    public class ExampleController : BaseController<MockContext>
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DoSearch(int pageIndex, int pageCount, string sortColumn, string sortDirection, FormCollection form)
        {
            var users = GetUsers(form, sortColumn, sortDirection);

            var count = users.Count();
            pageCount = count / 20 + (count % 20 > 0 ? 1 : 0);
            var pagedUsers = users.Skip(pageIndex * 20).Take(20);

            return PartialView(new ExampleViewModel
            {
                Users = pagedUsers,
                PageCount = pageCount,
                PageIndex = pageIndex,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            });
        }

        public ActionResult Export(string sortColumn, string sortDirection, FormCollection form)
        {
            var s = GetUsers(form, sortColumn, sortDirection);
            var sb = new StringBuilder();
            sb.AppendLine("Name,UserName,IsAdmin");

            foreach (var t in s)
                sb.AppendLine(
                    GetCSVLine(new []
                    {
                        t.FullName,
                        t.UserName,
                        t.IsAdmin? "Yes" : "No"
                    }));
            return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "Users.csv");
        }


        private IEnumerable<User> GetUsers(FormCollection form, string sortColumn, string sortDirection)
        {
            var users = DataContext.Users.AsQueryable();
            
            if ( !string.IsNullOrEmpty(form["username"] ) )
                users = users.Where(u => u.FullName.Contains(form["username"]));

            return users.OrderBy(sortColumn + "  " + sortDirection);
        }

        public ActionResult Edit(int id)
        {
            SubmitChanges = false;
            User user;
            if (id == 0)
            {
                user = new User {UserId = 0};
            }
            else
            {
                user = DataContext.Users.Single(u => u.UserId == id);
            }
            return View(new EditExampleViewModel {User = user});
        }

        public ActionResult Save(int id, FormCollection form)
        {
            User user;
            if (id == 0)
            {
                user = new User {UserId = 0};
                // TODO: Comment out below
                //DataContext.Users.InsertOnSubmit(user);
            }
            else
            {
                user = DataContext.Users.Single(u => u.UserId == id);
            }
            user.FullName = form["fullName"];
            user.UserName = form["userName"];
            user.IsAdmin = !string.IsNullOrEmpty(form["isAdmin"]);
            // not required
            // DataContext.SubmitChanges();
            return RedirectToAction("Index");
        }

    }
}
