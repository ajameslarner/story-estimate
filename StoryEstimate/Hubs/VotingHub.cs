using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.SignalR;

namespace StoryEstimate;

public class VotingHub : Hub
{
    private readonly SessionService _sessionService;

    public VotingHub(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public override async Task OnConnectedAsync()
    {
        await UpdateClient();

        await base.OnConnectedAsync();
    }

    private async Task UpdateClient()
    {
        foreach (var item in _sessionService.Sessions)
        {
            await Clients.Caller.SendAsync("ReceiveClient", item.Value.Name);
        }

        foreach (var item in _sessionService.Sessions)
        {
            if (item.Value.Voted)
            {
                await Clients.Caller.SendAsync("ReceiveVote", $"{item.Value.Name} has voted!");
            }
        }
    }

    public async Task Send(string message)
    {
        if (_sessionService.Sessions.TryGetValue(Context.ConnectionId, out Session session))
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{session.Name}: {message}");
        }   
    }

    public async Task Join(string name)
    {
        var session = new Session { Id = Context.ConnectionId, Name = name };
        _sessionService.AddClientSession(session);

        await Clients.Caller.SendAsync("SessionInit", session);

        session.Name = name;
        await Clients.All.SendAsync("ReceiveMessage", $"{name} has joined.");
        await Clients.All.SendAsync("ReceiveClient", name);
    }

    public async Task Reset()
    {
        foreach (var item in _sessionService.Sessions)
        {
            var session = item.Value;

            if (session.Voted)
            {
                session.Voted = false;
                session.Vote = null;
            }

            _sessionService.Sessions[item.Key] = session;
        }

        await Clients.All.SendAsync("ClearVotes");
    }

    public async Task Vote(string vote)
    {
        if (_sessionService.Sessions.TryGetValue(Context.ConnectionId, out Session session))
        {
            session.Vote = vote;
            session.Voted = true;
            _sessionService.Sessions[Context.ConnectionId] = session;
            await Clients.All.SendAsync("ReceiveVote", $"{session.Name} has voted!");
        }
    }

    public async Task GetVotes()
    {
        await Clients.All.SendAsync("ClearVotes");

        foreach (var item in _sessionService.Sessions)
        {
            if (item.Value.Voted)
            {
                await Clients.All.SendAsync("ReceiveVote", $"{item.Value.Name} voted: {item.Value.Vote}");
            }
        }
    }

    public async Task Leave()
    {
        if (_sessionService.Sessions.TryGetValue(Context.ConnectionId, out Session session))
        {
            _sessionService.Sessions.TryRemove(Context.ConnectionId, out _);
            await Clients.All.SendAsync("ReceiveMessage", $"{session.Name} has left.");
            await Clients.All.SendAsync("LeaveClient", session.Name);
        }
    }
}
