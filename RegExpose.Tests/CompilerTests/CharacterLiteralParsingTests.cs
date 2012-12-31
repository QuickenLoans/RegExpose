using NUnit.Framework;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CompilerTests
{
    public class CharacterLiteralParsingTests
    {
        [TestCase(@"[")]
        [TestCase(@"\")]
        [TestCase(@"|")]
        [TestCase(@"?")]
        [TestCase(@"*")]
        [TestCase(@"+")]
        [TestCase(@"(")]
        [TestCase(@")")]
        [TestCase(@"{")]
        public void SingleUnescapedSpecialCharacterThrowsException(string pattern)
        {
            var sut = new RegexCompiler();

            Assert.That(() => sut.Compile(pattern), Throws.Exception);
        }

        [Test]
        public void ParseSingleCharacterIntoCharacterLiteral()
        {
            const string pattern = @"a";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult(result, pattern);
        }

        [Test]
        public void ParseSingleCharacterIntoCharacterLiteralCaseInsensitive()
        {
            const string pattern = @"a";
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            AssertResult(result, pattern);
        }

        [TestCase(@"\[")]
        [TestCase(@"\\")]
        [TestCase(@"\^")]
        [TestCase(@"\$")]
        [TestCase(@"\.")]
        [TestCase(@"\|")]
        [TestCase(@"\?")]
        [TestCase(@"\*")]
        [TestCase(@"\+")]
        [TestCase(@"\(")]
        [TestCase(@"\)")]
        [TestCase(@"\{")]
        public void ParseEscapedSpecialCharacterIntoCharacterLiteral(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult(result, pattern);
        }

        [TestCase(@"\[")]
        [TestCase(@"\\")]
        [TestCase(@"\^")]
        [TestCase(@"\$")]
        [TestCase(@"\.")]
        [TestCase(@"\|")]
        [TestCase(@"\?")]
        [TestCase(@"\*")]
        [TestCase(@"\+")]
        [TestCase(@"\(")]
        [TestCase(@"\)")]
        [TestCase(@"\{")]
        public void ParseEscapedSpecialCharacterIntoCharacterLiteralCaseInsensitive(string pattern)
        {
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            AssertResult(result, pattern);
        }

        private static void AssertResult(Regex result, string pattern)
        {
            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<CharacterLiteral>());
            var stringLiteral = (CharacterLiteral) result.Children[0];
            Assert.That(stringLiteral.Index, Is.EqualTo(0));
            Assert.That(stringLiteral.Pattern, Is.EqualTo(pattern));
        }
    }
}