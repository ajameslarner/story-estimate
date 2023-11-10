using System.Collections.Concurrent;
using StoryEstimate.Models;
using StoryEstimate.Services.Abstract;

namespace StoryEstimate.Context;

public class ChatManager : IQueueContext<Message>
{
    private readonly ConcurrentQueue<Message> _messages = new();

    public bool TryEnqueue(Message item)
    {
        _messages.Enqueue(item);

        return true;
    }

    public bool TryDequeue(out Message result)
    {
        return _messages.TryDequeue(out result);
    }

    public bool TryPeek(out Message result)
    {
        return _messages.TryPeek(out result);
    }

    public IEnumerable<Message> GetAll()
    {
        return _messages.ToArray();
    }
}