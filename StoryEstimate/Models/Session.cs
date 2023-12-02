using StoryEstimate.Events;
using System.Collections.Concurrent;
using Timer = System.Timers.Timer;

namespace StoryEstimate.Models;

public class Session
{
    private Timer? _timeout;
    private int _timeoutValue = 30;
    private string? _name;
    
    public event EventHandler<SessionTimeoutEventArgs>? OnTimeout;
    public string? Id { get; set; }
    public string? Name 
    {
        get
        { 
            if (_timeout != null && _timeout.Enabled)
            {
                return _name + $" ({_timeoutValue})";
            }
            
            return _name;
        }
        set => _name = value;
    }
    public bool Private { get; set; }
    public ConcurrentDictionary<string, Client> Clients { get; set; } = new();
    public ConcurrentDictionary<string, string> Votes { get; set; } = new();
    public ConcurrentQueue<string> Chat { get; set; } = new(); // TODO - Improve by adding client for message sent by
    public bool AllVotesReceived => Votes.Count.Equals(Clients.Count);
    public event Action? OnUpdate;

    public void InitializeTimeout()
    {
        _timeout = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds)
        {
            AutoReset = true
        };

        _timeout.Elapsed += (sender, args) =>
        {
            if (_timeoutValue <= 0)
            {
                OnTimeout!(this, new SessionTimeoutEventArgs { SessionId = Id! });
            }

            _timeoutValue--;

            OnUpdate?.Invoke();
        };

        _timeout.Start();
    }

    public void StopTimeout()
    {
        _timeout?.Stop();
        OnUpdate?.Invoke();
        _timeoutValue = 30;
    }

    public bool Join(string connectionId, Client client)
    {
        if (!Clients.TryAdd(connectionId, client))
        {
            return false;
        }

        StopTimeout();

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
            InitializeTimeout();
        }

        return true;
    }
}