using Microsoft.Extensions.Logging;
using System;

namespace InterviewApp.Services;

public class TimeGreetingService(ILogger<TimeGreetingService> logger) : ITimeGreetingService
{
    public string GetGreeting()
    {
        try
        {
            var hour = DateTime.Now.Hour;
            var greeting = hour switch
            {
                < 12 => "Good morning",
                < 18 => "Good afternoon",
                _ => "Good evening"

            };

            logger.LogInformation("Time-based greeting for hour {Hour}: {Greeting}", hour, greeting);
            return greeting;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to determine a time-based greeting. Falling back to a generic hello.");
            return "Hello";
        }
    }
}
