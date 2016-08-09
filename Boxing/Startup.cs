using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Boxing.Startup))]
namespace Boxing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
