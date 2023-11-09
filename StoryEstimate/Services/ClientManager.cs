using System.Collections.Concurrent;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;

namespace StoryEstimate.Services;

public class ClientManager : IDataContext<Client>
{
    private readonly ConcurrentDictionary<string, Client> _connections = new();

    public bool Add(Client client)
    {
        if (string.IsNullOrWhiteSpace(client.Id))
        {
            return default;
        }
        
        return _connections.TryAdd(client.Id, client);
    }

    public Client Get(string Id)
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            return default;
        }

        return _connections.GetValueOrDefault(Id);
    }
}