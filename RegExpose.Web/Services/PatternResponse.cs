using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegExpose.Web.Services
{
    public class PatternResponse : IHasResponseStatus
    {
        public RegexNodeDto Regex { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}