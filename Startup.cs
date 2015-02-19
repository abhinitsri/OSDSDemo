using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebDoImport.Startup))]
namespace WebDoImport
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
