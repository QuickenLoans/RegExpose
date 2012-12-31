using System;
using System.Collections.Generic;

namespace RegExpose.Nodes.Parens
{
    public class LookBehind : ContainerNode
    {
        public LookBehind(bool negative, IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
            Negative = negative;
        }

        public bool Negative { get; private set; }

        public override string NodeType
        {
            get { throw new NotImplementedException(); }
        }

        internal override IEnumerable<ParseStep> Parse(IRegexEngine engine)
        {
            throw new NotImplementedException();
        }
    }
}