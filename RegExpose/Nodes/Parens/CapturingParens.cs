using System.Collections.Generic;

namespace RegExpose.Nodes.Parens
{
    public class CapturingParens : GroupingContainerNode
    {
        public CapturingParens(IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
        }

        public int Number { get; internal set; }

        protected override IEnumerable<ParseStep> GetSuccessParseStep(IRegexEngine engine, State initialState)
        {
            var matchedText = engine.Input.Substring(initialState.Index, engine.State.Index - initialState.Index);
            engine.AddCapture(Number, initialState.Index, matchedText);
            yield return ParseStep.Capture(this, matchedText, Number, initialState, engine.State);
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
            get { return string.Format("Capturing Parentheses({0})", Number); }
        }
    }
}