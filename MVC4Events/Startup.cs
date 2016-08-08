using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC4Events.Startup))]
namespace MVC4Events
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
