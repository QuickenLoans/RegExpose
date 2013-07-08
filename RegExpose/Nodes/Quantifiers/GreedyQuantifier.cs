using System.Collections.Generic;
using System.Text;

namespace RegExpose.Nodes.Quantifiers
{
    public class GreedyQuantifier : Quantifier
    {
        public GreedyQuantifier(int min, int? max, RegexNode child, int index, string pattern)
            : base(min, max, child, index, pattern)
        {
        }

        public override string NodeType
        {
            get { return "Greedy Quantifier"; }
        }

        protected override IEnumerable<ParseStep> ParseSpecific(IRegexEngine engine, State initialState, StringBuilder matchedText)
        {
            // At this point, we know we'll match. Attempt to match everything else, until we hit max, a non-match, or the end of the string.

            // We need to save states as we go, in case we're asked to backtrack.
            var savedStates = new Stack<SavedState>();

            int matchedQuantity = Min;
            for (; Max == null || matchedQuantity < Max; matchedQuantity++)
            {
                // If the last state we saved was at the end of the string, there's no point in going any further...
                if (savedStates.Count > 0 && savedStates.Peek().State.Index >= engine.Input.Length)
                {
                    // Should we ever get here???
                    savedStates.Pop();
                    break;
                }

                if (engine.State.Index < engine.Input.Length)
                {
                    // We're not at the end of the string, so save state before attempting a match - since we're greedy, we leave breadcrumbs before doing anything.
                    savedStates.Push(new SavedState(engine.State, matchedText.ToString()));
                    yield return ParseStep.StateSaved(this, engine.State, string.Format("Saving state - index {0}", engine.State.Index));
                }
                else
                {
                    // It looks like we're at the end of the string - which means we're done. Time to report as such.
                    yield return ParseStep.EndOfString(this, engine.State);
                    yield return ParseStep.Pass(this, matchedText.ToString(), initialState, engine.State);
                    yield return ParseStep.Break(this);
                    break;
                }

                var endOfMatch = false;

                // TODO: I think that we're going to need to use an enumerator here so we can initiate backtracking in our child and its descendants.
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
                            endOfMatch = true;
                            foreach (var backtrackStep in Backtrack(engine, initialState, savedStates))
                            {
                                yield return backtrackStep;
                            }
                            break;
                        }
                    }
                }

                if (endOfMatch)
                {
                    break;
                }
            }

            if (matchedQuantity >= Max)
            {
                // We've reached the maximum allowed quantity of repetitions, time to break;
                yield return ParseStep.Pass(this, matchedText.ToString(), initialState, engine.State);
                yield return ParseStep.Break(this);
            }

            // If we get here, it means that we're backtracking
            while (savedStates.Count > 0)
            {
                foreach (var backtrackStep in Backtrack(engine, initialState, savedStates))
                {
                    yield return backtrackStep;
                }
            }

            // If we get here, we ran out of saved states to backtrack to - report failure.
            yield return ParseStep.Fail(this, initialState, engine.State, "No backtrack is available");

            if (engine.State.Index != initialState.Index)
            {
                yield return ParseStep.ResetIndex(this, initialState, engine.State);
                engine.State = initialState;
            }

            yield return ParseStep.Break(this);
        }

        private IEnumerable<ParseStep> Backtrack(IRegexEngine engine, State initialState, Stack<SavedState> savedStates)
        {
            var savedState = savedStates.Pop();
            engine.State = savedState.State;
            yield return ParseStep.Backtrack(this, initialState, engine.State);
            yield return ParseStep.Pass(this, savedState.MatchedText, initialState, engine.State);
            yield return ParseStep.Break(this);
        }

        private class SavedState
        {
            private readonly State _state;
            private readonly string _matchedText;

            public SavedState(State state, string matchedText)
            {
                _state = state;
                _matchedText = matchedText;
            }

            public State State
            {
                get { return _state; }
            }

            public string MatchedText
            {
                get { return _matchedText; }
            }
        }
    }
}