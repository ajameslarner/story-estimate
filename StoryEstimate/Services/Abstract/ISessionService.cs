using StoryEstimate.Models;

namespace StoryEstimate.Services.Abstract;

public interface ISessionService
{
    public bool CreateSession(string name);
    public bool GetSession(string sessionId, out Session session);
}
