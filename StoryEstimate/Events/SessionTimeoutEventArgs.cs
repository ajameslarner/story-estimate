namespace StoryEstimate.Events
{
    public class SessionTimeoutEventArgs : EventArgs
    {
        public SessionTimeoutEventArgs() { }

        public string SessionId { get; set; }
    }
}