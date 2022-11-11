using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Soundbox.Reloaded.Ui.Web.App_Start.Startup))]

namespace Soundbox.Reloaded.Ui.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
