using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Winton.Extensions.Configuration.Consul;

namespace api_public_backOffice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {            
            var consulHost = Environment.GetEnvironmentVariable("CONSULHOST");
            var consulToken = Environment.GetEnvironmentVariable("CONSULTOKEN");
            var consulDir = Environment.GetEnvironmentVariable("CONSULDIR");
            var consulKey = Environment.GetEnvironmentVariable("CONSULKEY");

            
            if (!string.IsNullOrEmpty(consulHost) && !string.IsNullOrEmpty(consulToken) && !string.IsNullOrEmpty(consulDir) && !string.IsNullOrEmpty(consulKey))
            {
                Console.WriteLine("CONFIGURACION DESDE CONSUL");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"CONSULHOST :{consulHost}");
                Console.WriteLine($"CONSULTOKEN:************");
                Console.WriteLine($"CONSULDIR  :{consulDir}");
                Console.WriteLine($"CONSULKEY  :{consulKey}");
                var path = $"{consulDir}/{consulKey}";

                Console.WriteLine(path);
                return Host
                      .CreateDefaultBuilder(args)
                      .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                      .ConfigureAppConfiguration(
                          builder =>
                          {
                              builder.AddConsul(path,
                                  options => {
                                      options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consulHost); cco.Token = consulToken; };
                                      options.Optional = false;
                                      options.PollWaitTime = TimeSpan.FromSeconds(5);
                                      options.ReloadOnChange = true;
                                  }
                              ).AddConsul(consulDir + "/appsettings.json",
                                  options => {
                                      options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consulHost); cco.Token = consulToken; };
                                      options.Optional = false;
                                      options.PollWaitTime = TimeSpan.FromSeconds(5);
                                      options.ReloadOnChange = true;
                                  }
                              ).AddEnvironmentVariables();
                          });
            }

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            Console.WriteLine("CONFIGURACION DESDE appsettings.json");
            Console.WriteLine("--------------------------------------------------");
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>());
        }
    }
}
