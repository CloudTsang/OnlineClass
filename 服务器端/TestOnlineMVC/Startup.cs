using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestOnlineMVC.Startup))]
namespace TestOnlineMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
