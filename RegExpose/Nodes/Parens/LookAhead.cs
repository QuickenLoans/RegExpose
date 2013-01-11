using System.Collections.Generic;
using System.Linq;
using RegExpose.Nodes.Anchors;

namespace RegExpose.Nodes.Parens
{
    public class LookAhead : LookAround
    {
        public LookAhead(bool negative, IEnumerable<RegexNode> children, int index, string pattern)
            : base(negative, children, index, pattern)
        {
            _nonReportingNode = new StartOfString(0, "");
            _regex = new Regex(new[] { _nonReportingNode }.Concat(Children), 0, pattern);
        }

        public override string NodeType
        {
            get { return string.Format("{0} Look-ahead", Negative ? "Negative" : "Positive"); }
        }

        protected override string GetEngineInput(IRegexEngine outerEngine)
        {
            return outerEngine.Input.Substring(outerEngine.State.Index);
        }

        protected override int GetModifier(IRegexEngine outerEngine)
        {
            return outerEngine.State.Index;
        }

        protected override bool ShouldSkipAdvance
        {
            get { return true; }
        }
    }
}