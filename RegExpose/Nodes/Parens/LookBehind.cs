using System;
using System.Collections.Generic;
using System.Linq;
using RegExpose.Nodes.Anchors;

namespace RegExpose.Nodes.Parens
{
    public class LookBehind : ContainerNode
    {
        private readonly EndOfString _nonReportingEndOfString;
        private readonly Regex _regex;

        public LookBehind(bool negative, IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
            _nonReportingEndOfString = new EndOfString(false, 0, "");
            _regex = new Regex(Children.Concat(new RegexNode[] { _nonReportingEndOfString }), 0, pattern);
            Negative = negative;
        }

        public bool Negative { get; private set; }

        public override string NodeType
        {
            get { return string.Format("{0} Look-behind", Negative ? "Negative" : "Positive"); }
        }

        internal override IEnumerable<ParseStep> Parse(IRegexEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}