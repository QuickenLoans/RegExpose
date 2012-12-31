using System.Collections.Generic;
using System.Text;

namespace RegExpose.Nodes.Quantifiers
{
    public class LazyQuantifier : Quantifier
    {
        public LazyQuantifier(int min, int? max, RegexNode child, int index, string pattern)
            : base(min, max, child, index, pattern)
        {
        }

        public override string NodeType
        {
            get { return "Lazy Quantifier"; }
        }

        protected override IEnumerable<ParseStep> ParseSpecific(IRegexEngine engine, State initialState, StringBuilder matchedText)
        {
            for (int i = Min; Max == null || i <= Max; i++)
            {
                // We're lazy - we've already matched what was required of us, so declare that we're done.
                yield return ParseStep.StateSaved(this, initialState, string.Format("Saving state - index {0}", engine.State.Index));
                yield return ParseStep.Pass(this, matchedText.ToString(), initialState, engine.State);
                yield return ParseStep.Break(this);

                // However, if we make it to here, it indicates that we need to match more, in order to (attempt to) get the overall regex to match.
                // According to the parlance of regex people smarter than me, this is a backtrack, even though it's forward.
                yield return ParseStep.Backtrack(this, initialState, engine.State);

                var childSuccess = false;

                foreach (var result in Child.Parse(engine))
                {
                    if (result.Type == ParseStepType.Break && ReferenceEquals(Child, result.Node))
                    {
                        break;
                    }

                    yield return result;

                    if (ReferenceEquals(Child, result.Node))
                    {
                        if (result.Type == ParseStepType.Pass)
                        {
                            matchedText.Append(result.MatchedText);
                            childSuccess = true;
                        }
                    }
                }

                if (!childSuccess)
                {
                    break;
                }
            }

            // If we ever make it outside the loop, it means either we were asked to backtrack and our child didn't pass,
            // or, we were asked to backtrack more than the Max allowed matches
            yield return ParseStep.Fail(this, initialState, engine.State, "Exceeded max allowled quantities");
            yield return ParseStep.Break(this);
        }
    }
};