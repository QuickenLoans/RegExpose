using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegExpose.Web.Services
{
    public class PatternRequest : IReturn<PatternResponse>
    {
        public string Pattern { get; set; }
        public bool IgnoreCase { get; set; }
        public bool SingleLine { get; set; }
        public bool MultiLine { get; set; }
    }
}