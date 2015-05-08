using Microsoft.Owin;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Services.InMemory;

namespace Server
{
    

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var factory = InMemoryFactory.Create(
                scopes: Scopes.Get(),
                clients: Clients.Get(),
                users: Users.Get());

            var options = new IdentityServerOptions
            {
                Factory = factory,
                SiteName = "Self Hosted IdentityServer",

                SigningCertificate = LoadCertificate(),

                AuthenticationOptions = new Thinktecture.IdentityServer.Core.Configuration.AuthenticationOptions
                {
                    IdentityProviders = ConfigureIdentityProviders
                }
            };

            app.UseIdentityServer(options);
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(@"idsrv3test.pfx", "idsrv3test");
        }

        private void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "AzureAD",
                Caption = "Sign-in with Azure AD",
                SignInAsAuthenticationType = signInAsType,

                Authority = "https://login.windows.net/colemanrg.com",
                ClientId = "f33d6bfc-8f97-4faf-a4ec-d8da711ec107"
            });
        }
    }
}
