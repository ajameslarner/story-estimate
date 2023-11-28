using StoryEstimate.Events;
using StoryEstimate.Models.Configurations;
using System.Collections.Concurrent;
using Timer = System.Timers.Timer;

namespace StoryEstimate.Models;

public class Session
{
    private Timer _timeout;
    private SessionConfiguration _settings;
    
    public event EventHandler<SessionTimeoutEventArgs> OnTimeout;
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool Private { get; set; }
    public ConcurrentDictionary<string, Client> Clients { get; set; } = new();
    public ConcurrentDictionary<string, string> Votes { get; set; } = new();
    public ConcurrentQueue<string> Chat { get; set; } = new(); // TODO - Improve by adding client for message sent by
    public bool AllVotesReceived => Votes.Count.Equals(Clients.Count);

    public void Initialize(SessionConfiguration options)
    {
        _settings = options;
        _timeout = new Timer(TimeSpan.FromMinutes(_settings.TimeoutInMinutes).TotalMilliseconds)
        {
            AutoReset = false
        };

        _timeout.Elapsed += (sender, args) =>
        {
            OnTimeout(this, new SessionTimeoutEventArgs { SessionId = Id });
        };
    }

    public bool Join(string connectionId, Client client)
    {
        if (!Clients.TryAdd(connectionId, client))
        {
            return false;
        }

        _timeout.Stop();

        return true;
    }

    public bool Leave(string connectionId)
    {
        Votes.TryRemove(connectionId, out _);

        if (!Clients.TryRemove(connectionId, out _))
        {
            return false;
        }

        if (!Clients.Any())
        {
            _timeout.Start();
        }

        return true;
    }
}