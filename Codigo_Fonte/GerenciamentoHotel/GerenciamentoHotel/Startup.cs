using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GerenciamentoHotel.Startup))]
namespace GerenciamentoHotel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
