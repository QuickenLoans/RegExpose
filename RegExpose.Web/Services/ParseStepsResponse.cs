using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegExpose.Web.Services
{
    public class ParseStepsResponse
    {
        public RegexNodeDto Regex { get; set; }
        public List<ParseStepDto> ParseSteps { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}