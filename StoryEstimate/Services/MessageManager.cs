using System.Collections.Concurrent;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;

namespace StoryEstimate.Services;

public class ChatManager : IDataContext<Message>
{
    private readonly ConcurrentQueue<Message> _messages = new();
    
    public bool Add(Message message)
    {
        _messages.Enqueue(message);

        return true;
    }

    public Message Get(string Id)
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            return default;
        }

        return _connections.GetValueOrDefault(Id);
    }
}