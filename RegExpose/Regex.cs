using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RegExpose.Nodes;
using RegExpose.Nodes.Parens;

namespace RegExpose
{
    public class Regex : GroupingContainerNode
    {
        internal Regex(IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
        }

        public RegexEngine Parse(string input)
        {
            return new RegexEngine(this, input);
        }

        public RegexNode FindNodeById(int id)
        {
            return id == 0 ? this : Children.FindById(id);
        }

        public override string NodeType
        {
            get { return "Regex"; }
        }

        protected override IEnumerable<ParseStep> GetSuccessParseStep(IRegexEngine engine, State initialState)
        {
            var matchedText = engine.Input.Substring(initialState.Index, engine.State.Index - initialState.Index);

            IList<IList<ParenCapture>> captureSet = new List<IList<ParenCapture>>();

            foreach (CapturingParens capturingParen in Children.FindBy(node => node is CapturingParens))
            {
                var captures = engine.GetCaptures(capturingParen.Number);
                captureSet.Add(new ReadOnlyCollection<ParenCapture>(captures.ToList()));
                engine.PopCapture(capturingParen.Number);
            }

            yield return ParseStep.Match(this, initialState, matchedText, new ReadOnlyCollection<IList<ParenCapture>>(captureSet));

            if (initialState.Index == engine.State.Index)
            {
                // If we had a successful match, and the engine didn't move, we need to move it ourselves now.
                engine.State = engine.State.Advance();
                yield return ParseStep.AdvanceIndex(this, engine.State);
            }

            yield return ParseStep.BeginParse(this, engine.State);
        }

        protected override IEnumerable<ParseStep> GetFailParseSteps(IRegexEngine engine, State initialState, State currentState, bool skipAdvance)
        {
            yield return ParseStep.Fail(this, initialState, currentState);

            if (!skipAdvance)
            {
                engine.State = initialState.Advance();
                yield return ParseStep.AdvanceIndex(this, engine.State);
            }
        }

        protected override IEnumerable<ParseStep> GetEndOfStringSteps(IRegexEngine engine)
        {
            yield return ParseStep.EndOfString(this, engine.State);
        }

        public string Dump()
        {
            var sb = new StringBuilder();

            RegexNode node = this;
            for (var i = 0; node != null; i++)
            {
                node = FindNodeById(i);
                if (node != null)
                {
                    sb.AppendLine(node.ToString());
                }
            }

            return sb.ToString();
        }
    }
}