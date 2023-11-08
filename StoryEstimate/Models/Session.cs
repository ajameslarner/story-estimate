using System.ComponentModel;

namespace StoryEstimate;

public struct Session
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool Voted { get; set; }
    public string? Vote { get; set; }
}
