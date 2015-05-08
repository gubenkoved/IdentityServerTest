using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Logging;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());

            using (WebApp.Start<Startup>("https://localhost:44319"))
            {
                Console.WriteLine("server running...");
                Console.ReadLine();
            }
        }
    }
}
