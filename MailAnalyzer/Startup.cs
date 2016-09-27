using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MailAnalyzer.Startup))]
namespace MailAnalyzer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
