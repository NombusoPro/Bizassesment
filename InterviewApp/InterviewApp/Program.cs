using InterviewApp.Models;
using InterviewApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {  
                services.Configure<GreetingOptions>(context.Configuration.GetSection("Greeting"));
                services.AddTransient<IGreetingService, GreetingService>(); 
            })
            .Build();

        var greetingService = host.Services.GetRequiredService<IGreetingService>();
        greetingService.Run();

        await host.RunAsync();
    }
}