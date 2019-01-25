namespace LWBlog.Migrations
{
    using LWBlog.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LWBlog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LWBlog.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if(!context.Roles.Any(r=>r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //Add the admin user
            if (!context.Users.Any(u => u.Email == "soccerlgw1987@yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "soccerlgw1987@yahoo.com",
                    Email = "soccerlgw1987@yahoo.com",
                    FirstName = "Landon",
                    LastName = "Wyant",
                    DisplayName = "Landon"
                },
                "test87");
            }

            //Associate a user with a role
            var userId = userManager.FindByEmail("soccerlgw1987@yahoo.com").Id;
            userManager.AddToRole(userId, "Admin");


            //var userModerator = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //Add the Moderator user
            if (!context.Users.Any(u => u.Email == "JTwichell@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "JTwichell@Mailinator.com",
                    Email = "JTwichell@Mailinator.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "Da Boss"
                },
                "Abc&123!");
            }

            //Associate a user with a role
            //var moderatorId = userModerator.FindByEmail("JTwichell@Mailinator.com").Id;
            userId = userManager.FindByEmail("JTwichell@Mailinator.com").Id;
            userManager.AddToRole(userId, "Moderator");


            //userId.AddToRole(userId, "Moderator");

        }
    }
}
