using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Google;
using System.Web;
using System.Collections.Generic;

[assembly: OwinStartup(typeof(WebApplication2.Startup1))]

namespace WebApplication2
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "idsrv",
                Authority = "https://localhost:44319/",
                ClientId = "mvc",
                RedirectUri = "http://localhost:1656/Home/SignIn",
                ResponseType = "id_token",
                
                SignInAsAuthenticationType = "Cookies",

                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.OwinContext.Authentication.AuthenticationResponseChallenge != null)
                        {
                            var authenticationProperties = notification.OwinContext.Authentication.AuthenticationResponseChallenge.Properties;

                            string[] passThroughParams = new[] { "login_hint", "hd", "acr_values" };

                            foreach (var param in passThroughParams)
                            {
                                if (authenticationProperties.Dictionary.ContainsKey(param))
                                {
                                    notification.ProtocolMessage.SetParameter(param, authenticationProperties.Dictionary[param]);
                                }
                            }
                        }

                        return Task.FromResult(0);
                    }
                }

            });

            app.UseGoogleAuthentication(new Microsoft.Owin.Security.Google.GoogleOAuth2AuthenticationOptions()
            {
                AuthenticationType = "Google",
                ClientId = "test",
                ClientSecret = "test",
                Caption = "Google",
                SignInAsAuthenticationType = "Cookies",

                Provider = new CustomGoogleAuthProvider(),
            });
        }
    }

    class CustomGoogleAuthProvider : GoogleOAuth2AuthenticationProvider
    {
        public CustomGoogleAuthProvider()
        {
            OnApplyRedirect = (GoogleOAuth2ApplyRedirectContext context) =>
            {
                IDictionary<string, string> props = context.OwinContext.Authentication.AuthenticationResponseChallenge.Properties.Dictionary;

                string newRedirectUri = context.RedirectUri;

                string[] paramertsToPassThrough = new[] { "login_hint", "hd", "anything" };
                
                foreach (var param in paramertsToPassThrough)
                {
                    if (props.ContainsKey(param))
                    {
                        newRedirectUri += string.Format("&{0}={1}", param, HttpUtility.UrlEncode(props[param]));
                    }
                }

                context.Response.Redirect(newRedirectUri);
            };
        }
    }
}
