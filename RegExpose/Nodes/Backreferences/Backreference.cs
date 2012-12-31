using System.Collections.Generic;
using System.Linq;
using RegExpose.Nodes.Character;

namespace RegExpose.Nodes.Backreferences
{
    public class Backreference : LeafNode
    {
        private readonly int _number;

        public Backreference(int number, int index, string pattern)
            : base(index, pattern)
        {
            _number = number;
        }

        public int Number
        {
            get { return _number; }
        }

        public override string NodeType
        {
            get { return string.Format("Backreference({0})", Number); }
        }

        internal override IEnumerable<ParseStep> Parse(IRegexEngine engine)
        {
            yield return ParseStep.BeginParse(this, engine.State);

            var initialState = engine.State;

            var capture = engine.GetCaptures(Number).FirstOrDefault();
            if (capture == null || string.IsNullOrEmpty(capture.Value))
            {
                yield return ParseStep.Fail(this, initialState, engine.State, "No backreference value found");
                yield return ParseStep.Break(this);
            }
            else
            {
                var literals = capture.Value.Select((c, i) => new CharacterLiteral(c, false, capture.Index + i, new string(new[] { c })));
                foreach (var literal in literals)
                {
                    var success = false;

                    foreach (var result in literal.Parse(engine))
                    {
                        if (result.Type == ParseStepType.Break)
                        {
                            break;
                        }

                        yield return result;

                        if (result.Type == ParseStepType.Pass)
                        {
                            success = true;
                        }
                    }

                    if (!success)
                    {
                        yield return ParseStep.Fail(this, initialState, engine.State);
                        yield return ParseStep.Break(this);
                        yield break;
                    }
                }

                yield return ParseStep.Pass(this, capture.Value, initialState, engine.State);
                yield return ParseStep.Break(this);
            }
        }
    }
}