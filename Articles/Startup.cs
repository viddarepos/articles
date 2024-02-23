using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Articles.Startup))]
namespace Articles
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
