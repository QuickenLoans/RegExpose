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

        public IEnumerable<ParseStep> GetParseSteps()
        {
            var index = 0;
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
                    yield return ParseStep.Error(error).SetStepIndex(index);
                    yield break;
                }

                var step = enumerator.Current;
                yield return step.SetStepIndex(index);
                index++;
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

                _captures[number].Push(ParenCapture.Pass(number, index, value));
            }

            public void PopCapture(int number)
            {
                if (_captures.ContainsKey(number) && _captures[number].Count > 0)
                {
                    _captures[number].Pop();

                    if (_captures[number].Count == 0)
                    {
                        _captures.Remove(number);
                    }
                }
            }

            public IEnumerable<ParenCapture> GetCaptures(int number)
            {
                return
                    new ReadOnlyCollection<ParenCapture>(
                        _captures.ContainsKey(number)
                            ? _captures[number].ToList()
                            : (IList<ParenCapture>)new[] { ParenCapture.Fail(number) });
            }

            public override string ToString()
            {
                return State.ToString();
            }
        }
    }
}