using System.Collections.Concurrent;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;

namespace StoryEstimate.Context;

public class SessionContext : IDictionaryContext<string, Session>
{
    private readonly ConcurrentDictionary<string, Session> _sessions = new();

    public bool TryAdd(string key, Session value)
    {
        return _sessions.TryAdd(key, value);
    }

    public bool TryGetValue(string key, out Session value)
    {
        return _sessions.TryGetValue(key, out value);
    }

    public bool TryRemove(string key, out Session value)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<KeyValuePair<string, Session>> GetAll()
    {
        return _sessions.ToArray();
    }
}
