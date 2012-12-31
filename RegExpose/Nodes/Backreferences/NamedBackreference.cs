using System;
using System.Collections.Generic;

namespace RegExpose.Nodes.Backreferences
{
    public class NamedBackreference : LeafNode
    {
        public NamedBackreference(string name, int index, string pattern)
            : base(index, pattern)
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