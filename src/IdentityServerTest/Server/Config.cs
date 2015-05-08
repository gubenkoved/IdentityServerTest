using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    using System.Security.Claims;
    using Thinktecture.IdentityServer.Core;
    using Thinktecture.IdentityServer.Core.Models;
    using Thinktecture.IdentityServer.Core.Services.InMemory;

    static class Scopes
    {
        public static List<Scope> Get()
        {
            return StandardScopes.All.Union(
                new List<Scope>
                {
                    new Scope
                    {
                        Name = "api1"
                    },
                    new Scope
                    {
                        Name = "custom",
                        Type = ScopeType.Identity,
                        Claims = new List<ScopeClaim>()
                        {
                            new ScopeClaim("custom", true),
                        }
                    }
                }).ToList();
        }
    }

    static class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                // no human involved
                new Client
                {
                    ClientName = "Silicon-only Client",
                    ClientId = "silicon",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Reference,

                    Flow = Flows.ClientCredentials,
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256())
                    }
                },

                // human is involved
                new Client
                {
                    ClientName = "Silicon on behalf of Carbon Client",
                    ClientId = "carbon",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    Flow = Flows.ResourceOwner,
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret("21B5F798-BE55-42BC-8AA8-0025B903DC3B".Sha256())
                    }
                },

                //
                new Client 
                {
                    Enabled = true,
                    ClientName = "MVC Client",
                    ClientId = "mvc",
                    Flow = Flows.Implicit,
                    RequireConsent = false,
                    
                    RedirectUris = new List<string>
                    {
                        "http://localhost:1656/Home/SignIn"
                    }
                }
            };
        }
    }


    static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "bob",
                    Password = "secret",
                    Subject = "1",
                    

                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Bob"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Smith"),
                        new Claim(Constants.ClaimTypes.Email, "test@test.test"),
                        new Claim("custom", "value"),
                    }
                },
                new InMemoryUser
                {
                    Username = "alice",
                    Password = "secret",
                    Subject = "2"
                },
            };
        }
    }
}
