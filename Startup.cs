using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AlfaAccounting.Startup))]
namespace AlfaAccounting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
