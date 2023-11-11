using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;

namespace StoryEstimate;

public class SessionHub : Hub
{
    private readonly ISessionService _sessionService;

    public SessionHub(ISessionService sessionService)
    {
        this._sessionService = sessionService;
    }
    
    public async Task JoinSession(string name, string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        if (!session.Clients.TryAdd(Context.ConnectionId, new Client { Id = Context.ConnectionId, Name = name }))
        {
            await Clients.Caller.SendAsync("ServerError", "Failed to add client.");
            return;
        }

        // Success
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        await Clients.Caller.SendAsync("SessionUpdate", session);
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

        if (!session.Votes.TryAdd(client.Id, vote))
        {
            await Clients.Caller.SendAsync("ServerError", "Client not found.");
            return;
        }

        // Success
        await Clients.Caller.SendAsync("VoteReceived");

        if (session.AllVotesReceived)
        {
            await Clients.Group(sessionId).SendAsync("AllVotesReceived");
        }

        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
    }

    public async Task SendMessage(string message, string sessionId)
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
        DateTimeOffset now = DateTimeOffset.Now;
        string formattedTime = now.ToString("hh:mmtt", CultureInfo.InvariantCulture);
        session.Chat.Enqueue($"[{formattedTime}] {client.Name}: {message}");
        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
    }

    public async Task ResetVotes(string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        // Success
        session.Votes.Clear();
        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
    }

    public async Task RevealVotes(string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out _))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        // Success
        await Clients.Group(sessionId).SendAsync("Reveal");
    }
    
    public async Task Leave(string sessionId)
    {
        if (!_sessionService.GetSession(sessionId, out Session session))
        {
            await Clients.Caller.SendAsync("ServerError", "Session not found.");
            return;
        }

        // Success
        session.Leave(Context.ConnectionId);
        await Clients.Group(sessionId).SendAsync("SessionUpdate", session);
    }
}