using System.Collections.Generic;
using System.Linq;
using RegExpose.Nodes.Parens;

namespace RegExpose
{
    public class Match : Group
    {
        private readonly IEnumerator<ParseStep> _stepEnumerator;

        internal Match(ParseStep parseStep, IEnumerator<ParseStep> stepEnumerator, IEnumerable<IList<ParenCapture>> captureSet)
            : base(0, parseStep.InitialStateIndex, parseStep.MatchedText.Length, parseStep.MatchedText, true)
        {
            _stepEnumerator = stepEnumerator;
            Groups = new GroupCollection();
            Groups.Append(this);
            Captures.Prepend(this);

            foreach (var parenCaptures in captureSet)
            {
                var parenCapture = parenCaptures[0];

                var group = new Group(parenCapture.Number,
                                      parenCapture.Index,
                                      parenCapture.Value.Length,
                                      parenCapture.Value,
                                      parenCapture.Success);
                Groups.Append(group);

                if (parenCapture.Success)
                {
                    group.Captures.Prepend(group);
                }

                if (parenCaptures.Skip(1).All(c => c.Success))
                {
                    for (int i = 1; i < parenCaptures.Count; i++)
                    {
                        group.Captures.Prepend(new Capture(parenCaptures[i].Index,
                                                           parenCaptures[i].Value.Length,
                                                           parenCaptures[i].Value));
                    }
                }
            }
        }
                
        public GroupCollection Groups { get; private set; }

        public Match NextMatch()
        {
            if (_stepEnumerator == null)
            {
                return null;
            }

            while (_stepEnumerator.MoveNext())
            {
                if (_stepEnumerator.Current.Type == ParseStepType.Match)
                {
                    return new Match(_stepEnumerator.Current, _stepEnumerator, _stepEnumerator.Current.Captures);
                }
            }

            return null;
        }
    }
}