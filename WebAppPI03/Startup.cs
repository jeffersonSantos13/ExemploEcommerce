using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppPI03.Startup))]
namespace WebAppPI03
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
