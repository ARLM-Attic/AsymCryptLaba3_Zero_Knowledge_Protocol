using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(llll.Startup))]
namespace llll
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
