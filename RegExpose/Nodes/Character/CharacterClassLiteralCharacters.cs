using System.Linq;

namespace RegExpose.Nodes.Character
{
    public class CharacterClassLiteralCharacters : ICharacterMatcher, ICharacterClassPart
    {
        private readonly string _valueForMatching;

        internal CharacterClassLiteralCharacters(string value, bool ignoreCase)
        {
            // Remove any escaping backslashes to make the Maches check easier.
            Value = value;
            IgnoreCase = ignoreCase;

            _valueForMatching = new string(
                Value.Replace(@"\]", @"]").Replace(@"\^", @"^").Replace(@"\-", @"-").Replace(@"\\", @"\")
                    .Select(c => this.FixCase(c)).ToArray());
        }

        public string Value { get; private set; }

        #region ICharacterMatcher Members

        public bool IgnoreCase { get; private set; }

        #endregion

        #region ICharacterClassPart Members

        public bool Matches(char input)
        {
            return _valueForMatching.Any(c => c == this.FixCase(input));
        }

        #endregion
    }
}