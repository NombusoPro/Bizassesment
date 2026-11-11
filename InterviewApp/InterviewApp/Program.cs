using InterviewApp.Features.Greetings;
using InterviewApp.Models;
using InterviewApp.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task<int> Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<GreetingOptions>(context.Configuration.GetSection("Greeting"));
                services.AddTransient<IGreetingService, GreetingService>();
                services.AddTransient<ITimeGreetingService, TimeGreetingService>();
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            })
            .Build();

        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            var options = host.Services.GetRequiredService<IOptions<GreetingOptions>>().Value;

            if (string.IsNullOrWhiteSpace(options.Message) || string.IsNullOrWhiteSpace(options.Language))
            {
                logger.LogError("Invalid configuration: Greeting:Message and Greeting:Language are required.");
                return 1;
            }

            var mediator = host.Services.GetRequiredService<IMediator>();
            var greeting = await mediator.Send(new GreetUserCommand());

            Console.WriteLine(greeting);
            return 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "The application failed while producing a greeting.");
            return 1;
        }
    }
}
