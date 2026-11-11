using InterviewApp.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Features.Greetings;

public sealed class GreetUserHandler(
    ISender sender,
    IGreetingService greetingService,
    ILogger<GreetUserHandler> logger) : IRequestHandler<GreetUserCommand, string>
{
    public async Task<string> Handle(GreetUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //  compose greetings through MediatR rather than calling the time service directly.
            var timeGreeting = await sender.Send(new GetTimeGreetingQuery(), cancellationToken);
            var localizedGreeting = greetingService.GetLocalizedGreeting();

            var combined = $"{localizedGreeting}, {timeGreeting}";
            logger.LogInformation("Combined greeting ready: {Greeting}", combined);

            return combined;
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("GreetUserCommand was cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to build the user greeting.");
            throw;
        }
    }
}
