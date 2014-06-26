using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspMvc5Northwind.Startup))]
namespace AspMvc5Northwind
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
