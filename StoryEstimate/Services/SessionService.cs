using System.Collections.Concurrent;

namespace StoryEstimate;

// Singleton
public class SessionService
{
    public ConcurrentDictionary<string, Session> Sessions { get; private set;} = new();

    public void AddClientSession(Session session)
    {
        if (session.Id is null) return;

        Sessions.TryAdd(session.Id, session);
    }
}
