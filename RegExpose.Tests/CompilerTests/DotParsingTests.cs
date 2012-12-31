using NUnit.Framework;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CompilerTests
{
    public class DotParsingTests
    {
        [Test]
        public void DotIsParsedIntoDotWhenSingleLineIsFalse()
        {
            const string pattern = ".";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult(result, false);
        }

        [Test]
        public void DotIsParsedIntoDotWhenSingleLineIsTrue()
        {
            const string pattern = ".";
            var sut = new RegexCompiler(singleLine:true);

            var result = sut.Compile(pattern);

            AssertResult(result, true);
        }

        private static void AssertResult(Regex result, bool expectedSingleLine)
        {
            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<Dot>());
            var dot = (Dot) result.Children[0];
            Assert.That(dot.SingleLine, Is.EqualTo(expectedSingleLine));
        }
    }
}