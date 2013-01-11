using System.Collections.Generic;
using System.Linq;
using RegExpose.Nodes.Anchors;

namespace RegExpose.Nodes.Parens
{
    public class LookBehind : LookAround
    {
        public LookBehind(bool negative, IEnumerable<RegexNode> children, int index, string pattern)
            : base(negative, children, index, pattern)
        {
            _nonReportingNode = new EndOfString(false, 0, "");
            _regex = new Regex(Children.Concat(new[] { _nonReportingNode }), 0, pattern);
        }

        public override string NodeType
        {
            get { return string.Format("{0} Look-behind", Negative ? "Negative" : "Positive"); }
        }

        protected override string GetEngineInput(IRegexEngine outerEngine)
        {
            return outerEngine.Input.Substring(0, outerEngine.State.Index);
        }

        protected override int GetModifier(IRegexEngine outerEngine)
        {
            return 0;
        }

        protected override bool ShouldSkipAdvance
        {
            get { return false; }
        }
    }
}