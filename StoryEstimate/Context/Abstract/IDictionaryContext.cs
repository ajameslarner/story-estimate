namespace StoryEstimate.Services.Abstract;

internal interface IDictionaryContext<TKey, TValue> where TKey : notnull
{
    event Action OnChanged;
    bool TryAdd(TKey key, TValue value);
    bool TryRemove(TKey key, out TValue value);
    bool TryGetValue(TKey key, out TValue value);
    IEnumerable<KeyValuePair<TKey, TValue>> GetAll();
}