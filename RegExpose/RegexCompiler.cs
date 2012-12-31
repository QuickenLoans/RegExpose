using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RegExpose.Nodes;
using RegExpose.Nodes.Alternation;
using RegExpose.Nodes.Anchors;
using RegExpose.Nodes.Backreferences;
using RegExpose.Nodes.Boundries;
using RegExpose.Nodes.Character;
using RegExpose.Nodes.Parens;
using RegExpose.Nodes.Quantifiers;
using RegExpose.PatternParsing;
using Sprache;

namespace RegExpose
{
    public class RegexCompiler
    {
        private const string SpecialCharacters = @"^$.[\|?*+(){";
        private const string WordCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890_";

        private readonly Parser<RegexNode> _anchor;
        private readonly Parser<RegexNode> _characterNode;
        private readonly Parser<RegexNode> _container;
        private readonly Parser<RegexNode> _node;
        private readonly Parser<RegexNode> _nonContainer;
        private readonly Parser<Regex> _regexParser;
        private readonly Parser<RegexNode> _wordBoundry;
        private readonly Parser<RegexNode> _backreference;

        public RegexCompiler(bool ignoreCase = false, bool singleLine = false, bool multiLine = false)
        {
            _characterNode = CreateQuantifierParser(CreateCharacterNodeParser(ignoreCase, singleLine));
            _anchor = CreateAnchorParser(multiLine);
            _wordBoundry = CreateWordBoundryParser();
            _backreference = CreateBackreferenceParser();
            _nonContainer = _characterNode.Or(_anchor).Or(_wordBoundry).Or(_backreference);

            _container = CreateQuantifierParser(x => GetContainer(x));

            _node = _container.Or(_nonContainer);

            _regexParser =
                (
                    from leadingChoices in
                        (from first in _node.AtLeastOnce()
                        from pipe in Parse.Char('|')
                        select first).AtLeastOnce()
                    from lastChoice in _node.AtLeastOnce()
                    let choices =
                        leadingChoices.Select(choice => CreateAlternationChoice(choice))
                            .Concat(new[] { CreateAlternationChoice(lastChoice) })
                    select new Alternation(choices, choices.First().Children.First().Index, choices.Skip(1).Aggregate(choices.First().Pattern, (s, choice) => s + "|" + choice.Pattern))
                ).Once()
                .Or(_node.AtLeastOnce())
                .ToRegexNode((nodes, index, pattern) => Parse.Return(new Regex(nodes, index, pattern)));
        }

        public Regex Compile(string pattern)
        {
            var regex = _regexParser.Parse(pattern);

            // Quick check to make sure we parsed the entire pattern...
            if (regex.Pattern != pattern)
            {
                if (regex.Pattern.Length < pattern.Length)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Unable to consume entire pattern (possible unmatched parentheses). Provided pattern was: /{0}/, parsed pattern was: /{1}/",
                            pattern,
                            regex.Pattern));
                }

                throw new InvalidOperationException(
                    string.Format("Parse error. Provided pattern: {0}, parsed pattern: {1}", pattern, regex.Pattern));
            }

            var id = 0;
            var capturingNumber = 1;
            AssignNodeIdsAndCapturingNumbers(regex, ref id, ref capturingNumber);

            return regex;
        }

        private static AlternationChoice CreateAlternationChoice(IEnumerable<RegexNode> choiceNodes)
        {
            var choiceNodesList = choiceNodes.ToList();
            return new AlternationChoice(choiceNodesList, choiceNodesList.First().Index, choiceNodesList.Aggregate("", (s, child) => s + child.Pattern));
        }

        private static void AssignNodeIdsAndCapturingNumbers(RegexNode node, ref int nextId, ref int nextCapturingNumber)
        {
            node.Id = nextId++;

            var containerNode = node as ContainerNode;
            if (containerNode != null)
            {
                var capturingParens = node as CapturingParens;
                if (capturingParens != null)
                {
                    capturingParens.Number = nextCapturingNumber++;
                }

                foreach (var child in containerNode.Children)
                {
                    AssignNodeIdsAndCapturingNumbers(child, ref nextId, ref nextCapturingNumber);
                }
            }
            else
            {
                var wrapperNode = node as WrapperNode;
                if (wrapperNode != null)
                {
                    AssignNodeIdsAndCapturingNumbers(wrapperNode.Child, ref nextId, ref nextCapturingNumber);
                }
                else
                {
                    var alternation = node as Alternation;
                    if (alternation != null)
                    {
                        foreach (var choice in alternation.Choices)
                        {
                            AssignNodeIdsAndCapturingNumbers(choice, ref nextId, ref nextCapturingNumber);
                        }
                    }
                }
            }
        }

        private static Parser<CharacterNode> CreateCharacterNodeParser(bool ignoreCase, bool singleLine)
        {
            var characterLiteral = CreateCharacterLiteralParser(ignoreCase);
            var characterClass = CreateCharacterClassParser(ignoreCase);
            var dot = CreateDotParser(singleLine);
            var characterClassShorthand = CreateCharacterClassShorthandParser();

            var characterNodeParser =
                dot.Select(x => (CharacterNode) x)
                    .Or(characterClassShorthand)
                    .Or(characterClass)
                    .Or(characterLiteral);
            return characterNodeParser;
        }

        private static Parser<CharacterLiteral> CreateCharacterLiteralParser(bool ignoreCase)
        {
            var simpleCharacter =
                Parse.CharExcept(c => SpecialCharacters.Any(escape => c == escape), "Special Characters")
                    .ToRegexNode(
                        (value, index, pattern) => Parse.Return(new CharacterLiteral(value, ignoreCase, index, pattern)));

            var escapedSpecialCharacter =
                (from backslash in Parse.Char('\\')
                 from escapedCharacter in
                     Parse.Char(c => SpecialCharacters.Any(escape => c == escape), "Special Characters")
                 select new string(new[] { backslash, escapedCharacter }))
                    .ToRegexNode(
                        (value, index, pattern) => Parse.Return(new CharacterLiteral(value, ignoreCase, index, pattern)));

            return escapedSpecialCharacter.Or(simpleCharacter);
        }

        private static Parser<CharacterClass> CreateCharacterClassParser(bool ignoreCase)
        {
            var characterClassAlwaysLegalCharacter = Parse.CharExcept(c => c == ']' || c == '-' || c == '\\',
                                                                      "Right Bracket or Backslash or Dash");

            var characterClassEscapedCharacters =
                from backslash in Parse.Char('\\')
                from escapedCharacter in Parse.Char(']').Or(Parse.Char('^')).Or(Parse.Char('-')).Or(Parse.Char('\\'))
                select new[] { backslash, escapedCharacter };

            var characterClassCharacter =
                characterClassAlwaysLegalCharacter.Once().Text().Or(characterClassEscapedCharacters.Text());

            // FYI: this is an example of non-consuming look-ahead
            var safeCharacterClassCharacter =
                from character in
                    characterClassCharacter.Except(
                        from x in characterClassCharacter
                        from y in Parse.Char('-')
                        from z in Parse.CharExcept(']')
                        select x)
                select character;

            var characterClassLiteralCharacters =
                from characters in safeCharacterClassCharacter.AtLeastOnce()
                select new CharacterClassLiteralCharacters(characters.Aggregate("", (s, c) => s + c), ignoreCase);

            var characterClassRange =
                from min in characterClassEscapedCharacters.Or(characterClassAlwaysLegalCharacter.Once()).Text()
                from dash in Parse.Char('-')
                from max in characterClassEscapedCharacters.Or(characterClassAlwaysLegalCharacter.Once()).Text()
                select new CharacterClassRange(min, max, ignoreCase);

            var optionalDash =
                Parse.Char('-').Once().Text().Select(x => new CharacterClassLiteralCharacters(x, ignoreCase)).XOr(
                    Parse.Return((CharacterClassLiteralCharacters) null));

            return
                (from openBracket in
                     Parse.Char('[')
                     .Except( // Look ahead to make sure we don't have an empty character class or negated character class
                     from x in Parse.Char('[')
                     from y in Parse.Char('^').XOr(Parse.Return('_'))
                     // (this matches caret if its there, but doesn't fail the match if it isn't)
                     from z in Parse.Char(']')
                     select x)
                 from negate in Parse.Char('^').Once().Text().XOr(Parse.Return((string) null))
                 from beginningDash in optionalDash
                 from internals in
                     characterClassRange.Select(x => (ICharacterClassPart) x).Or(characterClassLiteralCharacters).Many()
                 from endDash in optionalDash
                 from closeBracket in Parse.Char(']')
                 select new
                 {
                     Internals = GetInternals(beginningDash, internals, endDash),
                     Negated = negate == "^"
                 })
                    .ToRegexNode(
                        (value, index, pattern) =>
                        Parse.Return(new CharacterClass(value.Internals, value.Negated, ignoreCase, index, pattern)));
        }

        private static IEnumerable<ICharacterClassPart> GetInternals(CharacterClassLiteralCharacters beginningDash,
                                                                     IEnumerable<ICharacterClassPart> internals,
                                                                     CharacterClassLiteralCharacters endDash)
        {
            if (beginningDash != null)
            {
                internals = new ICharacterClassPart[] { beginningDash }.Concat(internals);
            }

            if (endDash != null)
            {
                internals = internals.Concat(new ICharacterClassPart[] { endDash });
            }

            return internals;
        }

        private static Parser<Dot> CreateDotParser(bool singleLine)
        {
            return
                Parse.Char('.').ToRegexNode((value, index, pattern) => Parse.Return(new Dot(singleLine, index, pattern)));
        }

        private static Parser<CharacterClassShorthand> CreateCharacterClassShorthandParser()
        {
            return (from backslash in Parse.Char('\\')
                    from shorthandCharacter in Parse.Char(c => "dDwWsS".Any(sc => sc == c), "Shorthand Character")
                    select new string(new[] { backslash, shorthandCharacter }))
                .ToRegexNode((value, index, pattern) => Parse.Return(new CharacterClassShorthand(value, index, pattern)));
        }

        private static Parser<RegexNode> CreateAnchorParser(bool multiLine)
        {
            Parser<RegexNode> startOfStringParser =
                Parse.String(@"\A").ToRegexNode((_, index, pattern) => Parse.Return(new StartOfString(index, pattern)));
            Parser<RegexNode> endOfStringParser =
                (from backslash in Parse.Char('\\')
                 from z in Parse.Char(c => c == 'z' || c == 'Z', "z or Z")
                 select z)
                    .ToRegexNode((z, index, pattern) => Parse.Return(new EndOfString(z == 'Z', index, pattern)));
            Parser<RegexNode> dollarParser =
                Parse.Char('$').ToRegexNode((_, index, pattern) => Parse.Return(new Dollar(multiLine, index, pattern)));
            Parser<RegexNode> caretParser =
                Parse.Char('^').ToRegexNode((_, index, pattern) => Parse.Return(new Caret(multiLine, index, pattern)));

            return startOfStringParser.Or(endOfStringParser).Or(dollarParser).Or(caretParser);
        }

        private static Parser<RegexNode> CreateWordBoundryParser()
        {
            Parser<RegexNode> wordBoundryParser =
                (from backslash in Parse.Char('\\')
                 from b in Parse.Char(c => c == 'b' || c == 'B', "b or B")
                 select b)
                    .ToRegexNode((b, index, pattern) => Parse.Return(new WordBoundry(b == 'B', index, pattern)));

            return wordBoundryParser;
        }

        private Parser<RegexNode> CreateBackreferenceParser()
        {
            Parser<RegexNode> backreferenceParser =
                (from backslash in Parse.Char('\\')
                 from number in Parse.Number
                 select number)
                    .ToRegexNode((number, index, pattern) => Parse.Return(new Backreference(int.Parse(number), index, pattern)));

            Parser<RegexNode> angleBracketNamedBackreferenceParser =
                (from backslash in Parse.Char('\\')
                 from k in Parse.Char('k')
                 from open in Parse.Char('<')
                 from name in Parse.Char(c => WordCharacters.Contains(c), "Word Characters").AtLeastOnce().Text()
                 from close in Parse.Char('>')
                 select name)
                    .ToRegexNode((name, index, pattern) => Parse.Return(new NamedBackreference(name, index, pattern)));

            Parser<RegexNode> tickNamedBackreferenceParser =
                (from backslash in Parse.Char('\\')
                 from k in Parse.Char('k')
                 from tick1 in Parse.Char('\'')
                 from name in Parse.Char(c => WordCharacters.Contains(c), "Word Characters").AtLeastOnce().Text()
                 from tick2 in Parse.Char('\'')
                 select name)
                    .ToRegexNode((name, index, pattern) => Parse.Return(new NamedBackreference(name, index, pattern)));

            return backreferenceParser.Or(angleBracketNamedBackreferenceParser).Or(tickNamedBackreferenceParser);
        }

        private static Parser<RegexNode> CreateQuantifierParser(Parser<RegexNode> childParser)
        {
            var quantifierParamsParser =
                from required in
                    Parse.Char('+').Select(x => new QuantifierParams
                    {
                        Shorthand = x
                    })
                    .Or(Parse.Char('*').Select(x => new QuantifierParams
                    {
                        Shorthand = x
                    }))
                    .Or(Parse.Char('?').Select(x => new QuantifierParams
                    {
                        Shorthand = x
                    }))
                    .Or(
                        from open in Parse.Char('{').Once().Text()
                        from min in Parse.Numeric.AtLeastOnce().Text()
                        from commaAndOrMax in
                            (from comma in Parse.Char(',').Once().Text()
                             from max in Parse.Numeric.AtLeastOnce().Text().XOr(Parse.Return(""))
                             select Tuple.Create(comma, max))
                            .XOr(Parse.Return(Tuple.Create("", "")))
                        from close in Parse.Char('}').Once().Text()
                        select new QuantifierParams
                        {
                            Min = int.Parse(min),
                            Max =
                            (commaAndOrMax.Item1 != "" && commaAndOrMax.Item2 != "")
                                ? int.Parse(commaAndOrMax.Item2)
                                : (commaAndOrMax.Item1 != "")
                                      ? (int?) null
                                      : int.Parse(min)
                        }
                    )
                from optional in
                    (Parse.Char('?').Select(x => (char?) x)
                    .Or(Parse.Char('+').Select(x => (char?) x))
                    .XOr(Parse.Return((char?) null)))
                select new QuantifierParams
                {
                    Shorthand = required.Shorthand,
                    Min = required.Min,
                    Max = required.Max,
                    Optional = optional
                };

            return
                (from child in childParser
                 from quantifierParams in quantifierParamsParser.XOr(Parse.Return((QuantifierParams) null))
                 select new
                 {
                     child,
                     quantifierParams
                 })
                    .ToRegexNode(
                        (result, index, pattern) =>
                        Parse.Return(result.quantifierParams != null
                                         ? CreateQuantifier(result.quantifierParams, result.child, index, pattern)
                                         : result.child));
        }

        private static RegexNode CreateQuantifier(QuantifierParams quantifierParams,
                                                  RegexNode child,
                                                  int index,
                                                  string pattern)
        {
            if (quantifierParams.Shorthand != null)
            {
                switch (quantifierParams.Shorthand)
                {
                    case '?':
                        if (quantifierParams.Optional != null)
                        {
                            if (quantifierParams.Optional == '?')
                            {
                                return new LazyQuestionMark(child, index, pattern);
                            }
                            return new PossessiveQuestionMark(child, index, pattern);
                        }
                        return new GreedyQuestionMark(child, index, pattern);
                    case '*':
                        if (quantifierParams.Optional != null)
                        {
                            if (quantifierParams.Optional == '?')
                            {
                                return new LazyStar(child, index, pattern);
                            }
                            return new PossessiveStar(child, index, pattern);
                        }
                        return new GreedyStar(child, index, pattern);
                    case '+':
                        if (quantifierParams.Optional != null)
                        {
                            if (quantifierParams.Optional == '?')
                            {
                                return new LazyPlus(child, index, pattern);
                            }
                            return new PossessivePlus(child, index, pattern);
                        }
                        return new GreedyPlus(child, index, pattern);
                }
            }

            Debug.Assert(quantifierParams.Min != null);

            if (quantifierParams.Optional != null)
            {
                if (quantifierParams.Optional == '?')
                {
                    return new LazyQuantifier(quantifierParams.Min.Value, quantifierParams.Max, child, index, pattern);
                }
                return new PossessiveQuantifier(quantifierParams.Min.Value, quantifierParams.Max, child, index, pattern);
            }
            return new GreedyQuantifier(quantifierParams.Min.Value, quantifierParams.Max, child, index, pattern);
        }

        private IResult<RegexNode> GetContainer(Input input)
        {
            if (input.AtEnd)
            {
                return new Failure<RegexNode>(input,
                                              () => "Unexpected end of input reached",
                                              () => new[] { "GetContainer" });
            }

            if (input.Current == '|')
            {
                // It's possible that we're at a pipe that is creating a top-level alternation. Fail so a higher level
                // parser can deal with it.
                return new Failure<RegexNode>(input,
                                              () => "Pipe was first character parsed",
                                              () => new[] { "GetContainer" });
            }

            var containerStack = new Stack<ContainerInfo>();
            var candidateNodes = new Dictionary<Guid, List<RegexNode>>();

            while (!input.AtEnd)
            {
                var success = _nonContainer(input) as ISuccess<RegexNode>;
                if (success != null)
                {
                    if (containerStack.Count == 0)
                    {
                        if (input.Position == 0)
                        {
                            // Assume that we might have a top-level alternation - if it turns out we don't, we'll fail anyway
                            containerStack.Push(new ContainerInfo
                            {
                                Index = input.Position,
                                ContainerType = ContainerType.Alternation
                            });
                        }
                        else
                        {
                            return new Failure<RegexNode>(input,
                                                          () =>
                                                          "Last character of input reached - captures are not possible",
                                                          () => new[] { "GetContainer" });
                        }
                    }

                    var key = containerStack.Peek().Key;

                    if (!candidateNodes.ContainsKey(key))
                    {
                        candidateNodes.Add(key, new List<RegexNode>());
                    }

                    candidateNodes[key].Add(success.Result);
                    input = success.Remainder;
                }
                else
                {
                    switch (input.Current)
                    {
                        case ')':
                            var containerInfo = containerStack.Pop();

                            if (containerInfo.ContainerType == ContainerType.Alternation)
                            {
                                var alternationKey = containerInfo.Key;
                                var alternationChildren = candidateNodes[alternationKey];

                                var alternation = CreateAlternation(input, alternationChildren);

                                containerInfo = containerStack.Pop();
                                candidateNodes[containerInfo.Key] = new List<RegexNode>
                                {
                                    alternation
                                };
                            }

                            var children = candidateNodes[containerInfo.Key];
                            candidateNodes.Remove(containerInfo.Key);
                            var index = containerInfo.Index;
                            var pattern = input.Source.Substring(containerInfo.Index,
                                                                 (input.Position - containerInfo.Index) + 1);

                            ContainerNode paren;

                            if (containerInfo.ParenType == ParenType.Capturing)
                            {
                                paren = new CapturingParens(children, index, pattern);
                            }
                            else if (containerInfo.ParenType == ParenType.NonCapturing)
                            {
                                paren = new NonCapturingParens(children, index, pattern);
                            }
                            else if (containerInfo.ParenType == ParenType.PositiveLookAhead)
                            {
                                paren = new LookAhead(false, children, index, pattern);
                            }
                            else if (containerInfo.ParenType == ParenType.NegativeLookAhead)
                            {
                                paren = new LookAhead(true, children, index, pattern);
                            }
                            else if (containerInfo.ParenType == ParenType.PositiveLookBehind)
                            {
                                paren = new LookBehind(false, children, index, pattern);
                            }
                            else if (containerInfo.ParenType == ParenType.NegativeLookBehind)
                            {
                                paren = new LookBehind(true, children, index, pattern);
                            }
                            else if (containerInfo.ParenType == ParenType.Atomic)
                            {
                                paren = new AtomicGrouping(children, index, pattern);
                            }
                            else
                            {
                                paren = new NamedCapture(containerInfo.ParenType.Name, children, index, pattern);
                            }

                            if (containerStack.Count > 0)
                            {
                                var parenKey = containerStack.Peek().Key;

                                if (!candidateNodes.ContainsKey(parenKey))
                                {
                                    candidateNodes.Add(parenKey, new List<RegexNode>());
                                }

                                candidateNodes[parenKey].Add(paren);
                            }
                            else
                            {
                                return new Success<RegexNode>(paren, input.Advance());
                            }

                            break;
                        case '(':
                            var parentTypeParser =
                                from leftParen in Parse.Char('(')
                                from p in
                                    (from question in Parse.Char('?')
                                     from p in
                                         Parse.Char(':').Select(x => ParenType.NonCapturing)
                                         .Or(Parse.Char('=').Select(x => ParenType.PositiveLookAhead))
                                         .Or(Parse.Char('!').Select(x => ParenType.NegativeLookAhead))
                                         .Or(Parse.Char('>').Select(x => ParenType.Atomic))
                                         .Or(Parse.String("<=").Select(x => ParenType.PositiveLookBehind))
                                         .Or(Parse.String("<!").Select(x => ParenType.NegativeLookBehind))
                                         .Or(
                                             from open in Parse.Char('<')
                                             from name in Parse.LetterOrDigit.Or(Parse.Char('_')).AtLeastOnce().Text()
                                             from close in Parse.Char('>')
                                             select ParenType.NamedCapture(name))
                                         .Or(
                                             from open in Parse.Char('\'')
                                             from name in Parse.LetterOrDigit.Or(Parse.Char('_')).AtLeastOnce().Text()
                                             from close in Parse.Char('\'')
                                             select ParenType.NamedCapture(name))
                                     select p)
                                    .XOr(Parse.Return(ParenType.Capturing))
                                select p;

                            var parenType = ((ISuccess<ParenType>) parentTypeParser(input)).Result;

                            containerStack.Push(new ContainerInfo
                            {
                                Index = input.Position,
                                ContainerType = ContainerType.Parens,
                                ParenType = parenType
                            });

                            input = parenType.Advance(input);
                            break;
                        case '|':
                            var container = containerStack.Peek();

                            if (container.ContainerType != ContainerType.Alternation)
                            {
                                var alternationNodes = candidateNodes[container.Key];
                                var partialAlternation = new AlternationMarker(alternationNodes.First().Index);

                                candidateNodes[container.Key] = new List<RegexNode>
                                {
                                    partialAlternation
                                };

                                var alternationInfo = new ContainerInfo
                                {
                                    Index = partialAlternation.Index,
                                    ContainerType = ContainerType.Alternation
                                };
                                containerStack.Push(alternationInfo);

                                container = containerStack.Peek();
                                candidateNodes[container.Key] = new List<RegexNode>(alternationNodes);
                            }

                            candidateNodes[container.Key].Add(new AlternationMarker(input.Position));
                            break;
                        default:
                            return new Failure<RegexNode>(input,
                                                          () => "Unexpected character found inside parenthesis",
                                                          () => new[] { "GetContainer" });
                    }

                    input = input.Advance();
                }
            }

            if (containerStack.Count == 1 && containerStack.Peek().ContainerType == ContainerType.Alternation)
            {
                var containerInfo = containerStack.Pop();
                var candidates = candidateNodes[containerInfo.Key];
                if (candidates.Any(c => c is AlternationMarker))
                {
                    var alternation = CreateAlternation(input, candidates);
                    return new Success<RegexNode>(alternation, input.AtEnd ? input : input.Advance());
                }
            }

            return new Failure<RegexNode>(input, () => "Unmatched parentheses", () => new[] { "GetContainer" });
        }

        private static Alternation CreateAlternation(Input input, List<RegexNode> alternationChildren)
        {
            var childIndex = 0;
            var choices = new List<AlternationChoice>();
            while (true)
            {
                var choiceChildren =
                    alternationChildren.Skip(childIndex).TakeWhile(c => !(c is AlternationMarker)).ToList();

                if (choiceChildren.Count == 0)
                {
                    break;
                }

                var firstChildIndex = choiceChildren.First().Index;
                var lastChild = choiceChildren.Last();

                choices.Add(new AlternationChoice(choiceChildren,
                                                  firstChildIndex,
                                                  input.Source.Substring(firstChildIndex,
                                                                         (lastChild.Index + lastChild.Pattern.Length)
                                                                         - firstChildIndex)));
                childIndex += choiceChildren.Count + 1;
            }

            var firstChoiceIndex = choices.First().Index;
            var lastChoice = choices.Last();

            var alternation = new Alternation(choices,
                                              choices.First().Index,
                                              input.Source.Substring(firstChoiceIndex,
                                                                     (lastChoice.Index + lastChoice.Pattern.Length)
                                                                     - firstChoiceIndex));
            return alternation;
        }

        #region Nested type: AlternationMarker

        private class AlternationMarker : RegexNode
        {
            public AlternationMarker(int index)
                : base(index, null)
            {
            }

            public override string NodeType
            {
                get { throw new NotSupportedException(); }
            }

            internal override IEnumerable<ParseStep> Parse(IRegexEngine engine)
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Nested type: ContainerInfo

        private class ContainerInfo
        {
            public readonly Guid Key = Guid.NewGuid();
            public int Index { get; set; }
            public ParenType ParenType { get; set; }
            public ContainerType ContainerType { get; set; }
        }

        #endregion

        #region Nested type: ContainerType

        private enum ContainerType
        {
            Parens,
            Alternation
        }

        #endregion

        #region Nested type: ParenType

        private class ParenType
        {
            public static readonly ParenType Capturing = new ParenType
            {
                Name = "<Capturing>"
            };

            public static readonly ParenType NonCapturing = new ParenType
            {
                Name = "<NonCapturing>"
            };

            public static readonly ParenType PositiveLookAhead = new ParenType
            {
                Name = "<PositiveLookAhead>"
            };

            public static readonly ParenType NegativeLookAhead = new ParenType
            {
                Name = "<NegativeLookAhead>"
            };

            public static readonly ParenType PositiveLookBehind = new ParenType
            {
                Name = "<PositiveLookBehind>"
            };

            public static readonly ParenType NegativeLookBehind = new ParenType
            {
                Name = "<NegativeLookBehind>"
            };

            public static readonly ParenType Atomic = new ParenType
            {
                Name = "<Atomic>"
            };

            public string Name { get; private set; }

            public static ParenType NamedCapture(string name)
            {
                return new ParenType
                {
                    Name = name
                };
            }

            public Input Advance(Input input)
            {
                if (this == Capturing)
                {
                    return input;
                }

                if (Name.Contains("<"))
                    // All types except Named have a '<' (and a '>') in their names. And those types have a fixed-length identifier.
                {
                    input = input.Advance().Advance();
                    // Advances past the '?' and the first character of the identifier.

                    if (this == PositiveLookBehind || this == NegativeLookBehind)
                    {
                        input = input.Advance();
                        // Look behind has an extra character in its identifier - advance past it.
                    }
                }
                else
                {
                    input = input.Advance().Advance().Advance();
                    // Advances past the '?' and the '<' and '>' or both '\'' characters in its identifier.

                    input = Name.Aggregate(input, (current, t) => current.Advance());
                    // Advances past the name, no matter how long it is.
                }

                return input;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                var other = obj as ParenType;
                if (other == null)
                {
                    return false;
                }

                return Name == other.Name;
            }

            public override int GetHashCode()
            {
                return (Name != null ? Name.GetHashCode() : 0);
            }

            public override string ToString()
            {
                return Name.Contains("<") ? Name.Replace("<", "").Replace(">", "") : "NamedCapture: " + Name;
            }

            public static bool operator ==(ParenType lhs, ParenType rhs)
            {
                if (ReferenceEquals(lhs, rhs))
                {
                    return true;
                }

                if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                {
                    return false;
                }

                return lhs.Name == rhs.Name;
            }

            public static bool operator !=(ParenType lhs, ParenType rhs)
            {
                return !(lhs == rhs);
            }
        }

        #endregion

        #region Nested type: QuantifierParams

        private class QuantifierParams
        {
            public char? Shorthand { get; set; }
            public int? Min { get; set; }
            public int? Max { get; set; }
            public char? Optional { get; set; }
        }

        #endregion
    }
}