using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LWBlog.Startup))]
namespace LWBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
