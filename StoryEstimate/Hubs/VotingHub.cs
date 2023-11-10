// using Microsoft.AspNetCore.SignalR;
// using StoryEstimate.Models;

// namespace StoryEstimate;

// public class VotingHub : Hub
// {
//     private readonly IConnectionManager<Client> _clientManager;

//     public VotingHub(IConnectionManager<Client> clientManager)
//     {
//         _clientManager = clientManager;
//     }

    

//     private async Task UpdateClient()
//     {
//         foreach (var item in _clientService.Sessions)
//         {
//             await Clients.Caller.SendAsync("ReceiveClient", item.Value.Name);
//         }

//         foreach (var item in _clientService.Sessions)
//         {
//             if (item.Value.Voted)
//             {
//                 await Clients.Caller.SendAsync("ReceiveVote", $"{item.Value.Name} has voted!");
//             }
//         }
//     }

//     public async Task Send(string message)
//     {
//         if (_clientService.Sessions.TryGetValue(Context.ConnectionId, out Session session))
//         {
//             await Clients.All.SendAsync("ReceiveMessage", $"{session.Name}: {message}");
//         }   
//     }

//     public async Task Join(string name)
//     {
//         var session = new Session { Id = Context.ConnectionId, Name = name };
//         _clientService.AddClientSession(session);

//         await Clients.Caller.SendAsync("SessionInit", session);

//         session.Name = name;
//         await Clients.All.SendAsync("ReceiveMessage", $"{name} has joined.");
//         await Clients.All.SendAsync("ReceiveClient", name);
//     }

//     public async Task Reset()
//     {
//         foreach (var item in _clientService.Sessions)
//         {
//             var session = item.Value;

//             if (session.Voted)
//             {
//                 session.Voted = false;
//                 session.Vote = null;
//             }

//             _clientService.Sessions[item.Key] = session;
//         }

//         await Clients.All.SendAsync("Reset");
//     }

//     public async Task Vote(string vote)
//     {
//         if (_clientService.Sessions.TryGetValue(Context.ConnectionId, out Session session))
//         {
//             session.Vote = vote;
//             session.Voted = true;
//             _clientService.Sessions[Context.ConnectionId] = session;
//             await Clients.All.SendAsync("ReceiveVote", $"{session.Name} has voted!");
//         }

//         foreach (var item in _clientService.Sessions)
//         {
//             if (!item.Value.Voted)
//             {
//                 return;
//             }
//         }

//         await Clients.All.SendAsync("AllVotesReceived", $"All Votes in!");
//     }

//     public async Task GetVotes()
//     {
//         await Clients.All.SendAsync("ClearVotes");

//         foreach (var item in _clientService.Sessions)
//         {
//             if (item.Value.Voted)
//             {
//                 await Clients.All.SendAsync("ReceiveVote", $"{item.Value.Name} voted: {item.Value.Vote}");
//             }
//         }
//     }

//     public async Task Leave()
//     {
//         if (_clientService.Sessions.TryGetValue(Context.ConnectionId, out Session session))
//         {
//             _clientService.Sessions.TryRemove(Context.ConnectionId, out _);
//             await Clients.All.SendAsync("ReceiveMessage", $"{session.Name} has left.");
//             await Clients.All.SendAsync("LeaveClient", session.Name);
//         }
//     }
// }
