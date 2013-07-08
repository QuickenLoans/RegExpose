using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace RegExpose.Api.Services
{
    public class ParseStepsResponse
    {
        public RegexNodeDto Regex { get; set; }
        public List<ParseStepDto> ParseSteps { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}