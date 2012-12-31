using NUnit.Framework;

namespace RegExpose.Tests.CompilerTests
{
    public class RegexParsingTests
    {
        [Test]
        public void DotIsParsedIntoDotWhenSingleLineIsFalse()
        {
            const string pattern = "abcd";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            Assert.That(result, Is.InstanceOf<Regex>());
            Assert.That(result.Children, Is.Not.Empty);
        }
    }
}