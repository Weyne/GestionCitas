using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GestionCitas.Presentacion.Startup))]
namespace GestionCitas.Presentacion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
