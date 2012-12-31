using System;
using System.Collections.Generic;

namespace RegExpose.Nodes.Parens
{
    public class NamedCapture : CapturingParens
    {
        public NamedCapture(string name, IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
            Name = name;
        }

        public string Name { get; private set; }

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