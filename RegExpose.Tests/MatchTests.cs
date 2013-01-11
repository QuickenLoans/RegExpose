using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace RegExpose.Tests
{
    public class MatchTests : MatchTestsBase
    {
        [TestCase("a", @"^a", RegexOptions.None)]
        [TestCase("1a", @"^a", RegexOptions.None)]
        [TestCase("1\na", @"^a", RegexOptions.None)]
        [TestCase("1\na", @"^a", RegexOptions.Multiline)]
        [TestCase("1\r\na", @"^a", RegexOptions.None)]
        [TestCase("1\r\na", @"^a", RegexOptions.Multiline)]
        public void Caret(string input, string pattern, RegexOptions options)
        {
            PerformTest(input, pattern, options);
        }

        [TestCase("a", @"\Aa")]
        [TestCase("1a", @"\Aa")]
        public void StartOfString(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }

        [TestCase("a", @"a$", RegexOptions.None)]
        [TestCase("a1", @"a$", RegexOptions.None)]
        [TestCase("a\n1", @"a$", RegexOptions.None)]
        [TestCase("a\n1", @"a$", RegexOptions.Multiline)]
        [TestCase("a\r\n1", @"a$", RegexOptions.None)]
        [TestCase("a\r\n1", @"a$", RegexOptions.Multiline)]
        public void Dollar(string input, string pattern, RegexOptions options)
        {
            PerformTest(input, pattern, options);
        }

        [TestCase("a", @"a\z")]
        [TestCase("a1", @"a\z")]
        [TestCase("a\n1", @"a\z")]
        [TestCase("a\n", @"a\z")]
        [TestCase("a", @"a\Z")]
        [TestCase("a1", @"a\Z")]
        [TestCase("a\n1", @"a\Z")]
        [TestCase("a\n", @"a\Z")]
        public void EndOfString(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }

        [TestCase("a", @"\ba\b")]
        [TestCase("a", @"\ba\b")]
        [TestCase("1a", @"\ba\ba")]
        [TestCase("1\na", @"\ba\b")]
        [TestCase("1\r\na", @"\ba\b")]
        [TestCase("a", @"\ba\b")]
        [TestCase("a1", @"\ba\b")]
        [TestCase("a\n1", @"\ba\b")]
        [TestCase("a\r\n1", @"\ba\b")]
        [TestCase("", @"\ba\b")]
        public void WordBoundry(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }

        [TestCase("a", @"a|b")]
        [TestCase("b", @"a|b")]
        [TestCase("abc", @"abc|xyz")]
        [TestCase("xyz", @"abc|xyz")]
        [TestCase("a", @"(a|b)")]
        [TestCase("b", @"(a|b)")]
        [TestCase("a", @"(a)|(b)")]
        [TestCase("b", @"(a)|(b)")]
        [TestCase("a", @"(a|b)?b")]
        [TestCase("b", @"(a|b)?a")]
        [TestCase("_qI8", @"^([a-zA-Z_]\w\w\w|\d\d\d\d|24680)$")]
        [TestCase("1234", @"^([a-zA-Z_]\w\w\w|\d\d\d\d|24680)$")]
        [TestCase("246800", @"^([a-zA-Z_]\w\w\w|\d\d\d\d|24680)$")]
        [TestCase("Doods", @"^([a-zA-Z_]\w\w\w|\d\d\d\d|24680)$")]
        [TestCase("a", @"(a|b)?a")]
        [TestCase("b", @"(a|b)?b")]
        [TestCase("24680", @"^([a-zA-Z_]\w\w\w|\d\d\d\d|24680)$")]
        public void Alternation(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }

        [TestCase("a", @".*?")]
        [TestCase("", @".*?")]
        [TestCase("<a>1</a><b>2</b>", @"<\w>.+?</\w>")]
        [TestCase("This is a <EM>first</EM> test", @"<.+?>")]
        public void LazyQuantifiers(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }

        [TestCase("<a>123</a>", @"<([a-z])>.*?</\1>")]
        public void Backreferences(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }

        [TestCase("foobar", @"foo(?=bar)")]
        [TestCase("foobaz", @"foo(?=bar)")]
        public void PositiveLookAhead(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }

        [TestCase("foobar", @"foo(?!bar)")]
        [TestCase("foobaz", @"foo(?!bar)")]
        public void NegativeLookAhead(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }

        [TestCase("USD100", @"\d{3}(?<=USD\d{3})")]
        [TestCase("USD100", @"(?<=USD)\d{3}")]
        [TestCase("blah USD100 blah", @"\d{3}(?<=USD\d{3})")]
        [TestCase("blah USD100 blah", @"(?<=USD)\d{3}")]
        [TestCase("JPY100", @"\d{3}(?<=USD\d{3})")]
        [TestCase("JPY100", @"(?<=USD)\d{3}")]
        [TestCase("blah JPY100 blah", @"\d{3}(?<=USD\d{3})")]
        [TestCase("blah JPY100 blah", @"(?<=USD)\d{3}")]
        public void PositiveLookBehind(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }

        //[TestCase("foobar", @"foo(?!bar)")]
        //[TestCase("foobaz", @"foo(?!bar)")]
        //public void NegativeLookBehind(string input, string pattern)
        //{
        //    PerformTest(input, pattern, RegexOptions.None);
        //}
    }

    public class KnownIssues : MatchTestsBase
    {
        [Category("KnownIssues")]
        public void Alternation(string input, string pattern)
        {
            PerformTest(input, pattern, RegexOptions.None);
        }
    }

    public class MatchTestsBase
    {
        protected static void PerformTest(string input, string pattern, RegexOptions options)
        {
            var compiler = new RegexCompiler(
                (options & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase,
                (options & RegexOptions.Singleline) == RegexOptions.Singleline,
                (options & RegexOptions.Multiline) == RegexOptions.Multiline);
            var engine = compiler.Compile(pattern).Parse(input);

            var matchesEnumerator = engine.GetMatches().GetEnumerator();
            var netMatchesEnumerator = System.Text.RegularExpressions.Regex.Matches(input, pattern, options)
                .Cast<System.Text.RegularExpressions.Match>().GetEnumerator();

            while (true)
            {
                var matchesHasNext = matchesEnumerator.MoveNext();
                var netMatchesHasNext = netMatchesEnumerator.MoveNext();

                var match = matchesEnumerator.Current;
                var netMatch = netMatchesEnumerator.Current;

                object un = match == null ? "un" : "";
                Debug.WriteLine("Match was {0}successful.", un);
                Debug.WriteLine("");
                DumpParseSteps(engine, pattern);

                Assert.That(matchesHasNext, Is.EqualTo(netMatchesHasNext));

                if (!matchesHasNext || !netMatchesHasNext)
                {
                    break;
                }

                Assert.That(match != null, Is.EqualTo(netMatch.Success));

                if (match != null)
                {
                    Assert.That(match.Value, Is.EqualTo(netMatch.Value));
                    Assert.That(match.Groups.Count, Is.EqualTo(netMatch.Groups.Count));

                    foreach (var @group in match.Groups)
                    {
                        var netGroup = netMatch.Groups[@group.Number];
                        Assert.That(@group.Value, Is.EqualTo(netGroup.Value));
                        Assert.That(@group.Captures.Count, Is.EqualTo(netGroup.Captures.Count), "Unequal Capture count in Group");

                        for (int i = 0; i < @group.Captures.Count; i++)
                        {
                            Assert.That(@group.Captures[i].Value, Is.EqualTo(netGroup.Captures[i].Value));
                        }
                    }
                }
            }
        }

        private static void DumpParseSteps(RegexEngine engine, string pattern)
        {
            Debug.WriteLine(
                    "| Index | Step          | Node Type                | {0} | Message",
                    "Pattern".PadRight(Math.Max(pattern.Length + 2, 7)));
            Debug.WriteLine(
                    "|-------+---------------+--------------------------+-{0}-+---------------------------------------------------",
                    "".PadRight(Math.Max(pattern.Length + 2, 7), '-'));
            Debug.WriteLine(engine.GetParseSteps().Aggregate(
                new StringBuilder(),
                (sb, step) => sb.AppendFormat(
                    "{0}{1}{2}{3}{4}",
                    "| " + step.StepIndex.ToString(CultureInfo.InvariantCulture).PadRight(6),
                    "| " + step.Type.ToString().PadRight(14),
                    "| " + step.NodeType.PadRight(25),
                    "| " + step.Pattern.PadRight(Math.Max(pattern.Length + 3, 8)),
                    "| " + step.Message).AppendLine()).ToString());
        }
    }
}