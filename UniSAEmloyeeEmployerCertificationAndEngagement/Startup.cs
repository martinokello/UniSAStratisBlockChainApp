using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using UniSAEmloyeeEmployerCertificationAndEngagement.Models;

[assembly: OwinStartupAttribute(typeof(UniSAEmloyeeEmployerCertificationAndEngagement.Startup))]
namespace UniSAEmloyeeEmployerCertificationAndEngagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}
