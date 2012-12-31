using System;
using System.Collections.Generic;
using System.Text;

namespace RegExpose.Nodes.Quantifiers
{
    public class PossessiveQuantifier : Quantifier
    {
        public PossessiveQuantifier(int min, int? max, RegexNode child, int index, string pattern)
            : base(min, max, child, index, pattern)
        {
        }

        public override string NodeType
        {
            get { throw new NotImplementedException(); }
        }

        protected override IEnumerable<ParseStep> ParseSpecific(IRegexEngine engine, State initialState, StringBuilder matchedText)
        {
            throw new NotImplementedException();
        }
    }
}