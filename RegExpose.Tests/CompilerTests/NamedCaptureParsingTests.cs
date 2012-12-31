using System.Linq;
using NUnit.Framework;
using RegExpose.Nodes.Character;
using RegExpose.Nodes.Parens;

namespace RegExpose.Tests.CompilerTests
{
    public class NamedCaptureParsingTests
    {
        [TestCase(@"(?<foo>a)")]
        [TestCase(@"(?'foo'a)")]
        public void ANamedCaptureWithOneCharacterLiteralChildParsesCorrectly(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<NamedCapture>());
            var namedCapture = (NamedCapture)nodes[0];
            Assert.That(namedCapture.Number, Is.EqualTo(1));
            Assert.That(namedCapture.Name, Is.EqualTo("foo"));
            var children = namedCapture.Children.ToList();
            Assert.That(children.Count, Is.EqualTo(1));
            Assert.That(children[0], Is.InstanceOf<CharacterLiteral>());
        }

        [TestCase(@"(?<foo>a[0-9]\d.)")]
        [TestCase(@"(?'foo'a[0-9]\d.)")]
        public void ANamedCaptureWithMultipleCharacterNodesParsesCorrectly(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<NamedCapture>());
            var namedCapture = (NamedCapture)nodes[0];
            Assert.That(namedCapture.Number, Is.EqualTo(1));
            Assert.That(namedCapture.Name, Is.EqualTo("foo"));
            var children = namedCapture.Children.ToList();
            Assert.That(children.Count, Is.EqualTo(4));
            Assert.That(children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children[1], Is.InstanceOf<CharacterClass>());
            Assert.That(children[2], Is.InstanceOf<CharacterClassShorthand>());
            Assert.That(children[3], Is.InstanceOf<Dot>());
        }

        [TestCase(@"(?<foo>abc)(?<bar>123)")]
        [TestCase(@"(?'foo'abc)(?'bar'123)")]
        public void MultipleNamedCaptureParseCorrectly(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(2));
            Assert.That(nodes[0], Is.InstanceOf<NamedCapture>());
            Assert.That(nodes[1], Is.InstanceOf<NamedCapture>());
            var namedCapture1 = (NamedCapture)nodes[0];
            Assert.That(namedCapture1.Number, Is.EqualTo(1));
            Assert.That(namedCapture1.Name, Is.EqualTo("foo"));
            var children1 = namedCapture1.Children.ToList();
            Assert.That(children1.Count, Is.EqualTo(3));
            Assert.That(children1[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[2], Is.InstanceOf<CharacterLiteral>());
            var namedCapture2 = (NamedCapture)nodes[1];
            Assert.That(namedCapture2.Number, Is.EqualTo(2));
            Assert.That(namedCapture2.Name, Is.EqualTo("bar"));
            var children2 = namedCapture2.Children.ToList();
            Assert.That(children2.Count, Is.EqualTo(3));
            Assert.That(children2[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children2[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children2[2], Is.InstanceOf<CharacterLiteral>());
        }

        [TestCase(@"(?<foo>abc(?<bar>123))")]
        [TestCase(@"(?'foo'abc(?'bar'123))")]
        public void NestedNamedCaptureParseCorrectly(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<NamedCapture>());
            var namedCapture1 = (NamedCapture)nodes[0];
            Assert.That(namedCapture1.Number, Is.EqualTo(1));
            Assert.That(namedCapture1.Name, Is.EqualTo("foo"));
            var children1 = namedCapture1.Children;
            Assert.That(children1.Count, Is.EqualTo(4));
            Assert.That(children1[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[2], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[3], Is.InstanceOf<NamedCapture>());
            var namedCapture2 = (NamedCapture)children1[3];
            Assert.That(namedCapture2.Number, Is.EqualTo(2));
            Assert.That(namedCapture2.Name, Is.EqualTo("bar"));
            var children2 = namedCapture2.Children;
            Assert.That(children2.Count, Is.EqualTo(3));
            Assert.That(children2[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children2[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children2[2], Is.InstanceOf<CharacterLiteral>());
        }

        [TestCase(@"(?<foo>abc(?<bar>123)")]
        [TestCase(@"abc(?<bar>123))")]
        [TestCase(@"(?<foo>abc(?<bar>123)*")]
        [TestCase(@"abc(?<bar>123)*)")]
        [TestCase(@"abc(?<bar>123))*")]
        [TestCase(@"(?'foo'abc(?'bar'123)")]
        [TestCase(@"abc(?'bar'123))")]
        [TestCase(@"(?'foo'abc(?'bar'123)*")]
        [TestCase(@"abc(?'bar'123)*)")]
        [TestCase(@"abc(?'bar'123))*")]
        public void UnmatchedParensThrowException(string pattern)
        {
            var sut = new RegexCompiler();

            Assert.That(() => sut.Compile(pattern), Throws.Exception);
        }
    }
}