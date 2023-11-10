using StoryEstimate.Context;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;

namespace StoryEstimate.Services;

public class SessionService : ISessionService
{
    private readonly SessionManager _sessionManager;

    public SessionService(SessionManager sessionManager)
    {
        this._sessionManager = sessionManager;
    }

    public bool CreateSession(string name)
    {
        string sessionId = GenerateUniqueSessionId();
        var session = new Session { Id = sessionId, Name = name };

        return _sessionManager.TryAdd(sessionId, session);
    }

    public bool GetSession(string sessionId, out Session session)
    {
        return _sessionManager.TryGetValue(sessionId, out session);
    }

    private static string GenerateUniqueSessionId()
    {
        // Generate a unique session ID
        // You can use Guid.NewGuid() or other methods to ensure uniqueness
        return Guid.NewGuid().ToString();
    }
}

