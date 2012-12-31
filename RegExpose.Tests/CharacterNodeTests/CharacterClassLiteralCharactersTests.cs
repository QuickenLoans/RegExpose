using NUnit.Framework;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CharacterNodeTests
{
    public class CharacterClassLiteralCharactersTests
    {
        [Test]
        public void CaseSensitiveCharacterClassLiteralCharactersFromSimpleCharactersMatchesItsValue()
        {
            const string value = "abAB";

            var sut = new CharacterClassLiteralCharacters(value, false);

            Assert.That(sut.Value, Is.EqualTo(value));
            Assert.That(sut.Matches('a'), Is.True);
            Assert.That(sut.Matches('A'), Is.True);
            Assert.That(sut.Matches('b'), Is.True);
            Assert.That(sut.Matches('B'), Is.True);
            Assert.That(sut.Matches('e'), Is.False);
            Assert.That(sut.Matches('$'), Is.False);
        }

        [Test]
        public void CaseInsensitiveCharacterClassLiteralCharactersFromSimpleCharactersMatchesItsValue()
        {
            const string value = "ab";

            var sut = new CharacterClassLiteralCharacters(value, true);

            Assert.That(sut.Value, Is.EqualTo(value));
            Assert.That(sut.Matches('a'), Is.True);
            Assert.That(sut.Matches('A'), Is.True);
            Assert.That(sut.Matches('b'), Is.True);
            Assert.That(sut.Matches('B'), Is.True);
            Assert.That(sut.Matches('e'), Is.False);
            Assert.That(sut.Matches('$'), Is.False);
        }

        [Test]
        public void CaseSensitiveCharacterClassLiteralCharactersFromEscapedCharactersMatchesItsValue()
        {
            const string value = @"\]\^\-\\";

            var sut = new CharacterClassLiteralCharacters(value, false);

            Assert.That(sut.Value, Is.EqualTo(value));
            Assert.That(sut.Matches(']'), Is.True);
            Assert.That(sut.Matches('^'), Is.True);
            Assert.That(sut.Matches('-'), Is.True);
            Assert.That(sut.Matches('\\'), Is.True);
            Assert.That(sut.Matches('e'), Is.False);
            Assert.That(sut.Matches('$'), Is.False);
        }

        [Test]
        public void CaseInsensitiveCharacterClassLiteralCharactersFromEscapedCharactersMatchesItsValue()
        {
            const string value = @"\]\^\-\\";

            var sut = new CharacterClassLiteralCharacters(value, true);

            Assert.That(sut.Value, Is.EqualTo(value));
            Assert.That(sut.Matches(']'), Is.True);
            Assert.That(sut.Matches('^'), Is.True);
            Assert.That(sut.Matches('-'), Is.True);
            Assert.That(sut.Matches('\\'), Is.True);
            Assert.That(sut.Matches('e'), Is.False);
            Assert.That(sut.Matches('$'), Is.False);
        }
    }
}