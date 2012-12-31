using System.Linq;
using NUnit.Framework;
using RegExpose.Nodes.Character;
using RegExpose.Nodes.Parens;

namespace RegExpose.Tests.CompilerTests
{
    public class MiscParensParsingTests
    {
        [Test]
        public void AtomicGroupingParsesCorrectly()
        {
            const string pattern = @"(?>a[0-9]\d.)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<AtomicGrouping>());
            var container = (AtomicGrouping)nodes[0];
            var children = container.Children.ToList();
            Assert.That(children.Count, Is.EqualTo(4));
            Assert.That(children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children[1], Is.InstanceOf<CharacterClass>());
            Assert.That(children[2], Is.InstanceOf<CharacterClassShorthand>());
            Assert.That(children[3], Is.InstanceOf<Dot>());
        }

        [Test]
        public void PositiveLookAheadParsesCorrectly()
        {
            const string pattern = @"(?=a[0-9]\d.)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<LookAhead>());
            var container = (LookAhead)nodes[0];
            Assert.That(container.Negative, Is.False);
            var children = container.Children.ToList();
            Assert.That(children.Count, Is.EqualTo(4));
            Assert.That(children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children[1], Is.InstanceOf<CharacterClass>());
            Assert.That(children[2], Is.InstanceOf<CharacterClassShorthand>());
            Assert.That(children[3], Is.InstanceOf<Dot>());
        }

        [Test]
        public void NegativeLookAheadParsesCorrectly()
        {
            const string pattern = @"(?!a[0-9]\d.)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<LookAhead>());
            var container = (LookAhead)nodes[0];
            Assert.That(container.Negative, Is.True);
            var children = container.Children.ToList();
            Assert.That(children.Count, Is.EqualTo(4));
            Assert.That(children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children[1], Is.InstanceOf<CharacterClass>());
            Assert.That(children[2], Is.InstanceOf<CharacterClassShorthand>());
            Assert.That(children[3], Is.InstanceOf<Dot>());
        }

        [Test]
        public void PositiveLookBehindParsesCorrectly()
        {
            const string pattern = @"(?<=a[0-9]\d.)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<LookBehind>());
            var container = (LookBehind)nodes[0];
            Assert.That(container.Negative, Is.False);
            var children = container.Children.ToList();
            Assert.That(children.Count, Is.EqualTo(4));
            Assert.That(children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children[1], Is.InstanceOf<CharacterClass>());
            Assert.That(children[2], Is.InstanceOf<CharacterClassShorthand>());
            Assert.That(children[3], Is.InstanceOf<Dot>());
        }

        [Test]
        public void NegativeLookBehindParsesCorrectly()
        {
            const string pattern = @"(?<!a[0-9]\d.)";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(1));
            Assert.That(nodes[0], Is.InstanceOf<LookBehind>());
            var container = (LookBehind)nodes[0];
            Assert.That(container.Negative, Is.True);
            var children = container.Children.ToList();
            Assert.That(children.Count, Is.EqualTo(4));
            Assert.That(children[0], Is.InstanceOf<CharacterLiteral>());
            Assert.That(children[1], Is.InstanceOf<CharacterClass>());
            Assert.That(children[2], Is.InstanceOf<CharacterClassShorthand>());
            Assert.That(children[3], Is.InstanceOf<Dot>());
        }
    }
}