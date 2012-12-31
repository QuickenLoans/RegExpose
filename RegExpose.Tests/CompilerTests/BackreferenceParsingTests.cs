using System.Linq;
using NUnit.Framework;
using RegExpose.Nodes.Backreferences;

namespace RegExpose.Tests.CompilerTests
{
    public class BackreferenceParsingTests
    {
        [TestCase(@"\1\2\3")]
        public void BackreferencesAreParsedCorrectly(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(3));
            Assert.That(nodes[0], Is.InstanceOf<Backreference>());
            Assert.That(((Backreference)nodes[0]).Number, Is.EqualTo(1));
            Assert.That(nodes[1], Is.InstanceOf<Backreference>());
            Assert.That(((Backreference)nodes[1]).Number, Is.EqualTo(2));
            Assert.That(nodes[2], Is.InstanceOf<Backreference>());
            Assert.That(((Backreference)nodes[2]).Number, Is.EqualTo(3));
        }

        [TestCase(@"\k<foo>\k<bar>")]
        public void AngleBracketNamedBackreferencesAreParsedCorrectly(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(2));
            Assert.That(nodes[0], Is.InstanceOf<NamedBackreference>());
            Assert.That(((NamedBackreference)nodes[0]).Name, Is.EqualTo("foo"));
            Assert.That(nodes[0].Index, Is.EqualTo(0));
            Assert.That(nodes[1], Is.InstanceOf<NamedBackreference>());
            Assert.That(((NamedBackreference)nodes[1]).Name, Is.EqualTo("bar"));
            Assert.That(nodes[1].Index, Is.EqualTo(7));
        }

        [TestCase(@"\k'foo'\k'bar'")]
        public void TickNamedBackreferencesAreParsedCorrectly(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var nodes = result.Children.ToList();
            Assert.That(nodes.Count, Is.EqualTo(2));
            Assert.That(nodes[0], Is.InstanceOf<NamedBackreference>());
            Assert.That(((NamedBackreference)nodes[0]).Name, Is.EqualTo("foo"));
            Assert.That(nodes[0].Index, Is.EqualTo(0));
            Assert.That(nodes[1], Is.InstanceOf<NamedBackreference>());
            Assert.That(((NamedBackreference)nodes[1]).Name, Is.EqualTo("bar"));
            Assert.That(nodes[1].Index, Is.EqualTo(7));
        }
    }
}