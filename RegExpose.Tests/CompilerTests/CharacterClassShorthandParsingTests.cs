using NUnit.Framework;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CompilerTests
{
    public class CharacterClassShorthandParsingTests
    {
        [Test]
        public void ParseDigitIntoDigitShorthand()
        {
            const string pattern = @"\d";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult(result, Shorthand.Digit);
        }

        [Test]
        public void ParseNonDigitIntoNonDigitShorthand()
        {
            const string pattern = @"\D";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult(result, Shorthand.NonDigit);
        }

        [Test]
        public void ParseWordCharacterIntoWordCharacterShorthand()
        {
            const string pattern = @"\w";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult(result, Shorthand.WordCharacter);
        }

        [Test]
        public void ParseNonWordCharacterIntoNonWordCharacterShorthand()
        {
            const string pattern = @"\W";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult(result, Shorthand.NonWordCharacter);
        }

        [Test]
        public void ParseWhitespaceIntoWhitespaceShorthand()
        {
            const string pattern = @"\s";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult(result, Shorthand.Whitespace);
        }

        [Test]
        public void ParseNonWhitespaceIntoNonWhitespaceShorthand()
        {
            const string pattern = @"\S";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult(result, Shorthand.NonWhitespace);
        }

        private static void AssertResult(Regex result, Shorthand expectedShorthand)
        {
            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<CharacterClassShorthand>());
            var characterClassShorthand = (CharacterClassShorthand) result.Children[0];
            Assert.That(characterClassShorthand.Shorthand, Is.EqualTo(expectedShorthand));
        }
    }
}