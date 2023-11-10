using System.Collections.Concurrent;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;

namespace StoryEstimate.Context;

public class ClientManager : IDictionaryContext<string, Client>
{
    private readonly ConcurrentDictionary<string, Client> _clients = new();

    public bool TryAdd(string key, Client value)
    {
        return _clients.TryAdd(key, value);
    }

    public bool TryGetValue(string key, out Client value)
    {
        return _clients.TryGetValue(key, out value);
    }

    public bool TryRemove(string key, out Client value)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<KeyValuePair<string, Client>> GetAll()
    {
        return _clients.ToArray();
    }
}