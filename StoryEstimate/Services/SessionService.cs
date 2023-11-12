using StoryEstimate.Context;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;

namespace StoryEstimate.Services;

public class SessionService : ISessionService
{
    private readonly SessionContext _sessionManager;

    public SessionService(SessionContext sessionManager)
    {
        this._sessionManager = sessionManager;
    }

    public string? CreateSession(string name)
    {
        string sessionId = GenerateUniqueSessionId();
        var session = new Session
        {
            Id = sessionId,
            Name = name
        };

        if (_sessionManager.TryAdd(sessionId, session))
        {
            return sessionId;
        }

        return default;
    }

    public bool GetSession(string sessionId, out Session session)
    {
        return _sessionManager.TryGetValue(sessionId, out session);
    }

    public bool RemoveSession(string sessionId)
    {
        return _sessionManager.TryRemove(sessionId, out _);
    }

    private static string GenerateUniqueSessionId()
    {
        // Generate a unique session ID
        // You can use Guid.NewGuid() or other methods to ensure uniqueness
        return Guid.NewGuid().ToString()[..12].Replace("-", "");
    }
}

