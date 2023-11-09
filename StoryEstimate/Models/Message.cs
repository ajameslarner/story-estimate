namespace StoryEstimate.Models;

public struct Message
{
    public int Id { get; set; }
    public string? Body { get; set; }
    public string? AuthorId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}
