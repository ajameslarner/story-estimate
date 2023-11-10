using Microsoft.AspNetCore.SignalR;
using StoryEstimate.Context;
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

    public async Task JoinSession(string sessionId)
    {
        if (_sessionService.GetSession(sessionId, out Session session))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            await Clients.Caller.SendAsync("SessionJoined", session);
        }
        else
        {
            await Clients.Caller.SendAsync("SessionNotFound");
        }
    }
}
