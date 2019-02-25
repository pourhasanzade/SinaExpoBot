using Microsoft.Owin;
using Owin;
using SinaExpoBot;

[assembly: OwinStartup(typeof(Startup))]

namespace SinaExpoBot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
