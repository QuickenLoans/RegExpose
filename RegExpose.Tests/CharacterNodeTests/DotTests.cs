using NUnit.Framework;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CharacterNodeTests
{
    public class DotTests
    {
        [Test]
        public void NonSingleLineDotMatchesEverythingButNewLine()
        {
            var dot = new Dot(false, 0, ".");

            Assert.That(dot.Matches('a'), Is.True);
            Assert.That(dot.Matches('~'), Is.True);
            Assert.That(dot.Matches('Y'), Is.True);
            Assert.That(dot.Matches('\n'), Is.False);
        }

        [Test]
        public void SingleLineDotMatchesEverythingButNewLine()
        {
            var dot = new Dot(true, 0, ".");

            Assert.That(dot.Matches('a'), Is.True);
            Assert.That(dot.Matches('~'), Is.True);
            Assert.That(dot.Matches('Y'), Is.True);
            Assert.That(dot.Matches('\n'), Is.True);
        }
    }
}