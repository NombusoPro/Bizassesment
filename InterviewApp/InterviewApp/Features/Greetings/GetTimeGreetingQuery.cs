using MediatR;

namespace InterviewApp.Features.Greetings;

// Gets  greeting based on the current time of day.
public sealed record GetTimeGreetingQuery : IRequest<string>;
