using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace PhotonRoomListGrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configApp.AddJsonFile( $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange:true);
                    Console.WriteLine(hostContext.HostingEnvironment.EnvironmentName);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls("http://localhost:5003", "https://localhost:5004");
                });
    }
}
