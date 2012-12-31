using System;
using System.Collections.Generic;

namespace RegExpose.Nodes.Parens
{
    public class AtomicGrouping : ContainerNode
    {
        public AtomicGrouping(IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
        }

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