using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ITW_MobileAppService.Startup))]

namespace ITW_MobileAppService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}