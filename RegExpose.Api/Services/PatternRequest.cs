using ServiceStack.ServiceHost;

namespace RegExpose.Api.Services
{
    public class PatternRequest : IReturn<PatternResponse>
    {
        public string Pattern { get; set; }
        public bool IgnoreCase { get; set; }
        public bool SingleLine { get; set; }
        public bool MultiLine { get; set; }
    }
}