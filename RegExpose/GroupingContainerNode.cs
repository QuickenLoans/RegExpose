using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RegExpose.Nodes;
using RegExpose.Nodes.Parens;

namespace RegExpose
{
    public abstract class GroupingContainerNode : ContainerNode
    {
        protected GroupingContainerNode(IEnumerable<RegexNode> children, int index, string pattern)
            : base(children, index, pattern)
        {
        }

        internal override IEnumerable<ParseStep> Parse(IRegexEngine engine)
        {
            yield return ParseStep.BeginParse(this, engine.State);

            var savedStates = new Stack<SavedState>();
            SavedState savedState = null;

            // Attempt the regex at every location in the input, starting at the beginning and going until just past the last position.
            while (engine.State.Index <= engine.Input.Length)
            {
                var initialState = engine.State;
                var matchSuccess = true;
                State failedState = null;
                var skipAdvanceOnFail = false;

                // For this location in the input, try every child node.
                for (var item = savedState == null ? Children.First : savedState.Item; item != null;)
                {
                    var node = item.Value;
                    var nodeResultEnumerator = savedState == null ? node.Parse(engine).GetEnumerator() : savedState.Enumerator;
                    savedState = null;

                    var nodeSuccess = false;

                    // Get all the reported results from this child node.
                    while (nodeResultEnumerator.MoveNext())
                    {
                        var result = nodeResultEnumerator.Current;
                        
                        if (result.Type == ParseStepType.StateSaved)
                        {
                            // If decendent said it's saved its state - we need to do the same.
                            var state = new SavedState(nodeResultEnumerator, item, result.CurrentState);
                            engine.AddSavedState(state);
                            savedStates.Push(state);
                        }

                        if (ReferenceEquals(node, result.Node)) // We only pay attention to our children's results
                        {
                            if (result.Type == ParseStepType.Pass)
                            {
                                // If our child told us it was successful, note it.
                                nodeSuccess = true;
                            }
                            else if (result.Type == ParseStepType.Break)
                            {
                                // If our child told us to break, do so. (this indicates that the child was done, regardless of success or failure
                                break;
                            }
                            else if (result.Type == ParseStepType.Fail)
                            {
                                failedState = result.CurrentState;
                                skipAdvanceOnFail = result.SkipAdvanceOnFail;
                            }
                        }

                        // Pass our children's results up.
                        yield return result;
                    }

                    if (!nodeSuccess)
                    {
                        if (savedStates.Count > 0)
                        {
                            // If our child told us that it backtracked, begin backtracking.
                            savedState = savedStates.Pop();
                            foreach (var capture in savedState.PopCaptures(engine))
                            {
                                yield return ParseStep.CaptureDiscarded(savedState.Item.Value, capture.Value, capture.Number);
                            }
                            item = savedState.Item;
                        }
                        else
                        {
                            // If we're here, we either ran out of saved states, or didn't have any to begin with. Our current
                            // node had no way of passing, so we'll fail at this index location in the input string.
                            matchSuccess = false;
                            break;
                        }
                    }
                    else
                    {
                        item = item.Next;
                    }
                }

                if (matchSuccess)
                {
                    // Yay! We've matched the whole group!
                    // However, we want to know if we're yielding an actual 'Match' - if we are, we need to clear our saved states.
                    bool isMatch = false;
                    foreach (var successStep in GetSuccessParseStep(engine, initialState))
                    {
                        if (successStep.Type == ParseStepType.Match)
                        {
                            isMatch = true;
                        }

                        yield return successStep;
                    }

                    if (isMatch)
                    {
                        savedStates.Clear();
                    }

                    // ...but if we're here, our enumerator has been started up again, indicating that we're backtracking. Oblige.
                    if (savedStates.Count > 0)
                    {
                        // If our child told us that it backtracked, begin backtracking.
                        savedState = savedStates.Pop();
                        foreach (var capture in savedState.PopCaptures(engine))
                        {
                            yield return ParseStep.CaptureDiscarded(savedState.Item.Value, capture.Value, capture.Number);
                        }
                        engine.State = savedState.CurrentState;
                    }
                }
                else
                {
                    // We failed at this index location. Advance the engine and start all over again.
                    Debug.Assert(failedState != null);
                    foreach (var failStep in GetFailParseSteps(engine, initialState, failedState, skipAdvanceOnFail))
                    {
                        yield return failStep;
                    }
                }
            }

            // We always get here, regardless of success or failure.
            foreach (var endOfStringStep in GetEndOfStringSteps(engine))
            {
                yield return endOfStringStep;
            }
        }

        protected abstract IEnumerable<ParseStep> GetSuccessParseStep(IRegexEngine engine, State initialState);
        protected abstract IEnumerable<ParseStep> GetFailParseSteps(IRegexEngine engine, State initialState, State currentState, bool skipAdvance);
        protected abstract IEnumerable<ParseStep> GetEndOfStringSteps(IRegexEngine engine); 

        private class SavedState : ISavedState
        {
            private readonly IEnumerator<ParseStep> _enumerator;
            private readonly LinkedListNode<RegexNode> _item;
            private readonly State _state;
            private readonly List<ParenCapture> _captures = new List<ParenCapture>();

            public SavedState(IEnumerator<ParseStep> enumerator, LinkedListNode<RegexNode> item, State currentState)
            {
                _item = item;
                _state = currentState;
                _enumerator = enumerator;
            }

            public IEnumerator<ParseStep> Enumerator
            {
                get { return _enumerator; }
            }

            public LinkedListNode<RegexNode> Item
            {
                get { return _item; }
            }

            public State CurrentState
            {
                get { return _state; }
            }

            public void AddCapture(ParenCapture capture)
            {
                _captures.Add(capture);
            }

            public void RemoveCapture(ParenCapture capture)
            {
                _captures.Remove(capture);
            }

            public IEnumerable<ParenCapture> PopCaptures(IRegexEngine engine)
            {
                var captures = _captures.ToArray();
                return captures.Where(capture => engine.PopCapture(capture.Number));
            }
        }
    }

    public interface ISavedState
    {
        void AddCapture(ParenCapture capture);
        void RemoveCapture(ParenCapture capture);
    }
}