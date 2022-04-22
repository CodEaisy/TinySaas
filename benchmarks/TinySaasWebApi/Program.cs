using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CodEaisy.TinySaas.Extensions;

namespace TinySaasWebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                // add multitenant support
                .ConfigureMultitenancy<MultitenantStartup, SimpleTenant>();
    }
}
