using NUnit.Framework;
using RegExpose.Nodes.Boundries;

namespace RegExpose.Tests.CompilerTests
{
    public class BoundryParsingTests
    {
        [Test]
        public void WordBoundryParsesToWordBoundry()
        {
            const string pattern = @"\b";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<WordBoundry>());
            var wordBoundry = (WordBoundry) result.Children[0];
            Assert.That(wordBoundry.Index, Is.EqualTo(0));
            Assert.That(wordBoundry.Pattern, Is.EqualTo(pattern));
        }
    }
}