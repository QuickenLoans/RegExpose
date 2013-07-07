using System.Collections.Generic;
using System.Linq;

namespace RegExpose.Nodes.Alternation
{
    public class Alternation : RegexNode
    {
        public Alternation(IEnumerable<AlternationChoice> choices, int index, string pattern)
            : base(index, pattern)
        {
            Choices = new RegexNodeCollection<AlternationChoice>(choices.ToList());
        }

        public RegexNodeCollection<AlternationChoice> Choices { get; private set; }

        public override string NodeType
        {
            get { return "Alternation"; }
        }

        internal override IEnumerable<ParseStep> Parse(IRegexEngine engine)
        {
            yield return ParseStep.BeginParse(this, engine.State);

            var initialState = engine.State;

            var choiceIndex = 0;
            foreach (var choice in Choices)
            {
                var matchedText = "";
                var choicePassed = false;

                foreach (var result in choice.Parse(engine))
                {
                    if (ReferenceEquals(choice, result.Node) && result.Type == ParseStepType.Break)
                    {
                        break;
                    }

                    yield return result;

                    if (ReferenceEquals(choice, result.Node))
                    {
                        if (result.Type == ParseStepType.Pass)
                        {
                            matchedText = result.MatchedText;
                            choicePassed = true;

                            if (choiceIndex < Choices.Count - 1)
                            {
                                // Only save state if we're not the last choice...
                                yield return ParseStep.StateSaved(this, initialState, string.Format("Saving state - index {0}", engine.State.Index));
                            }
                        }
                    }
                }

                if (choicePassed)
                {
                    yield return ParseStep.Pass(this, matchedText, initialState, engine.State);
                    yield return ParseStep.Break(this); // TODO: lazy quantifiers might act in a similar manner as alternation here...

                    yield return ParseStep.Backtrack(this, initialState, engine.State);
                }
                else
                {
                    if (engine.State.Index != initialState.Index)
                    {
                        yield return ParseStep.ResetIndex(this, initialState, engine.State);
                        engine.State = initialState;
                    }
                }

                choiceIndex++;
            }

            yield return ParseStep.Fail(this, initialState, engine.State);
            yield return ParseStep.Break(this);
        }
    }
}