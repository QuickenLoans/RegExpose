using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RegExpose.Nodes;
using RegExpose.Nodes.Character;

namespace RegExpose.Tests.CompilerTests
{
    public class CharacterClassParsingTests
    {
        [TestCase("[]")]
        [TestCase("[\\]")]
        [TestCase("[^]")]
        public void InvalidCharacterClassThrowsException(string pattern)
        {
            var sut = new RegexCompiler();

            Assert.That(() => sut.Compile(pattern), Throws.Exception);
        }

        [Test]
        public void ParseSimpleCharacterClassIntoCharacterClass()
        {
            const string pattern = @"[a1]";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, false);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassLiteralCharacters(parts[0], "a1", false);
        }

        [Test]
        public void ParseSimpleCharacterClassIntoCharacterClassCaseInsensitive()
        {
            const string pattern = @"[a1]";
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, true, false);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassLiteralCharacters(parts[0], "a1", true);
        }

        [Test]
        public void ParseSingleRangedCharacterClassIntoCharacterClass()
        {
            const string pattern = @"[a-z]";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, false);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassRange(parts[0], "a", "z", false);
        }

        [Test]
        public void ParseSingleRangedCharacterClassIntoCharacterClassCaseInsensitive()
        {
            const string pattern = @"[a-z]";
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, true, false);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassRange(parts[0], "a", "z", true);
        }

        [Test]
        public void ParseMultipleRangedCharacterClassIntoCharacterClass()
        {
            const string pattern = @"[a-mA-M0-5]";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, false);
            var parts = AssertPartsCount(characterClass, 3);
            AssertCharacterClassRange(parts[0], "a", "m", false);
            AssertCharacterClassRange(parts[1], "A", "M", false);
            AssertCharacterClassRange(parts[2], "0", "5", false);
        }

        [Test]
        public void ParseMultipleRangedCharacterClassIntoCharacterClassCaseInsensitive()
        {
            const string pattern = @"[a-m0-5]";
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, true, false);
            var parts = AssertPartsCount(characterClass, 2);
            AssertCharacterClassRange(parts[0], "a", "m", true);
            AssertCharacterClassRange(parts[1], "0", "5", true);
        }

        [Test]
        public void ParseMixedPartsCharacterClassIntoCharacterClass()
        {
            const string pattern = @"[a-m!@#A-M]";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, false);
            var parts = AssertPartsCount(characterClass, 3);
            AssertCharacterClassRange(parts[0], "a", "m", false);
            AssertCharacterClassLiteralCharacters(parts[1], "!@#", false);
            AssertCharacterClassRange(parts[2], "A", "M", false);
        }

        [Test]
        public void ParseMixedPartsCharacterClassIntoCharacterClassCaseInsentive()
        {
            const string pattern = @"[a-f!@#g-m]";
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, true, false);
            var parts = AssertPartsCount(characterClass, 3);
            AssertCharacterClassRange(parts[0], "a", "f", true);
            AssertCharacterClassLiteralCharacters(parts[1], "!@#", true);
            AssertCharacterClassRange(parts[2], "g", "m", true);
        }

        [TestCase(@"[a-]")]
        [TestCase(@"[-a]")]
        public void ParseCharacterClassWithBeginningOrEndDashIntoCharacterClass(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, false);
            var parts = AssertPartsCount(characterClass, 2);
            AssertCharacterClassLiteralCharacters(parts[0], pattern.Substring(1, 1), false);
            AssertCharacterClassLiteralCharacters(parts[1], pattern.Substring(2, 1), false);
        }

        [Test]
        public void ParseSimpleCharacterClassIntoNegatedCharacterClass()
        {
            const string pattern = @"[^a1]";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, true);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassLiteralCharacters(parts[0], "a1", false);
        }

        [Test]
        public void ParseSimpleCharacterClassIntoNegatedCharacterClassCaseInsensitive()
        {
            const string pattern = @"[^a1]";
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, true, true);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassLiteralCharacters(parts[0], "a1", true);
        }

        [Test]
        public void ParseSingleRangedCharacterClassIntoNegatedCharacterClass()
        {
            const string pattern = @"[^a-z]";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, true);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassRange(parts[0], "a", "z", false);
        }

        [Test]
        public void ParseSingleRangedCharacterClassIntoNegatedCharacterClassCaseInsensitive()
        {
            const string pattern = @"[^a-z]";
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, true, true);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassRange(parts[0], "a", "z", true);
        }

        [Test]
        public void ParseMultipleRangedCharacterClassIntoNegatedCharacterClass()
        {
            const string pattern = @"[^A-M0-5]";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, true);
            var parts = AssertPartsCount(characterClass, 2);
            AssertCharacterClassRange(parts[0], "A", "M", false);
            AssertCharacterClassRange(parts[1], "0", "5", false);
        }

        [Test]
        public void ParseMultipleRangedCharacterClassIntoNegatedCharacterClassCaseInsensitive()
        {
            const string pattern = @"[^A-M0-5]";
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, true, true);
            var parts = AssertPartsCount(characterClass, 2);
            AssertCharacterClassRange(parts[0], "A", "M", true);
            AssertCharacterClassRange(parts[1], "0", "5", true);
        }

        [Test]
        public void ParseMixedPartsNegatedCharacterClassIntoCharacterClass()
        {
            const string pattern = @"[^a-m!@#A-M]";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, true);
            var parts = AssertPartsCount(characterClass, 3);
            AssertCharacterClassRange(parts[0], "a", "m", false);
            AssertCharacterClassLiteralCharacters(parts[1], "!@#", false);
            AssertCharacterClassRange(parts[2], "A", "M", false);
        }

        [Test]
        public void ParseMixedPartsNegatedCharacterClassIntoCharacterClassCaseInsensitive()
        {
            const string pattern = @"[^a-f!@#g-m]";
            var sut = new RegexCompiler(true);

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, true, true);
            var parts = AssertPartsCount(characterClass, 3);
            AssertCharacterClassRange(parts[0], "a", "f", true);
            AssertCharacterClassLiteralCharacters(parts[1], "!@#", true);
            AssertCharacterClassRange(parts[2], "g", "m", true);
        }

        [TestCase(@"[^a-]")]
        [TestCase(@"[^-a]")]
        public void ParseNegatedCharacterClassWithBeginningOrEndDashIntoCharacterClass(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, true);
            var parts = AssertPartsCount(characterClass, 2);
            AssertCharacterClassLiteralCharacters(parts[0], pattern.Substring(2, 1), false);
            AssertCharacterClassLiteralCharacters(parts[1], pattern.Substring(3, 1), false);
        }

        [TestCase("[-]")]
        [TestCase("[^-]")]
        public void ParseSingleDashInCharacterClassIntoCharacterClass(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, pattern[1] == '^');
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassLiteralCharacters(parts[0], "-", false);
        }

        [TestCase("[abcdef^]")]
        [TestCase("[a^bcdef]")]
        public void CaretInCharacterClassAfterFirstPositionIsAcceptedUnescaped(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, false);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassLiteralCharacters(parts[0], pattern.Substring(1, pattern.Length - 2), false);
        }

        [TestCase("[^abcdef^]")]
        [TestCase("[^^abcdef]")]
        public void CaretInNegatedCharacterClassAfterFirstPositionIsAcceptedUnescaped(string pattern)
        {
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            var characterClass = AssertCharacterClass(result.Children, pattern, false, true);
            var parts = AssertPartsCount(characterClass, 1);
            AssertCharacterClassLiteralCharacters(parts[0], pattern.Substring(2, pattern.Length - 3), false);
        }

        private static CharacterClass AssertCharacterClass(RegexNodeCollection children,
                                                           string pattern,
                                                           bool ignoreCase,
                                                           bool negated)
        {
            Assert.That(children.Count(), Is.EqualTo(1));
            Assert.That(children.First(), Is.InstanceOf<CharacterClass>());
            var characterClass = (CharacterClass) children.First();
            Assert.That(characterClass.Index, Is.EqualTo(0));
            Assert.That(characterClass.Pattern, Is.EqualTo(pattern));
            Assert.That(characterClass.IgnoreCase, Is.EqualTo(ignoreCase));
            Assert.That(characterClass.Negated, Is.EqualTo(negated));
            return characterClass;
        }

        private static IList<ICharacterClassPart> AssertPartsCount(CharacterClass characterClass, int expectedCount)
        {
            Assert.That(characterClass.Parts.Count, Is.EqualTo(expectedCount));
            return characterClass.Parts;
        }

        private static void AssertCharacterClassLiteralCharacters(ICharacterClassPart part,
                                                                  string expectedValue,
                                                                  bool expectedIgnoreCase)
        {
            Assert.That(part, Is.InstanceOf<CharacterClassLiteralCharacters>());
            var range = (CharacterClassLiteralCharacters) part;
            Assert.That(range.Value, Is.EqualTo(expectedValue));
            Assert.That(range.IgnoreCase, Is.EqualTo(expectedIgnoreCase));
        }

        private static void AssertCharacterClassRange(ICharacterClassPart part,
                                                      string expectedMin,
                                                      string expectedMax,
                                                      bool expectedIgnoreCase)
        {
            Assert.That(part, Is.InstanceOf<CharacterClassRange>());
            var range = (CharacterClassRange) part;
            Assert.That(range.Min, Is.EqualTo(expectedMin));
            Assert.That(range.Max, Is.EqualTo(expectedMax));
            Assert.That(range.IgnoreCase, Is.EqualTo(expectedIgnoreCase));
        }
    }
}