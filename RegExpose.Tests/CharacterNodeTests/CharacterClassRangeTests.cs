using NUnit.Framework;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CharacterNodeTests
{
    public class CharacterClassRangeTests
    {
        [Test]
        public void CaseSensitiveCharacterClassRangeCharactersFromSimpleCharactersMatchesItsRange()
        {
            const string min = "a";
            const string max = "z";

            var sut = new CharacterClassRange(min, max, false);

            Assert.That(sut.Min, Is.EqualTo(min));
            Assert.That(sut.Max, Is.EqualTo(max));
            Assert.That(sut.Matches('a'), Is.True);
            Assert.That(sut.Matches('z'), Is.True);
            Assert.That(sut.Matches('e'), Is.True);
            Assert.That(sut.Matches('A'), Is.False);
            Assert.That(sut.Matches('Z'), Is.False);
            Assert.That(sut.Matches('E'), Is.False);
            Assert.That(sut.Matches('$'), Is.False);
        }

        [Test]
        public void CaseInensitiveCharacterClassRangeCharactersFromSimpleCharactersMatchesItsRange()
        {
            const string min = "a";
            const string max = "z";

            var sut = new CharacterClassRange(min, max, true);

            Assert.That(sut.Min, Is.EqualTo(min));
            Assert.That(sut.Max, Is.EqualTo(max));
            Assert.That(sut.Matches('a'), Is.True);
            Assert.That(sut.Matches('z'), Is.True);
            Assert.That(sut.Matches('e'), Is.True);
            Assert.That(sut.Matches('A'), Is.True);
            Assert.That(sut.Matches('Z'), Is.True);
            Assert.That(sut.Matches('E'), Is.True);
            Assert.That(sut.Matches('$'), Is.False);
        }

        [Test]
        public void CaseSensitiveCharacterClassRangeCharactersFromEscapedCharactersMatchesItsRange()
        {
            const string min = @"\\";
            const string max = @"\^";

            var sut = new CharacterClassRange(min, max, false);

            Assert.That(sut.Min, Is.EqualTo(min));
            Assert.That(sut.Max, Is.EqualTo(max));
            Assert.That(sut.Matches('\\'), Is.True);
            Assert.That(sut.Matches('^'), Is.True);
            Assert.That(sut.Matches(']'), Is.True);
            Assert.That(sut.Matches('@'), Is.False);
            Assert.That(sut.Matches('a'), Is.False);
        }

        [Test]
        public void CaseInensitiveCharacterClassRangeCharactersFromEscapedCharactersMatchesItsRange()
        {
            const string min = @"\\";
            const string max = @"\^";

            var sut = new CharacterClassRange(min, max, true);

            Assert.That(sut.Min, Is.EqualTo(min));
            Assert.That(sut.Max, Is.EqualTo(max));
            Assert.That(sut.Matches('\\'), Is.True);
            Assert.That(sut.Matches('^'), Is.True);
            Assert.That(sut.Matches(']'), Is.True);
            Assert.That(sut.Matches('@'), Is.False);
            Assert.That(sut.Matches('a'), Is.False);
        }
    }
}