using System.Linq;
using NUnit.Framework;
using RegExpose.Nodes.Character;
using RegExpose.Nodes.Parens;
using RegExpose.Nodes.Quantifiers;

namespace RegExpose.Tests.CompilerTests
{
    public class CapturingParensParsingTests
    {
        [Test]
        public void ACapturingParensWithOneCharacterLiteralChildParsesCorrectly()
        {
            const string pattern = @"(a)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<CapturingParens>());
            var capturingParens = (CapturingParens) nodes[0];
            Assert.That(capturingParens.Number, Is.EqualTo(1));
            var children = capturingParens.Children.ToList();
            Assert.That(children.Count, Is.EqualTo(1));
            Assert.That(children[0], Is.InstanceOf<CharacterLiteral>());
        }

        [Test]
        public void ACapturingParensWithMultipleCharacterNodesParsesCorrectly()
        {
            const string pattern = @"(a[0-9]\d.)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<CapturingParens>());
            var capturingParens = (CapturingParens) nodes[0];
            Assert.That(capturingParens.Number, Is.EqualTo(1));
            var children = capturingParens.Children.ToList();
            Assert.That(children.Count, Is.EqualTo(4));
            Assert.That(children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children[1], Is.InstanceOf<CharacterClass>());
            Assert.That(children[2], Is.InstanceOf<CharacterClassShorthand>());
            Assert.That(children[3], Is.InstanceOf<Dot>());
        }

        [Test]
        public void MultipleCapturingParensParseCorrectly()
        {
            const string pattern = @"(abc)(123)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(2));
            Assert.That(nodes[0], Is.InstanceOf<CapturingParens>());
            Assert.That(nodes[1], Is.InstanceOf<CapturingParens>());
            var capturingParens1 = (CapturingParens) nodes[0];
            Assert.That(capturingParens1.Number, Is.EqualTo(1));
            var children1 = capturingParens1.Children.ToList();
            Assert.That(children1.Count, Is.EqualTo(3));
            Assert.That(children1[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[2], Is.InstanceOf<CharacterLiteral>());
            var capturingParens2 = (CapturingParens) nodes[1];
            Assert.That(capturingParens2.Number, Is.EqualTo(2));
            var children2 = capturingParens2.Children.ToList();
            Assert.That(children2.Count, Is.EqualTo(3));
            Assert.That(children2[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children2[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children2[2], Is.InstanceOf<CharacterLiteral>());
        }

        [Test]
        public void NestedCapturingParensParseCorrectly()
        {
            const string pattern = @"(abc(123))";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<CapturingParens>());
            var capturingParens1 = (CapturingParens) nodes[0];
            Assert.That(capturingParens1.Number, Is.EqualTo(1));
            var children1 = capturingParens1.Children;
            Assert.That(children1.Count, Is.EqualTo(4));
            Assert.That(children1[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[2], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[3], Is.InstanceOf<CapturingParens>());
            var capturingParens2 = (CapturingParens) children1[3];
            Assert.That(capturingParens2.Number, Is.EqualTo(2));
            var children2 = capturingParens2.Children;
            Assert.That(children2.Count, Is.EqualTo(3));
            Assert.That(children2[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children2[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children2[2], Is.InstanceOf<CharacterLiteral>());
        }

        [Test]
        public void NestedQuantifierParensParseCorrectly()
        {
            const string pattern = @"(ab(12)?xy)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<CapturingParens>());
            var capturingParens1 = (CapturingParens)nodes[0];
            Assert.That(capturingParens1.Number, Is.EqualTo(1));
            var children1 = capturingParens1.Children;
            Assert.That(children1.Count, Is.EqualTo(5));
            Assert.That(children1[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[2], Is.InstanceOf<GreedyQuestionMark>());
            Assert.That(children1[3], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children1[4], Is.InstanceOf<CharacterLiteral>());
            var greedyQuestionMark = (GreedyQuestionMark)children1[2];
            Assert.That(greedyQuestionMark.Child, Is.InstanceOf<CapturingParens>());
            var children2 = ((CapturingParens)greedyQuestionMark.Child).Children;
            Assert.That(children2.Count, Is.EqualTo(2));
            Assert.That(children2[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children2[1], Is.InstanceOf<CharacterLiteral>());
        }

        [TestCase(@"(abc(123)")]
        [TestCase(@"abc(123))")]
        [TestCase(@"(abc(123)*")]
        [TestCase(@"abc(123)*)")]
        [TestCase(@"abc(123))*")]
        public void UnmatchedParensThrowException(string pattern)
        {
            var sut = new RegexCompiler();

            Assert.That(() => sut.Compile(pattern), Throws.Exception);
        }
    }
}