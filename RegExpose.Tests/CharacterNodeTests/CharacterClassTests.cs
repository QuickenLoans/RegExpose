using Moq;
using NUnit.Framework;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CharacterNodeTests
{
    public class CharacterClassTests
    {
        [Test]
        public void WhenPartMatchesReturnsTrueCharacterClassMatchesReturnsTrue()
        {
            var mockCharacterClassPart = new Mock<ICharacterClassPart>();
            mockCharacterClassPart.Setup(m => m.Matches(It.IsAny<char>())).Returns(true);

            var sut = new CharacterClass(new[] { mockCharacterClassPart.Object }, false, false, 0, "[a]");

            Assert.That(sut.Matches('a'), Is.True);
        }

        [Test]
        public void WhenPartMatchesReturnsFalseCharacterClassMatchesReturnsFalse()
        {
            var mockCharacterClassPart = new Mock<ICharacterClassPart>();
            mockCharacterClassPart.Setup(m => m.Matches(It.IsAny<char>())).Returns(false);

            var sut = new CharacterClass(new[] { mockCharacterClassPart.Object }, false, false, 0, "[a]");

            Assert.That(sut.Matches('a'), Is.False);
        }

        [Test]
        public void WhenPartMatchesReturnsTrueNegatedCharacterClassMatchesReturnsFalse()
        {
            var mockCharacterClassPart = new Mock<ICharacterClassPart>();
            mockCharacterClassPart.Setup(m => m.Matches(It.IsAny<char>())).Returns(true);

            var sut = new CharacterClass(new[] { mockCharacterClassPart.Object }, true, false, 0, "[^a]");

            Assert.That(sut.Matches('a'), Is.False);
        }

        [Test]
        public void WhenPartMatchesReturnsFalseNegatedCharacterClassMatchesReturnsTrue()
        {
            var mockCharacterClassPart = new Mock<ICharacterClassPart>();
            mockCharacterClassPart.Setup(m => m.Matches(It.IsAny<char>())).Returns(false);

            var sut = new CharacterClass(new[] { mockCharacterClassPart.Object }, true, false, 0, "[^a]");

            Assert.That(sut.Matches('a'), Is.True);
        }
    }
}