using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TakeTwo.Startup))]
namespace TakeTwo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
