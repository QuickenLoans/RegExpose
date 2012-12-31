using System.Collections.Generic;
using RegExpose.Nodes.Parens;

namespace RegExpose.Nodes.Alternation
{
    public class AlternationChoice : NonCapturingParens
    {
        public AlternationChoice(IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
        }

        public override string NodeType
        {
            get { return "Alternation Choice"; }
        }
    }
}