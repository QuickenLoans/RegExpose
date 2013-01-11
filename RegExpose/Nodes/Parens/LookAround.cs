using System.Collections.Generic;
using System.Linq;

namespace RegExpose.Nodes.Parens
{
    public abstract class LookAround : ContainerNode
    {
        protected Regex _regex;
        protected RegexNode _nonReportingNode;

        internal LookAround(bool negative, IEnumerable<RegexNode> children, int index, string pattern) : base(children, index, pattern)
        {
            Negative = negative;
        }

        public bool Negative { get; private set; }

        internal override IEnumerable<ParseStep> Parse(IRegexEngine outerEngine)
        {
            var engine = new RegexEngine.RegexEngineInternal(GetEngineInput(outerEngine));
            foreach (var capture in outerEngine.GetAllDefinedCaptures().SelectMany(kvp => kvp.Value))
            {
                engine.AddCapture(capture.Number, capture.Index, capture.Value);
            }

            var modifier = GetModifier(outerEngine);

            bool match = false;

            yield return ParseStep.StartLookaround(this, engine.State).WithSkipAdvanceOnFail(ShouldSkipAdvance);

            foreach (var result in _regex.Parse(engine))
            {
                // Don't report the results of the non-reporting start of string element.
                if (ReferenceEquals(result.Node, _nonReportingNode))
                {
                    continue;
                }

                if (ReferenceEquals(result.Node, _regex))
                {
                    if (result.Type == ParseStepType.Match)
                    {
                        match = true;
                        break;
                    }

                    if (result.Type == ParseStepType.Fail)
                    {
                        continue;
                    }

                    if (result.Type == ParseStepType.Break)
                    {
                        break;
                    }
                }

                if (result.Type != ParseStepType.Break && engine.State.Index <= engine.Input.Length)
                {
                    yield return result
                        .ConvertToOuterContext(outerEngine.Input, modifier, this, n => ReferenceEquals(n, _regex), message => message.Replace(_regex.NodeType, NodeType))
                        .AsLookaround()
                        .WithSkipAdvanceOnFail(ShouldSkipAdvance);
                }
            }

            if (match)
            {
                if (!Negative)
                {
                    // TODO: we need to forward any captures from the look-around to the outer engine.
                    yield return ParseStep.Pass(this, "", outerEngine.State, engine.State.Plus(modifier)).WithSkipAdvanceOnFail(ShouldSkipAdvance);
                }
                else
                {
                    yield return ParseStep.Fail(this, outerEngine.State, engine.State.Plus(modifier)).WithSkipAdvanceOnFail(ShouldSkipAdvance);
                }
            }
            else
            {
                if (!Negative)
                {
                    yield return ParseStep.Fail(this, outerEngine.State, engine.State.Plus(modifier)).WithSkipAdvanceOnFail(ShouldSkipAdvance);
                }
                else
                {
                    // TODO: we need to forward any captures from the look-around to the outer engine.
                    yield return ParseStep.Pass(this, "", outerEngine.State, engine.State.Plus(modifier)).WithSkipAdvanceOnFail(ShouldSkipAdvance);
                }
            }

            yield return ParseStep.EndLookaround(this).WithSkipAdvanceOnFail(ShouldSkipAdvance);
            yield return ParseStep.Break(this).WithSkipAdvanceOnFail(ShouldSkipAdvance);
        }

        protected abstract string GetEngineInput(IRegexEngine outerEngine);
        protected abstract int GetModifier(IRegexEngine outerEngine);
        protected abstract bool ShouldSkipAdvance { get; }
    }
}