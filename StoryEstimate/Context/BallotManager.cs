using System.Collections.Concurrent;
using StoryEstimate.Models;

namespace StoryEstimate.Context;

public class BallotManager: IBagContext<Vote>
{
    private readonly ConcurrentBag<Vote> _votes = new();

    public void Add(Vote item)
    {
        _votes.Add(item);
    }

    public bool TryTake(out Vote result)
    {
        return _votes.TryTake(out result);
    }

    public bool TryPeek(out Vote result)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Vote> GetAll()
    {
        return _votes.ToArray();
    }
}
