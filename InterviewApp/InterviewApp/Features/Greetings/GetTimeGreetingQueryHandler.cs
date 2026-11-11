using InterviewApp.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Features.Greetings;

// Resolves a time-of-day greeting using <see cref="ITimeGreetingService"/>.
public sealed class GetTimeGreetingQueryHandler(
    ITimeGreetingService timeGreetingService,
    ILogger<GetTimeGreetingQueryHandler> logger) : IRequestHandler<GetTimeGreetingQuery, string>
{
    public Task<string> Handle(GetTimeGreetingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var greeting = timeGreetingService.GetGreeting();
            logger.LogInformation("Time-based greeting resolved: {TimeGreeting}", greeting);

            return Task.FromResult(greeting);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("GetTimeGreetingQuery was cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to resolve a time-based greeting.");
            throw;
        }
    }
}
