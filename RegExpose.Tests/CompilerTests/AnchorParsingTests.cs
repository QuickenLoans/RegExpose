using NUnit.Framework;
using RegExpose.Nodes.Anchors;

namespace RegExpose.Tests.CompilerTests
{
    public class AnchorParsingTests
    {
        [Test]
        public void CaretParsesAsDollarWithMultilineSetToFalse()
        {
            const string pattern = "^";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<Caret>());
            var caret = (Caret) result.Children[0];
            Assert.That(caret.Index, Is.EqualTo(0));
            Assert.That(caret.Pattern, Is.EqualTo(pattern));
            Assert.That(caret.MultiLine, Is.EqualTo(false));
        }

        [Test]
        public void CaretParsesAsDollarWithMultilineSetToTrue()
        {
            const string pattern = "^";
            var sut = new RegexCompiler(multiLine:true);

            var result = sut.Compile(pattern);

            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<Caret>());
            var caret = (Caret) result.Children[0];
            Assert.That(caret.Index, Is.EqualTo(0));
            Assert.That(caret.Pattern, Is.EqualTo(pattern));
            Assert.That(caret.MultiLine, Is.EqualTo(true));
        }

        [Test]
        public void DollarParsesAsDollarWithMultilineSetToFalse()
        {
            const string pattern = "$";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<Dollar>());
            var dollar = (Dollar) result.Children[0];
            Assert.That(dollar.Index, Is.EqualTo(0));
            Assert.That(dollar.Pattern, Is.EqualTo(pattern));
            Assert.That(dollar.MultiLine, Is.EqualTo(false));
        }

        [Test]
        public void DollarParsesAsDollarWithMultilineSetToTrue()
        {
            const string pattern = "$";
            var sut = new RegexCompiler(multiLine:true);

            var result = sut.Compile(pattern);

            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<Dollar>());
            var dollar = (Dollar) result.Children[0];
            Assert.That(dollar.Index, Is.EqualTo(0));
            Assert.That(dollar.Pattern, Is.EqualTo(pattern));
            Assert.That(dollar.MultiLine, Is.EqualTo(true));
        }

        [Test]
        public void StartOfStringAnchorParsesToStartOfString()
        {
            const string pattern = @"\A";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<StartOfString>());
            var startOfString = (StartOfString) result.Children[0];
            Assert.That(startOfString.Index, Is.EqualTo(0));
            Assert.That(startOfString.Pattern, Is.EqualTo(pattern));
        }

        [TestCase(@"\z", false)]
        [TestCase(@"\Z", true)]
        public void EndOfStringAnchorParsesToEndOfString(string pattern, bool expectedMatchedFinalNewLine)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<EndOfString>());
            var endOfString = (EndOfString) result.Children[0];
            Assert.That(endOfString.Index, Is.EqualTo(0));
            Assert.That(endOfString.Pattern, Is.EqualTo(pattern));
            Assert.That(endOfString.MatchesFinalNewLine, Is.EqualTo(expectedMatchedFinalNewLine));
        }
    }
}