using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(model.Startup))]
namespace model
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
