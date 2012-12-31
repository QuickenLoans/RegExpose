using NUnit.Framework;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CharacterNodeTests
{
    public class CharacterLiteralTests
    {
        [Test]
        public void CaseSensitiveCharacterLiteralFromSimpleCharacterMatchesItsCharacter()
        {
            var sut = new CharacterLiteral('a', false, 0, "a");

            Assert.That(sut.Matches('a'), Is.True);
            Assert.That(sut.Matches('A'), Is.False);
            Assert.That(sut.Matches('e'), Is.False);
        }

        [Test]
        public void CaseInensitiveCharacterLiteralFromSimpleCharacterMatchesItsCharacter()
        {
            var sut = new CharacterLiteral('a', true, 0, "a");

            Assert.That(sut.Matches('a'), Is.True);
            Assert.That(sut.Matches('A'), Is.True);
            Assert.That(sut.Matches('e'), Is.False);
        }

        [Test]
        public void CaseSensitiveCharacterLiteralFromEscapedCharacterMatchesItsCharacter()
        {
            var sut = new CharacterLiteral(@"\?", false, 0, @"\?");

            Assert.That(sut.Matches('?'), Is.True);
            Assert.That(sut.Matches('\\'), Is.False);
            Assert.That(sut.Matches('e'), Is.False);
        }

        [Test]
        public void CaseInensitiveCharacterLiteralFromEscapedCharacterMatchesItsCharacter()
        {
            var sut = new CharacterLiteral(@"\?", true, 0, @"\?");

            Assert.That(sut.Matches('?'), Is.True);
            Assert.That(sut.Matches('\\'), Is.False);
            Assert.That(sut.Matches('e'), Is.False);
        }
    }
}