namespace StoryEstimate.Services.Abstract;

public interface IDataContext<T> where T : struct
{
    public bool Add(T data);
    public T Get(string Id);
}