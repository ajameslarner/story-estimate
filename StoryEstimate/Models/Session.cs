using System.Collections.Concurrent;

namespace StoryEstimate.Models;

public class Session
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool Private { get; set; }
    public ConcurrentDictionary<string, Client> Clients { get; set; } = new();
    public ConcurrentDictionary<string, string> Votes { get; set; } = new();
    public ConcurrentQueue<string> Chat { get; set; } = new(); // TODO - Improve by adding client for message sent by
    public bool AllVotesReceived => Votes.Count.Equals(Clients.Count);

    public bool Leave(string connectionId)
    {
        Votes.TryRemove(connectionId, out _);

        if (!Clients.TryRemove(connectionId, out _))
        {
            return false;
        }

        return true;
    }
}