using InterviewApp.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace InterviewApp.Services
{
    public class GreetingService(IOptions<GreetingOptions> options) : IGreetingService
    {
        private readonly GreetingOptions _options = options.Value;

        public void Run()
        {

            var language = string.IsNullOrEmpty(_options.Language) ? "English" : _options.Language.Trim();

            var greetings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                 { "English", "Hello" },
                 { "Afrikaans", "Hallo" },
                 { "Zulu", "Sawubona" }
            };

            if (!greetings.TryGetValue(language, out var hello))
            {
                hello = greetings["English"]; // fallback
            }

            Console.WriteLine($"{hello}! {_options.Message}");
        }
    }
}