using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RegExpose.Nodes.Parens;

namespace RegExpose
{
    public class RegexEngine
    {
        private readonly string _input;
        private readonly Func<IEnumerable<ParseStep>> _getParseSteps;

        public RegexEngine(Regex regex, string input)
        {
            _input = input;

            // Every time GetParseSteps() is called, the Regex gets a new IRegexEngine.
            _getParseSteps = () => regex.Parse(new RegexEngineInternal(input));
        }

        public IEnumerable<ParseStep> GetParseSteps(Func<ParseStep, bool> excludePredicate = null)
        {
            if (excludePredicate == null)
            {
                excludePredicate = step => false;
            }

            var enumerator = _getParseSteps().GetEnumerator();

            while (true)
            {
                Exception error = null;

                try
                {
                    if (!enumerator.MoveNext())
                    {
                        yield break;
                    }
                }
                catch (Exception ex)
                {
                    error = ex;
                }

                if (error != null)
                {
                    yield return ParseStep.Error(error);
                    yield break;
                }

                if (excludePredicate(enumerator.Current))
                {
                    continue;
                }

                yield return enumerator.Current;
            }
        }

        public string Input
        {
            get { return _input; }
        }

        public Match GetMatch()
        {
            var stepEnumerator = GetParseSteps().GetEnumerator();

            while (stepEnumerator.MoveNext())
            {
                if (stepEnumerator.Current.Type == ParseStepType.Match)
                {
                    return new Match(stepEnumerator.Current, stepEnumerator, stepEnumerator.Current.Captures);
                }
            }

            return null;
        }

        public IEnumerable<Match> GetMatches()
        {
            var match = GetMatch();

            while (match != null)
            {
                yield return match;
                match = match.NextMatch();
            }
        }

        public string Replace(string replacement)
        {
            return Replace(m => replacement);
        }

        public string Replace(string replacement, int count)
        {
            return Replace(m => replacement, count);
        }

        public string Replace(Func<Match, string> matchEvaluator, int count = int.MaxValue)
        {
            var inputReplacement = new StringBuilder(_input);

            var replacementLengthDifference = 0;
            // We need to account for the possibility that a replacement might have a different length than its match.
            var match = GetMatch();
            for (var i = 0; i < count && match != null; i++, match = match.NextMatch())
            {
                var replacement = matchEvaluator(match);
                inputReplacement.Remove(match.Index + replacementLengthDifference, match.Length);
                inputReplacement.Insert(match.Index + replacementLengthDifference, replacement);

                replacementLengthDifference += replacement.Length - match.Length;
            }

            return inputReplacement.ToString();
        }

        internal class RegexEngineInternal : IRegexEngine
        {
            private readonly string _input;
            private readonly Dictionary<int, Stack<ParenCapture>> _captures = new Dictionary<int, Stack<ParenCapture>>();
            private readonly List<ISavedState> _savedStates = new List<ISavedState>();

            public RegexEngineInternal(string input)
            {
                _input = input;
                State = new State(input, 0);
            }

            public State State { get; set; }

            public string Input
            {
                get { return _input; }
            }

            public void AddCapture(int number, int index, string value)
            {
                if (!_captures.ContainsKey(number))
                {
                    _captures.Add(number, new Stack<ParenCapture>());
                }

                var capture = ParenCapture.Pass(number, index, value);
                _captures[number].Push(capture);

                // In case there are any saved states, we need to associate the capture with all of them. That way, should
                // a saved state be backtracted away, it can clear any captures associated with it.
                foreach (var savedState in _savedStates)
                {
                    savedState.AddCapture(capture);
                }
            }

            public bool PopCapture(int number)
            {
                if (_captures.ContainsKey(number) && _captures[number].Count > 0)
                {
                    var capture = _captures[number].Pop();

                    if (_captures[number].Count == 0)
                    {
                        _captures.Remove(number);
                    }

                    // If we're popping a capture, remove it from each of the saved states.
                    foreach (var savedState in _savedStates)
                    {
                        savedState.RemoveCapture(capture);
                    }

                    return true;
                }

                return false;
            }

            public IEnumerable<ParenCapture> GetCaptures(int number)
            {
                return
                    new ReadOnlyCollection<ParenCapture>(
                        _captures.ContainsKey(number)
                            ? _captures[number].ToList()
                            : (IList<ParenCapture>)new[] { ParenCapture.Fail(number) });
            }

            public IEnumerable<KeyValuePair<int, Stack<ParenCapture>>> GetAllDefinedCaptures()
            {
                return _captures;
            }

            public void AddSavedState(ISavedState state)
            {
                _savedStates.Add(state);
            }

            public override string ToString()
            {
                return State.ToString();
            }
        }
    }
}