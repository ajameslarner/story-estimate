namespace StoryEstimate.Domain.Models.Misc
{
    public class SessionTimeoutEventArgs : EventArgs
    {
        public SessionTimeoutEventArgs() { }

        public string SessionId { get; set; }
    }
}