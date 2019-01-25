using LWBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWBlog.Helpers
{
    public class BPHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static List<BlogPost> GetRecentPosts()
        {
            var recentPosts = new List<BlogPost>();
            foreach(var post in db.BlogPosts.Where(b => b.Published).ToList())
            {
                if ((DateTime.Now - post.Created).TotalDays <=30)
                {
                    recentPosts.Add(post);
                }
            }
            return recentPosts.OrderByDescending(b => b.Created).ToList();
            //dateForButton.Subtract(TimeSpan.FromDays(1));
            //var allPosts = db.BlogPosts.Where(b => b.Published == true);
            //var recentPosts = allPosts.Where(b => (DateTime.Now - b.Created).TotalDays <= 30);
            //return recentPosts.ToList();
            //return db.BlogPosts.Where(b => (b.Created - DateTime.Now).TotalDays <= 30).OrderByDescending(b => b.Created).ToList();
        }
    }
}