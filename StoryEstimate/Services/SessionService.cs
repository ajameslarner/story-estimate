using Microsoft.Extensions.Options;
using StoryEstimate.Context;
using StoryEstimate.Models.Configurations;
using StoryEstimate.Services.Abstract;
using Session = StoryEstimate.Models.Session;
using Timer = System.Timers.Timer;

namespace StoryEstimate.Services;

public class SessionService : ISessionService
{
    private readonly SessionConfiguration _options;
    private readonly SessionContext _sessionManager;

    public SessionService(SessionContext sessionManager, IOptions<SessionConfiguration> options)
    {
        _options = options.Value;
        _sessionManager = sessionManager;
    }

    public string? CreateSession(string name, bool isPrivate)
    {
        string sessionId = GenerateUniqueSessionId();
        var session = new Session
        {
            Id = sessionId,
            Name = name,
            Private = isPrivate
        };

        session.InitializeTimeout();
        session.OnTimeout += SessionOnTimeout;

        if (_sessionManager.TryAdd(sessionId, session))
        {
            return sessionId;
        }

        return default;
    }

    private void SessionOnTimeout(object? sender, Events.SessionTimeoutEventArgs e)
        => _sessionManager.TryRemove(e.SessionId, out _);

    public bool GetSession(string sessionId, out Session session)
        => _sessionManager.TryGetValue(sessionId, out session);

    private static string GenerateUniqueSessionId()
        => Guid.NewGuid().ToString()[..12].Replace("-", "");
}