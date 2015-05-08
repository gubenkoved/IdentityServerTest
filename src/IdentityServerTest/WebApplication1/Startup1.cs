using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Thinktecture.IdentityServer.AccessTokenValidation;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebApplication1.Startup1))]

namespace WebApplication1
{
    public class Startup1
    {
        

        public void Configuration(IAppBuilder app)
        {
            // accept access tokens from identityserver and require a scope of 'api1'
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = "https://localhost:44333",
                    RequiredScopes = new[] { "api1" }
                });

            // configure web api
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            // require authentication for all controllers
            config.Filters.Add(new AuthorizeAttribute());

            app.UseWebApi(config);
        }
    }
}
