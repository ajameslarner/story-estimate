using System.Collections.Concurrent;

namespace Guesstimate;

// Singleton
public class BallotService
{
    public ConcurrentDictionary<string, string> Votes { get; private set;} = new();

    public void AddClientVote(Session session, string vote)
    {
        if (session.Id is null) return;

        Votes[session.Id] = vote;
    }
}
