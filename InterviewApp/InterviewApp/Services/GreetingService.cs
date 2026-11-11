using InterviewApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace InterviewApp.Services;

public class GreetingService(
    IOptions<GreetingOptions> options,
    ILogger<GreetingService> logger) : IGreetingService
{
    private static readonly Dictionary<string, string> Greetings = new(StringComparer.OrdinalIgnoreCase)
    {
        { "English", "Hello" },
        { "Afrikaans", "Hallo" },
        { "Zulu", "Sawubona" }
    };

    private readonly GreetingOptions _options = options.Value;

    public string GetLocalizedGreeting()
    {
        logger.LogInformation("Building localized greeting.");

        try
        {
            var language = string.IsNullOrWhiteSpace(_options.Language)
                ? "English"
                : _options.Language.Trim();

            if (!Greetings.TryGetValue(language, out var hello))
            {
                logger.LogWarning("Unsupported language '{Language}'. Falling back to English.", language);
                hello = Greetings["English"];
            }

            var message = string.IsNullOrWhiteSpace(_options.Message)
                ? string.Empty
                : _options.Message.Trim();

            var greeting = string.IsNullOrEmpty(message)
                ? $"{hello},"
                : $"{hello}";

            logger.LogInformation("Localized greeting for '{Language}': {Greeting}", language, greeting);
            return greeting;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to build the localized greeting. Falling back to English.");
            return "Hello!";
        }
    }
}
