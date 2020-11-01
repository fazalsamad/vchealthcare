using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VCHeathCare.Startup))]
namespace VCHeathCare
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
