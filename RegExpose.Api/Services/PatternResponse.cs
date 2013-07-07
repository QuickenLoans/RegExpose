using ServiceStack.ServiceInterface.ServiceModel;

namespace RegExpose.Api.Services
{
    public class PatternResponse : IHasResponseStatus
    {
        public RegexNodeDto Regex { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}