using System;
using System.Diagnostics;
using RegExpose.Nodes.Character;

namespace RegExpose.Nodes.Boundries
{
    public class WordBoundry : ZeroWidthAssertion
    {
        // Hijack the 'word' Character Class Shorthand...
        private static readonly CharacterClass _wordCharacters = new CharacterClassShorthand(@"\w", 0, @"\w");

        public WordBoundry(bool negated, int index, string pattern)
            : base(index, pattern)
        {
            Negated = negated;
        }

        public bool Negated { get; private set; }

        public override string NodeType
        {
            get
            {
                if (Negated)
                {
                    throw new NotImplementedException();
                }

                return "Word Boundry";
            }
        }

        protected override bool Matches(State state)
        {
            if (Negated)
            {
                throw new NotImplementedException();
            }

            if (IsAtBeginningOfInput(state)
                || IsAtBeginningOfLine(state)
                || IsAtEndOfInput(state)
                || IsAtEndOfLine(state))
            {
                return true;
            }

            // Make sure we have at least two charaters in the input, and we're not at the beginning or the end of the input.
            Debug.Assert(state.Input.Length > 1 && state.Index > 0 && state.Index < state.Input.Length);

            var isMatch = _wordCharacters.Matches(state.Input[state.Index])
                          != _wordCharacters.Matches(state.Input[state.Index - 1]);
            return isMatch;
        }

        internal override string GetPassMessage(string match, State initialState)
        {
            if (Negated)
            {
                throw new NotImplementedException();
            }

            return base.GetPassMessage(match, initialState);
        }

        internal override string GetFailMessage(State initialState)
        {
            if (Negated)
            {
                throw new NotImplementedException();
            }

            return base.GetFailMessage(initialState);
        }
    }
}