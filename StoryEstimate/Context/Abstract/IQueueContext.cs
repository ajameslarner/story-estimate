namespace StoryEstimate.Services.Abstract;

internal interface IQueueContext<T> where T : struct
{
    bool TryEnqueue(T item);
    bool TryDequeue(out T result);
    bool TryPeek(out T result);
    IEnumerable<T> GetAll();
}