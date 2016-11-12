using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaPruebas.Startup))]
namespace SistemaPruebas
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
