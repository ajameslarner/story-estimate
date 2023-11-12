namespace StoryEstimate;

internal interface IBagContext<T> where T : struct
{
    void Add(T item);
    bool TryTake(out T result);
    bool TryPeek(out T result);
    IEnumerable<T> GetAll();
}

