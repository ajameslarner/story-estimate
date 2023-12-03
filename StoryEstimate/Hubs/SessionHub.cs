using Microsoft.AspNetCore.SignalR;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;
using Session = StoryEstimate.Models.Session;

namespace StoryEstimate;

public class SessionHub : Hub
{
    private readonly ISessionService _sessionService;

    public SessionHub(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    public async Task JoinSession(string name, string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Failed to retrieve session.");
            return;
        }

        if (!session.Join(Context.ConnectionId, new Client { Id = Context.ConnectionId, Name = name }))
        {
            await Clients.Caller.SendAsync("ServerError", "Failed to add client.");
            return;
        }

        // Success
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        session.Chat.Enqueue($"[{DateTimeOffset.Now:HH:mm}] {name} has joined!");
        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
    }

    public async Task GetSession(string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        // Success
        await Clients.Caller.SendAsync("SetSession", session);
    }

    public async Task Vote(string vote, string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        if (!session.Clients.TryGetValue(Context.ConnectionId, out Client client))
        {
            await Clients.Caller.SendAsync("ServerError", "Client not found.");
            return;
        }

        if (!session.Votes.TryAdd(client.Name, vote))
        {
            session.Votes.TryGetValue(client.Name, out var oldVote);

            if (session.Votes.TryUpdate(client.Name, vote, oldVote))
            {
                session.Chat.Enqueue($"[{DateTimeOffset.Now:HH:mm}] {client.Name} has changed their mind!");
            }
            else
            {
                await Clients.Caller.SendAsync("ServerError", "Client not found.");
                return;
            }
        }

        // Success
        await Clients.Caller.SendAsync("VoteReceived");
        session.Chat.Enqueue($"[{DateTimeOffset.Now:HH:mm}] {client.Name} has voted!");
        var updatedClient = client;
        updatedClient.HasVoted = true;

        if (!session.Clients.TryUpdate(Context.ConnectionId, updatedClient, client))
        {
            await Clients.Caller.SendAsync("ServerError", "Client not updated.");
            return;
        }

        if (session.AllVotesReceived)
        {
            await Clients.Group(sessionId).SendAsync("AllVotesReceived");
        }

        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
    }

    public async Task SendMessage(string message, string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Models.Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        if (!session.Clients.TryGetValue(Context.ConnectionId, out Client client))
        {
            await Clients.Caller.SendAsync("ServerError", "Client not found.");
            return;
        }

        // Success
        session.Chat.Enqueue($"[{DateTimeOffset.Now:HH:mm}] {client.Name}: {message}");
        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
    }

    public async Task ResetVotes(string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Models.Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        // Success
        session.Votes.Clear();
        session.Chat.Enqueue($"[{DateTimeOffset.Now:HH:mm}] The votes have been reset.");
        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
        await Clients.Group(sessionId).SendAsync("VotesReset");
    }

    public async Task ResetVote(string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        if (!session.Clients.TryGetValue(Context.ConnectionId, out Client client))
        {
            await Clients.Caller.SendAsync("ServerError", "Client not found.");
            return;
        }

        // Success
        var updatedClient = client;
        updatedClient.HasVoted = false;

        if (!session.Clients.TryUpdate(Context.ConnectionId, updatedClient, client))
        {
            await Clients.Caller.SendAsync("ServerError", "Client not updated.");
            return;
        }
        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
    }

    public async Task RevealVotes(string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        // Success
        await Clients.Group(sessionId).SendAsync("Reveal");
        session.Chat.Enqueue($"[{DateTimeOffset.Now:HH:mm}] The results are in!");
    }
    
    public async Task Leave(string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        if (!session.Clients.TryGetValue(Context.ConnectionId, out Client client))
        {
            await Clients.Caller.SendAsync("ServerError", "Client not found.");
            return;
        }

        // Success
        session.Leave(Context.ConnectionId);
        session.Chat.Enqueue($"[{DateTimeOffset.Now:HH:mm}] {client.Name} has left.");
        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
    }
}