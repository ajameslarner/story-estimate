using StoryEstimate.Context;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;
using System.Timers;
using Timer = System.Timers.Timer;

namespace StoryEstimate.Services;

public class SessionService : ISessionService
{
    private readonly SessionContext _sessionManager;
    private readonly Timer _timer = new(5000);

    public SessionService(SessionContext sessionManager)
    {
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

        if (_sessionManager.TryAdd(sessionId, session))
        {
            return sessionId;
        }

        return default;
    }

    public bool GetSession(string sessionId, out Session session) =>
        _sessionManager.TryGetValue(sessionId, out session);

    public bool RemoveSession(string sessionId) =>
        _sessionManager.TryRemove(sessionId, out _);

    private static string GenerateUniqueSessionId() => 
        Guid.NewGuid().ToString()[..12].Replace("-", "");
}