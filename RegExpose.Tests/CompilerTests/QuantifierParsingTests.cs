using NUnit.Framework;
using RegExpose.Nodes.Quantifiers;

namespace RegExpose.Tests.CompilerTests
{
    public class QuantifierParsingTests
    {
        private static readonly object[] _characterNodes =
            {
                new object[] { @"A" },
                new object[] { @"[a-z]" },
                new object[] { @"." },
                new object[] { @"\d" }
            };

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void GreedyPlus(string characterNode)
        {
            var pattern = characterNode + "+";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<GreedyPlus>(result, 1, null);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void GreedyQuantifierMinAndMaxSpecified(string characterNode)
        {
            var pattern = characterNode + "{2,4}";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<GreedyQuantifier>(result, 2, 4);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void GreedyQuantifierMaxOmittedWithComma(string characterNode)
        {
            var pattern = characterNode + "{2,}";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<GreedyQuantifier>(result, 2, null);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void GreedyQuantifierMaxOmittedCommaOmitted(string characterNode)
        {
            var pattern = characterNode + "{2}";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<GreedyQuantifier>(result, 2, 2);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void GreedyQuestionMark(string characterNode)
        {
            var pattern = characterNode + "?";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<GreedyQuestionMark>(result, 0, 1);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void GreedyStar(string characterNode)
        {
            var pattern = characterNode + "*";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<GreedyStar>(result, 0, null);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void LazyPlus(string characterNode)
        {
            var pattern = characterNode + "+?";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<LazyPlus>(result, 1, null);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void LazyQuantifierMinAndMaxSpecified(string characterNode)
        {
            var pattern = characterNode + "{2,4}?";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<LazyQuantifier>(result, 2, 4);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void LazyQuantifierMaxOmittedWithComma(string characterNode)
        {
            var pattern = characterNode + "{2,}?";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<LazyQuantifier>(result, 2, null);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void LazyQuantifierMaxOmittedCommaOmitted(string characterNode)
        {
            var pattern = characterNode + "{2}?";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<LazyQuantifier>(result, 2, 2);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void LazyQuestionMark(string characterNode)
        {
            var pattern = characterNode + "??";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<LazyQuestionMark>(result, 0, 1);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void LazyStar(string characterNode)
        {
            var pattern = characterNode + "*?";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<LazyStar>(result, 0, null);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void PossessivePlus(string characterNode)
        {
            var pattern = characterNode + "++";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<PossessivePlus>(result, 1, null);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void PossessiveQuantifierMinAndMaxSpecified(string characterNode)
        {
            var pattern = characterNode + "{2,4}+";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<PossessiveQuantifier>(result, 2, 4);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void PossessiveQuantifierMaxOmittedWithComma(string characterNode)
        {
            var pattern = characterNode + "{2,}+";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<PossessiveQuantifier>(result, 2, null);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void PossessiveQuantifierMaxOmittedCommaOmitted(string characterNode)
        {
            var pattern = characterNode + "{2}+";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<PossessiveQuantifier>(result, 2, 2);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void PossessiveQuestionMark(string characterNode)
        {
            var pattern = characterNode + "?+";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<PossessiveQuestionMark>(result, 0, 1);
        }

        [Test]
        [TestCaseSource("CharacterNodes")]
        public void PossessiveStar(string characterNode)
        {
            var pattern = characterNode + "*+";
            var sut = new RegexCompiler();

            var result = sut.Compile(pattern);

            AssertResult<PossessiveStar>(result, 0, null);
        }

        public object[] CharacterNodes
        {
            get { return _characterNodes; }
        }

        protected static void AssertResult<TQuantifier>(Regex result, int? expectedMin, int? expectedMax)
            where TQuantifier : Quantifier
        {
            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.InstanceOf<TQuantifier>());
            var quantifier = (TQuantifier) result.Children[0];
            Assert.That(quantifier.Min, Is.EqualTo(expectedMin));
            Assert.That(quantifier.Max, Is.EqualTo(expectedMax));
        }
    }
}