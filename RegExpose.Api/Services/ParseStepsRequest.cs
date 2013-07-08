using ServiceStack.ServiceHost;

namespace RegExpose.Api.Services
{
    [Api("Gets the steps that the regular expression engine goes through while parsing its input text.")]
    [Route("/regex/steps", "GET", Summary = @"Regex Parse Steps", Notes = "Gets the steps that the regular expression engine goes through while parsing its input text.")]
    public class ParseStepsRequest : IReturn<ParseStepsResponse>
    {
        internal const string MaxStepCountPerRequest = "1000";

        [ApiMember(Name = "Pattern", Description = "The regular expression pattern to match.", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string Pattern { get; set; }

        [ApiMember(Name = "Input", Description = "The string to search for a match.", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string Input { get; set; }

        [ApiMember(Name = "IgnoreCase", Description = "Specifies to use case-insensitive matching.", ParameterType = "query", DataType = "boolean", IsRequired = false)]
        public bool IgnoreCase { get; set; }

        [ApiMember(Name = "SingleLine", Description = @"If true, changes the meaning of the dot (.) so it matches every character (instead of every character except \n).", ParameterType = "query", DataType = "boolean", IsRequired = false)]
        public bool SingleLine { get; set; }

        [ApiMember(Name = "MultiLine", Description = "If true, changes the meaning of ^ and $ so they match at the beginning and end, respectively, of any line, and not just the beginning and end of the entire string.", ParameterType = "query", DataType = "boolean", IsRequired = false)]
        public bool MultiLine { get; set; }

        [ApiMember(Name = "Skip", Description = "Bypasses this many steps and then returns the remaining steps. Useful when the number of steps is large (more than " + MaxStepCountPerRequest + ").", ParameterType = "query", DataType = "int", IsRequired = false)]
        public int? Skip { get; set; }

        [ApiMember(Name = "Take", Description = "Returns this many steps. Useful when the number of steps is large (more than " + MaxStepCountPerRequest + ").", ParameterType = "query", DataType = "int", IsRequired = false)]
        public int? Take { get; set; }
    }
}