using StoryEstimate.Models;

namespace StoryEstimate.Services.Abstract;

public interface ISessionService
{
    public string CreateSession(string name);
    public bool GetSession(string sessionId, out Session session);
    public bool RemoveSession(string sessionId);
}
