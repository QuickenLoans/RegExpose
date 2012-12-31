using System.Collections.Generic;
using System.Text;

namespace RegExpose.Nodes.Quantifiers
{
    public abstract class Quantifier : WrapperNode
    {
        protected Quantifier(int min, int? max, RegexNode child, int index, string pattern)
            : base(child, index, pattern)
        {
            Max = max;
            Min = min;
        }

        public int Min { get; private set; }
        public int? Max { get; private set; }

        internal override sealed IEnumerable<ParseStep> Parse(IRegexEngine engine)
        {
            yield return ParseStep.BeginParse(this, engine.State);

            var initialState = engine.State;

            var matchedText = new StringBuilder();

            foreach (var parseStep in ParseRequired(engine, matchedText, initialState))
            {
                yield return parseStep;
            }

            // If min equals max, we're done.
            if (Min == Max)
            {
                yield return ParseStep.Pass(this, matchedText.ToString(), initialState, engine.State);
                yield return ParseStep.Break(this);
                yield break;
            }

            foreach (var specific in ParseSpecific(engine, initialState, matchedText))
            {
                if (ReferenceEquals(specific.Node, this) && specific.Type == ParseStepType.Break)
                {
                    yield return ParseStep.Break(this);
                }
                else
                {
                    yield return specific;
                }
            }
        }

        /// <summary>
        /// Parse the required quantities - we must match at least the minimum.
        /// </summary>
        private IEnumerable<ParseStep> ParseRequired(IRegexEngine engine, StringBuilder matchedText, State initialState)
        {
            for (int i = 0; i < Min; i++)
            {
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
                        }
                        else if (result.Type == ParseStepType.Fail)
                        {
                            yield return ParseStep.Fail(
                                this,
                                initialState,
                                engine.State,
                                string.Format("Greedy quantifier was required to match at least {0} times, but matched {1} times", Min, i));
                            yield return ParseStep.Break(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Parse the "non-required" quantities in the manner unique to the specific quantifier.
        /// </summary>
        protected abstract IEnumerable<ParseStep> ParseSpecific(IRegexEngine engine, State initialState, StringBuilder matchedText);
    }
}