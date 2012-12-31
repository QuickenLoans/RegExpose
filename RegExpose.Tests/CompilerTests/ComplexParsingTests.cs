using NUnit.Framework;
using RegExpose.Nodes.Alternation;
using RegExpose.Nodes.Anchors;
using RegExpose.Nodes.Character;
using RegExpose.Nodes.Parens;
using RegExpose.Nodes.Quantifiers;

namespace RegExpose.Tests.CompilerTests
{
    public class ComplexParsingTests
    {
        [TestCase(@"^([a-z]{2}\d{3,}?)+(?:th(?:is|at|e other).?)*?$", true, true, true, 1)]
        public void Test(string pattern, bool ignoreCase, bool singleLine, bool multiLine, int testNumber)
        {
            var sut = new RegexCompiler(ignoreCase, singleLine, multiLine);

            var result = sut.Compile(pattern);

            switch (testNumber)
            {
                case 1:
                    Test1(result);
                    break;
            }
        }

        private static void Test1(Regex result)
        {
            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Children.Count, Is.EqualTo(4));

            var child0 = result.Children[0] as Caret;
            Assert.That(child0, Is.Not.Null);
            Assert.That(child0.MultiLine, Is.True);

            var child1 = result.Children[1] as GreedyPlus;
            Assert.That(child1, Is.Not.Null);

            var child10 = child1.Child as CapturingParens;
            Assert.That(child10, Is.Not.Null);
            Assert.That(child10.Children.Count, Is.EqualTo(2));

            var child100 = child10.Children[0] as GreedyQuantifier;
            Assert.That(child100, Is.Not.Null);
            Assert.That(child100.Min, Is.EqualTo(2));
            Assert.That(child100.Max, Is.EqualTo(2));

            var child1000 = child100.Child as CharacterClass;
            Assert.That(child1000, Is.Not.Null);
            Assert.That(child1000.Parts.Count, Is.EqualTo(1));

            var child1000Part0 = child1000.Parts[0] as CharacterClassRange;
            Assert.That(child1000Part0, Is.Not.Null);
            Assert.That(child1000Part0.Min, Is.EqualTo("a"));
            Assert.That(child1000Part0.Max, Is.EqualTo("z"));
            Assert.That(child1000Part0.IgnoreCase, Is.True);

            var child101 = child10.Children[1] as LazyQuantifier;
            Assert.That(child101, Is.Not.Null);
            Assert.That(child101.Min, Is.EqualTo(3));
            Assert.That(child101.Max, Is.Null);

            var child1010 = child101.Child as CharacterClassShorthand;
            Assert.That(child1010, Is.Not.Null);
            Assert.That(child1010.Shorthand, Is.EqualTo(Shorthand.Digit));

            var child2 = result.Children[2] as LazyStar;
            Assert.That(child2, Is.Not.Null);

            var child20 = child2.Child as NonCapturingParens;
            Assert.That(child20, Is.Not.Null);
            Assert.That(child20.Children.Count, Is.EqualTo(4));

            var child200 = child20.Children[0] as CharacterLiteral;
            Assert.That(child200, Is.Not.Null);
            Assert.That(child200.Value, Is.EqualTo("t"));
            Assert.That(child200.IgnoreCase, Is.True);

            var child201 = child20.Children[1] as CharacterLiteral;
            Assert.That(child201, Is.Not.Null);
            Assert.That(child201.Value, Is.EqualTo("h"));
            Assert.That(child201.IgnoreCase, Is.True);

            var child202 = child20.Children[2] as NonCapturingParens;
            Assert.That(child202, Is.Not.Null);
            Assert.That(child202.Children.Count, Is.EqualTo(1));

            var child2020 = child202.Children[0] as Alternation;
            Assert.That(child2020, Is.Not.Null);
            Assert.That(child2020.Choices.Count, Is.EqualTo(3));

            var child2020Choice0 = child2020.Choices[0];
            Assert.That(child2020Choice0.Children.Count, Is.EqualTo(2));

            var child2020Choice00 = child2020Choice0.Children[0] as CharacterLiteral;
            Assert.That(child2020Choice00, Is.Not.Null);
            Assert.That(child2020Choice00.Value, Is.EqualTo("i"));
            Assert.That(child2020Choice00.IgnoreCase, Is.True);

            var child2020Choice01 = child2020Choice0.Children[1] as CharacterLiteral;
            Assert.That(child2020Choice01, Is.Not.Null);
            Assert.That(child2020Choice01.Value, Is.EqualTo("s"));
            Assert.That(child2020Choice01.IgnoreCase, Is.True);

            var child2020Choice1 = child2020.Choices[1];
            Assert.That(child2020Choice1.Children.Count, Is.EqualTo(2));

            var child2020Choice10 = child2020Choice1.Children[0] as CharacterLiteral;
            Assert.That(child2020Choice10, Is.Not.Null);
            Assert.That(child2020Choice10.Value, Is.EqualTo("a"));
            Assert.That(child2020Choice10.IgnoreCase, Is.True);

            var child2020Choice11 = child2020Choice1.Children[1] as CharacterLiteral;
            Assert.That(child2020Choice11, Is.Not.Null);
            Assert.That(child2020Choice11.Value, Is.EqualTo("t"));
            Assert.That(child2020Choice11.IgnoreCase, Is.True);

            var child2020Choice2 = child2020.Choices[2];
            Assert.That(child2020Choice2.Children.Count, Is.EqualTo(7));

            var child2020Choice20 = child2020Choice2.Children[0] as CharacterLiteral;
            Assert.That(child2020Choice20, Is.Not.Null);
            Assert.That(child2020Choice20.Value, Is.EqualTo("e"));
            Assert.That(child2020Choice20.IgnoreCase, Is.True);

            var child2020Choice21 = child2020Choice2.Children[1] as CharacterLiteral;
            Assert.That(child2020Choice21, Is.Not.Null);
            Assert.That(child2020Choice21.Value, Is.EqualTo(" "));
            Assert.That(child2020Choice21.IgnoreCase, Is.True);

            var child2020Choice22 = child2020Choice2.Children[2] as CharacterLiteral;
            Assert.That(child2020Choice22, Is.Not.Null);
            Assert.That(child2020Choice22.Value, Is.EqualTo("o"));
            Assert.That(child2020Choice22.IgnoreCase, Is.True);

            var child2020Choice23 = child2020Choice2.Children[3] as CharacterLiteral;
            Assert.That(child2020Choice23, Is.Not.Null);
            Assert.That(child2020Choice23.Value, Is.EqualTo("t"));
            Assert.That(child2020Choice23.IgnoreCase, Is.True);

            var child2020Choice24 = child2020Choice2.Children[4] as CharacterLiteral;
            Assert.That(child2020Choice24, Is.Not.Null);
            Assert.That(child2020Choice24.Value, Is.EqualTo("h"));
            Assert.That(child2020Choice24.IgnoreCase, Is.True);

            var child2020Choice25 = child2020Choice2.Children[5] as CharacterLiteral;
            Assert.That(child2020Choice25, Is.Not.Null);
            Assert.That(child2020Choice25.Value, Is.EqualTo("e"));
            Assert.That(child2020Choice25.IgnoreCase, Is.True);

            var child2020Choice26 = child2020Choice2.Children[6] as CharacterLiteral;
            Assert.That(child2020Choice26, Is.Not.Null);
            Assert.That(child2020Choice26.Value, Is.EqualTo("r"));
            Assert.That(child2020Choice26.IgnoreCase, Is.True);

            var child203 = child20.Children[3] as GreedyQuestionMark;
            Assert.That(child203, Is.Not.Null);

            var child2030 = child203.Child as Dot;
            Assert.That(child2030, Is.Not.Null);
            Assert.That(child2030.SingleLine, Is.True);

            var child3 = result.Children[3] as Dollar;
            Assert.That(child3, Is.Not.Null);
            Assert.That(child3.MultiLine, Is.True);
            // ReSharper restore PossibleNullReferenceException
        }
    }
}