using LWBlog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace LWBlog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.search = searchStr;
            var blogList = IndexSearch(searchStr);
            //int? page
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            //var listPosts = db.BlogPosts.AsQueryable();
            //return PagedList(posts.ToPagedList(pageNumber,pageSize));
            return View(blogList.ToPagedList(pageNumber,pageSize));
        }


        public IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            IQueryable<BlogPost> result = null;
            if(searchStr != null)
            {
                result = db.BlogPosts.AsQueryable();
                result = result.Where(p => p.Title.Contains(searchStr) ||
                    p.Body.Contains(searchStr) ||
                    p.Comments.Any(c => c.Body.Contains(searchStr) ||
                    c.Author.FirstName.Contains(searchStr) ||
                    c.Author.LastName.Contains(searchStr) ||
                    c.Author.DisplayName.Contains(searchStr) ||
                    c.Author.Email.Contains(searchStr)));
            }
            else
            {
                result = db.BlogPosts.AsQueryable();
            }
            return result.OrderByDescending(p => p.Created);
        }








        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";
            EmailModel model = new EmailModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email from: <bold>{0}</bold>({1})</p><p>Message:</p><p>{2}</p>";
                    var emailTo = ConfigurationManager.AppSettings["emailTo"];
                    var from = string.Format("LWBlog<{0}>", emailTo);
                    model.Body = model.Body;
                    var htmlBody = new MvcHtmlString(model.Body);
                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject,
                        Body = string.Format(body, model.FromName, model.FromEmail, htmlBody),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }
    }
}
