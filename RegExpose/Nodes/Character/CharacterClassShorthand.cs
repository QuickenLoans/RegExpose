using System;
using System.Collections.Generic;

namespace RegExpose.Nodes.Character
{
    public class CharacterClassShorthand : CharacterClass
    {
        internal CharacterClassShorthand(string value, int index, string pattern)
            : base(GetParts(value), GetNegated(value), false, index, pattern)
        {
            Value = value;
            Shorthand = GetShorthand(value);
        }

        public string Value { get; private set; }
        public Shorthand Shorthand { get; private set; }
        public override string NodeType { get { return Shorthand.ToString(); } }

        private static IEnumerable<ICharacterClassPart> GetParts(string value)
        {
            switch (GetShorthand(value))
            {
                case Shorthand.Digit:
                case Shorthand.NonDigit:
                    yield return new CharacterClassRange("0", "9", false);
                    break;
                case Shorthand.WordCharacter:
                case Shorthand.NonWordCharacter:
                    yield return new CharacterClassRange("a", "z", false);
                    yield return new CharacterClassRange("A", "Z", false);
                    yield return new CharacterClassRange("0", "9", false);
                    yield return new CharacterClassLiteralCharacters("_", false);
                    break;
                case Shorthand.Whitespace:
                case Shorthand.NonWhitespace:
                    yield return new CharacterClassLiteralCharacters(" \f\n\r\t\v", false);
                    break;
                default:
                    throw new InvalidOperationException("Invalid shorthand value.");
            }
        }

        private static bool GetNegated(string value)
        {
            switch (GetShorthand(value))
            {
                case Shorthand.Digit:
                case Shorthand.WordCharacter:
                case Shorthand.Whitespace:
                    return false;
                case Shorthand.NonDigit:
                case Shorthand.NonWordCharacter:
                case Shorthand.NonWhitespace:
                    return true;
                default:
                    throw new InvalidOperationException("Invalid shorthand value.");
            }
        }

        private static Shorthand GetShorthand(string value)
        {
            switch (value)
            {
                case @"\d":
                    return Shorthand.Digit;
                case @"\D":
                    return Shorthand.NonDigit;
                case @"\w":
                    return Shorthand.WordCharacter;
                case @"\W":
                    return Shorthand.NonWordCharacter;
                case @"\s":
                    return Shorthand.Whitespace;
                case @"\S":
                    return Shorthand.NonWhitespace;
                default:
                    throw new InvalidOperationException("Invalid shorthand value: " + value);
            }
        }
    }
}