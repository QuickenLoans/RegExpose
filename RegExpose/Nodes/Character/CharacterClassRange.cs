using System;

namespace RegExpose.Nodes.Character
{
    public class CharacterClassRange : ICharacterMatcher, ICharacterClassPart
    {
        private readonly ushort _maxForMatching;
        private readonly ushort _minForMatching;

        internal CharacterClassRange(string min, string max, bool ignoreCase)
        {
            // We allow escaped characters, so strip off the backslash.
            var minChar = min[0] == '\\' ? min[1] : min[0];
            var maxChar = max[0] == '\\' ? max[1] : max[0];

            Min = min;
            Max = max;
            IgnoreCase = ignoreCase;

            if (minChar > maxChar)
            {
                throw new InvalidOperationException("Character class range min must not be higher than max.");
            }

            _minForMatching = this.FixCase(minChar);
            _maxForMatching = this.FixCase(maxChar);
        }

        public string Min { get; private set; }
        public string Max { get; private set; }

        #region ICharacterMatcher Members

        public bool IgnoreCase { get; private set; }

        #endregion

        #region ICharacterClassPart Members

        public bool Matches(char input)
        {
            var i = (ushort) this.FixCase(input);
            return i >= _minForMatching && i <= _maxForMatching;
        }

        #endregion
    }
}