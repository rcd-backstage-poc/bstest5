using System;
using Kraken.ParameterStore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RockLib.Configuration;
using RockLib.Logging.DependencyInjection;
 
namespace bstest5
{
    public static class Program
    {
        private static readonly string environment = Environment.GetEnvironmentVariable("AppSettings__RockLib_Environment");
 
        public static void Main(string[] args)
        {
            Console.WriteLine($"loading config file for the {environment} environment.");
 
            IHostBuilder builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureAppConfiguration(ConfigureConfiguration);
            builder.ConfigureAppConfiguration(config => config.AddJsonFile($"appsettings.{environment}.json", true));
            builder.ConfigureLogging(ConfigureLogging);
 
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
 
            builder.Build().Run();
        }
 
        private static void ConfigureConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.AddKrakenConfiguration();
        }
 
        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            Config.SetRoot(context.Configuration);
            builder.Services.AddLogger();
        }
    }
}
