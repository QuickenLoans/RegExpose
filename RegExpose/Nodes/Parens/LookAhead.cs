using System.Collections.Generic;
using System.Linq;
using RegExpose.Nodes.Anchors;

namespace RegExpose.Nodes.Parens
{
    public class LookAhead : ContainerNode
    {
        private readonly StartOfString _nonReportingStartOfString;
        private readonly Regex _regex;

        public LookAhead(bool negative, IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
            _nonReportingStartOfString = new StartOfString(0, "");
            _regex = new Regex(new RegexNode[] { _nonReportingStartOfString }.Concat(Children), 0, pattern);
            Negative = negative;
        }

        public bool Negative { get; private set; }

        public override string NodeType
        {
            get { return string.Format("{0} Look-ahead", Negative ? "Negative" : "Positive"); }
        }

        internal override IEnumerable<ParseStep> Parse(IRegexEngine outerEngine)
        {
            var engine = new RegexEngine.RegexEngineInternal(outerEngine.Input.Substring(outerEngine.State.Index));
            var modifier = outerEngine.State.Index;

            yield return ParseStep.StartLookaround(this, outerEngine.State).WithSkipAdvanceOnFail(true);

            foreach (var result in _regex.Parse(engine))
            {
                // Don't report the results of the non-reporting start of string element.
                if (ReferenceEquals(result.Node, _nonReportingStartOfString))
                {
                    continue;
                }

                if (ReferenceEquals(result.Node, _regex) && result.Type == ParseStepType.Match)
                {
                    if (!Negative)
                    {
                        // TODO: we need to forward any captures from the look-ahead to the outer engine.
                        yield return ParseStep.Pass(this, "", outerEngine.State, outerEngine.State.Plus(engine.State.Index)).WithSkipAdvanceOnFail(true);
                    }
                    else
                    {
                        yield return ParseStep.Fail(this, outerEngine.State, outerEngine.State.Plus(engine.State.Index)).WithSkipAdvanceOnFail(true);
                    }

                    yield return ParseStep.EndLookaround(this).WithSkipAdvanceOnFail(true);
                    yield return ParseStep.Break(this).WithSkipAdvanceOnFail(true);
                }

                if (result.Type != ParseStepType.Break && engine.State.Index <= engine.Input.Length)
                {
                    if (!Negative && ReferenceEquals(result.Node, _regex) && result.Type == ParseStepType.Fail)
                    {
                        yield return result.ConvertToOuterContext(outerEngine.Input, modifier, this, n => true, message => message.Replace(_regex.NodeType, NodeType)).WithSkipAdvanceOnFail(true);
                        yield return ParseStep.EndLookaround(this).WithSkipAdvanceOnFail(true);
                        yield return ParseStep.Break(this).WithSkipAdvanceOnFail(true);
                    }
                    else if (Negative && ReferenceEquals(result.Node, _regex) && result.Type == ParseStepType.Fail)
                    {
                        // TODO: we need to forward any captures from the look-ahead to the outer engine.
                        yield return ParseStep.Pass(this, "", outerEngine.State, outerEngine.State.Plus(engine.State.Index)).WithSkipAdvanceOnFail(true);
                        yield return ParseStep.EndLookaround(this).WithSkipAdvanceOnFail(true);
                        yield return ParseStep.Break(this).WithSkipAdvanceOnFail(true);
                    }
                    else
                    {
                        yield return result
                        .ConvertToOuterContext(outerEngine.Input, modifier, this, n => ReferenceEquals(n, _regex), message => message.Replace(_regex.NodeType, NodeType))
                        .AsLookaround()
                        .WithSkipAdvanceOnFail(true);
                    }
                }
            }

            if (!Negative)
            {
                yield return ParseStep.Fail(this, outerEngine.State, outerEngine.State.Plus(engine.State.Index)).WithSkipAdvanceOnFail(true);
            }
            else
            {
                // TODO: we need to forward any captures from the look-ahead to the outer engine.
                yield return ParseStep.Pass(this, "", outerEngine.State, outerEngine.State.Plus(engine.State.Index)).WithSkipAdvanceOnFail(true);
            }

            yield return ParseStep.EndLookaround(this).WithSkipAdvanceOnFail(true);
            yield return ParseStep.Break(this).WithSkipAdvanceOnFail(true);
        }
    }
}