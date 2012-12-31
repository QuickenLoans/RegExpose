using NUnit.Framework;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CharacterNodeTests
{
    public class CharacterClassShorthandTests
    {
        [Test]
        public void DigitMatchesDigits()
        {
            var sut = new CharacterClassShorthand(@"\d", 0, @"\d");

            Assert.That(sut.Matches('0'), Is.True);
            Assert.That(sut.Matches('5'), Is.True);
            Assert.That(sut.Matches('9'), Is.True);
            Assert.That(sut.Matches('d'), Is.False);
            Assert.That(sut.Matches('Y'), Is.False);
            Assert.That(sut.Matches('#'), Is.False);
            Assert.That(sut.Matches(' '), Is.False);
            Assert.That(sut.Matches('\n'), Is.False);
        }

        [Test]
        public void NonDigitMatchesNonDigits()
        {
            var sut = new CharacterClassShorthand(@"\D", 0, @"\D");

            Assert.That(sut.Matches('0'), Is.False);
            Assert.That(sut.Matches('5'), Is.False);
            Assert.That(sut.Matches('9'), Is.False);
            Assert.That(sut.Matches('d'), Is.True);
            Assert.That(sut.Matches('Y'), Is.True);
            Assert.That(sut.Matches('#'), Is.True);
            Assert.That(sut.Matches(' '), Is.True);
            Assert.That(sut.Matches('\n'), Is.True);
        }

        [Test]
        public void PartOfWordMatchesPartsOfWord()
        {
            var sut = new CharacterClassShorthand(@"\w", 0, @"\w");

            Assert.That(sut.Matches('0'), Is.True);
            Assert.That(sut.Matches('5'), Is.True);
            Assert.That(sut.Matches('9'), Is.True);
            Assert.That(sut.Matches('d'), Is.True);
            Assert.That(sut.Matches('w'), Is.True);
            Assert.That(sut.Matches('Y'), Is.True);
            Assert.That(sut.Matches('M'), Is.True);
            Assert.That(sut.Matches('#'), Is.False);
            Assert.That(sut.Matches(' '), Is.False);
            Assert.That(sut.Matches('\n'), Is.False);
        }

        [Test]
        public void NonWordCharacterMatchesNonWordCharacters()
        {
            var sut = new CharacterClassShorthand(@"\W", 0, @"\W");

            Assert.That(sut.Matches('0'), Is.False);
            Assert.That(sut.Matches('5'), Is.False);
            Assert.That(sut.Matches('9'), Is.False);
            Assert.That(sut.Matches('d'), Is.False);
            Assert.That(sut.Matches('w'), Is.False);
            Assert.That(sut.Matches('Y'), Is.False);
            Assert.That(sut.Matches('M'), Is.False);
            Assert.That(sut.Matches('#'), Is.True);
            Assert.That(sut.Matches(' '), Is.True);
            Assert.That(sut.Matches('\n'), Is.True);
        }

        [Test]
        public void WhitspaceMatchesDigitsWhitespace()
        {
            var sut = new CharacterClassShorthand(@"\s", 0, @"\s");

            Assert.That(sut.Matches(' '), Is.True);
            Assert.That(sut.Matches('\f'), Is.True);
            Assert.That(sut.Matches('\n'), Is.True);
            Assert.That(sut.Matches('\r'), Is.True);
            Assert.That(sut.Matches('\t'), Is.True);
            Assert.That(sut.Matches('\v'), Is.True);
            Assert.That(sut.Matches('s'), Is.False);
            Assert.That(sut.Matches('i'), Is.False);
            Assert.That(sut.Matches('E'), Is.False);
            Assert.That(sut.Matches('N'), Is.False);
            Assert.That(sut.Matches('3'), Is.False);
            Assert.That(sut.Matches('9'), Is.False);
            Assert.That(sut.Matches('%'), Is.False);
            Assert.That(sut.Matches(':'), Is.False);
        }

        [Test]
        public void NonWhitspaceMatchesDigitsNonWhitespace()
        {
            var sut = new CharacterClassShorthand(@"\S", 0, @"\S");

            Assert.That(sut.Matches(' '), Is.False);
            Assert.That(sut.Matches('\f'), Is.False);
            Assert.That(sut.Matches('\n'), Is.False);
            Assert.That(sut.Matches('\r'), Is.False);
            Assert.That(sut.Matches('\t'), Is.False);
            Assert.That(sut.Matches('\v'), Is.False);
            Assert.That(sut.Matches('s'), Is.True);
            Assert.That(sut.Matches('i'), Is.True);
            Assert.That(sut.Matches('E'), Is.True);
            Assert.That(sut.Matches('N'), Is.True);
            Assert.That(sut.Matches('3'), Is.True);
            Assert.That(sut.Matches('9'), Is.True);
            Assert.That(sut.Matches('%'), Is.True);
            Assert.That(sut.Matches(':'), Is.True);
        }
    }
}