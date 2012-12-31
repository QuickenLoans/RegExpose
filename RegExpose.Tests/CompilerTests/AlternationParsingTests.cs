using System.Linq;
using NUnit.Framework;
using RegExpose.Nodes.Alternation;
using RegExpose.Nodes.Character;
using RegExpose.Nodes.Parens;

namespace RegExpose.Tests.CompilerTests
{
    public class AlternationParsingTests
    {
        [Test]
        public void AnAlternationWithTwoCharacterLiteralChoicesParsesCorrectly()
        {
            const string pattern = @"a|b";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<Alternation>());
            var alternation = (Alternation) nodes[0];
            var choices = alternation.Choices.ToList();
            Assert.That(choices.Count, Is.EqualTo(2));
            Assert.That(choices[0], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[1], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[0].Children.Count, Is.EqualTo(1));
            Assert.That(choices[1].Children.Count, Is.EqualTo(1));
            Assert.That(choices[0].Children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(choices[1].Children[0], Is.InstanceOf<CharacterLiteral>());
        }

        [Test]
        public void AnAlternationWithAllCharacterNodesChoicesParsesCorrectly()
        {
            const string pattern = @"abc|[0-9]|\s|.";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<Alternation>());
            var alternation = (Alternation) nodes[0];
            var choices = alternation.Choices.ToList();
            Assert.That(choices.Count, Is.EqualTo(4));
            Assert.That(choices[0], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[1], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[2], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[3], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[0].Children.Count, Is.EqualTo(3));
            Assert.That(choices[1].Children.Count, Is.EqualTo(1));
            Assert.That(choices[2].Children.Count, Is.EqualTo(1));
            Assert.That(choices[3].Children.Count, Is.EqualTo(1));
            Assert.That(choices[0].Children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(choices[0].Children[1], Is.InstanceOf<CharacterLiteral>());
            Assert.That(choices[0].Children[2], Is.InstanceOf<CharacterLiteral>());
            Assert.That(choices[1].Children[0], Is.InstanceOf<CharacterClass>());
            Assert.That(choices[2].Children[0], Is.InstanceOf<CharacterClassShorthand>());
            Assert.That(choices[3].Children[0], Is.InstanceOf<Dot>());
        }

        [Test]
        public void CapturedChoices()
        {
            const string pattern = @"(a)(b)|(c)(d)|(e)(f)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<Alternation>());
            var alternation = (Alternation)nodes[0];
            var choices = alternation.Choices.ToList();
            Assert.That(choices.Count, Is.EqualTo(3));
            Assert.That(choices[0], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[1], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[2], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[0].Children.Count, Is.EqualTo(2));
            Assert.That(choices[1].Children.Count, Is.EqualTo(2));
            Assert.That(choices[2].Children.Count, Is.EqualTo(2));
            Assert.That(choices[0].Children[0], Is.InstanceOf<CapturingParens>());
            Assert.That(((CapturingParens)choices[0].Children[0]).Number, Is.EqualTo(1));
            Assert.That(choices[0].Children[1], Is.InstanceOf<CapturingParens>());
            Assert.That(((CapturingParens)choices[0].Children[1]).Number, Is.EqualTo(2));
            Assert.That(choices[1].Children[0], Is.InstanceOf<CapturingParens>());
            Assert.That(((CapturingParens)choices[1].Children[0]).Number, Is.EqualTo(3));
            Assert.That(choices[1].Children[1], Is.InstanceOf<CapturingParens>());
            Assert.That(((CapturingParens)choices[1].Children[1]).Number, Is.EqualTo(4));
            Assert.That(choices[2].Children[0], Is.InstanceOf<CapturingParens>());
            Assert.That(((CapturingParens)choices[2].Children[0]).Number, Is.EqualTo(5));
            Assert.That(choices[2].Children[1], Is.InstanceOf<CapturingParens>());
            Assert.That(((CapturingParens)choices[2].Children[1]).Number, Is.EqualTo(6));
        }

        [Test]
        public void CapturedAlternation()
        {
            const string pattern = @"(a|b)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<CapturingParens>());
            var parens = (CapturingParens)nodes[0];
            var parensChildren = parens.Children.ToList();
            Assert.That(parensChildren.Count, Is.EqualTo(1));
            Assert.That(parensChildren[0], Is.InstanceOf<Alternation>());
            var alternation = (Alternation)parensChildren[0];
            var choices = alternation.Choices.ToList();
            Assert.That(choices.Count, Is.EqualTo(2));
            Assert.That(choices[0], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[1], Is.InstanceOf<AlternationChoice>());
            Assert.That(choices[0].Children.Count, Is.EqualTo(1));
            Assert.That(choices[1].Children.Count, Is.EqualTo(1));
            Assert.That(choices[0].Children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(choices[1].Children[0], Is.InstanceOf<CharacterLiteral>());
        }
    }
}