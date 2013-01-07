using System.Collections.Generic;

namespace RegExpose.Nodes.Parens
{
    public class NonCapturingParens : GroupingContainerNode
    {
        public NonCapturingParens(IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
        }

        protected override IEnumerable<ParseStep> GetSuccessParseStep(IRegexEngine engine, State initialState)
        {
            var matchedText = engine.Input.Substring(initialState.Index, engine.State.Index - initialState.Index);
            yield return ParseStep.Pass(this, matchedText, initialState, engine.State);
            yield return ParseStep.Break(this);
        }

        protected override IEnumerable<ParseStep> GetFailParseSteps(IRegexEngine engine, State initialState, State currentState, bool skipAdvance)
        {
            yield return ParseStep.Fail(this, initialState, currentState);
            yield return ParseStep.Break(this);
        }

        protected override IEnumerable<ParseStep> GetEndOfStringSteps(IRegexEngine engine)
        {
            yield return ParseStep.EndOfString(this, engine.State);
            yield return ParseStep.Break(this);
        }

        public override string NodeType
        {
            get { return "Non-capturing Parentheses"; }
        }
    }
}