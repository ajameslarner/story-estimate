using System.Collections.Concurrent;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;

namespace StoryEstimate.Context;

public class SessionContext : IDictionaryContext<string, Session>
{
    private readonly ConcurrentDictionary<string, Session> _sessions = new();

    public event Action? OnChanged;

    public bool TryAdd(string key, Session value)
    {
        value.OnUpdate += Value_OnUpdate;
        if (!_sessions.TryAdd(key, value))
        {
            return false;
        }

        OnChanged?.Invoke();
        
        return true;
    }

    private void Value_OnUpdate()
    {
        OnChanged?.Invoke();
    }

    public bool TryGetValue(string key, out Session value)
    {
        return _sessions.TryGetValue(key, out value);
    }

    public bool TryRemove(string key, out Session value)
    {
        return _sessions.Remove(key, out value);
    }

    public IEnumerable<KeyValuePair<string, Session>> GetAll()
    {
        return _sessions.ToArray();
    }
}
