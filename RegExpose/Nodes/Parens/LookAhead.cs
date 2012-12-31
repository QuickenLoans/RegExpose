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

            yield return ParseStep.StartLookaround(this, outerEngine.State);

            foreach (var result in _regex.Parse(engine))
            {
                // Don't report the results of the non-reporting start of string element.
                if (ReferenceEquals(result.Node, _nonReportingStartOfString))
                {
                    continue;
                }

                if (ReferenceEquals(result.Node, _regex) && result.Type == ParseStepType.Match)
                {
                    // TODO: we need to forward any captures from the look-ahead to the outer engine.
                    yield return ParseStep.Pass(this, "", outerEngine.State, outerEngine.State);
                    yield return ParseStep.EndLookaround(this);

                    yield return ParseStep.Break(this);
                }

                if (result.Type != ParseStepType.Break && engine.State.Index <= engine.Input.Length)
                {
                    yield return result
                        .ConvertToOuterContext(outerEngine.Input, modifier, this, n => ReferenceEquals(n, _regex))
                        .AsLookaround();

                    if (ReferenceEquals(result.Node, this) && result.Type == ParseStepType.Fail)
                    {
                        yield return ParseStep.EndLookaround(this);
                        yield return ParseStep.Break(this);
                    }
                }
            }

            yield return ParseStep.Fail(this, outerEngine.State, outerEngine.State);
            yield return ParseStep.EndLookaround(this);
            yield return ParseStep.Break(this);
        }
    }
}