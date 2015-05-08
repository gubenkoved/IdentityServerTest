using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Client;

namespace ConsoleApplication1
{
    class Program
    {
        static TokenResponse GetToken()
        {
            var client = new OAuth2Client(
                new Uri("https://localhost:44333/connect/token"),
                "silicon",
                "F621F470-9731-4A25-80EF-67A6F7C5F4B8");

            return client.RequestClientCredentialsAsync("api1").Result;
        }

        static void CallApi(TokenResponse response)
        {
            var client = new HttpClient();
            client.SetBearerToken(response.AccessToken);

            Console.WriteLine(client.GetStringAsync("http://localhost:14869/test").Result);
        }

        static void Main(string[] args)
        {
            var client = new OAuth2Client(
                new Uri("https://localhost:44319/connect/token"),
                "carbon",
                "21B5F798-BE55-42BC-8AA8-0025B903DC3B");

            var token = client.RequestResourceOwnerPasswordAsync(
                "bob",
                "secret",
                "openid profile email phone custom")
                .Result;

            var at = token.AccessToken;

            var userInfoClient = new UserInfoClient(
                new Uri("https://localhost:44319/connect/userinfo"),
                at);

            var userInfoResult = userInfoClient.GetAsync().Result;

            Console.WriteLine(userInfoResult.Raw);
        }
    }
}
