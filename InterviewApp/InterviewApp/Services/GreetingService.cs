using InterviewApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace InterviewApp.Services
{
    public class GreetingService(IOptions<GreetingOptions> options, ILogger<GreetingService> logger) : IGreetingService
    {
        private readonly GreetingOptions _options = options.Value;

        public void Run()
        {
            logger.LogInformation("GreetingService started");

            var language = string.IsNullOrEmpty(_options.Language) ? "English" : _options.Language.Trim();

            var greetings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                 { "English", "Hello" },
                 { "Afrikaans", "Hallo" },
                 { "Zulu", "Sawubona" }
            };

            if (!greetings.TryGetValue(language, out var hello))
            {
                logger.LogWarning("Unsupported language '{Language}'. Falling back to English.", language);
                hello = greetings["English"]; // fallback
            }

            logger.LogInformation("Displaying greeting: {Greeting}", hello);
            Console.WriteLine($"{hello}! {_options.Message}");
        }
    }
}