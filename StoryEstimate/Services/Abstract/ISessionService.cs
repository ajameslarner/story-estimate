using StoryEstimate.Models;

namespace StoryEstimate.Services.Abstract;

public interface ISessionService
{
    public string CreateSession(string name, bool isPrivate);
    public bool GetSession(string sessionId, out Session session);
}
