namespace StoryEstimate.Services.Abstract;

public interface IVotingService<T> where T : notnull
{
    public bool AddVote(T vote);
    public T[] GetVotes();
}