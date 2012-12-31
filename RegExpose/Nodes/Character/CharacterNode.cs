using System.Collections.Generic;

namespace RegExpose.Nodes.Character
{
    public abstract class CharacterNode : LeafNode, ICharacterMatcher
    {
        internal CharacterNode(bool ignoreCase, int index, string pattern)
            : base(index, pattern)
        {
            IgnoreCase = ignoreCase;
        }

        #region ICharacterMatcher Members

        public bool IgnoreCase { get; private set; }

        public abstract bool Matches(char input);

        #endregion

        internal override IEnumerable<ParseStep> Parse(IRegexEngine engine)
        {
            yield return ParseStep.BeginParse(this, engine.State);

            if (engine.State.Index >= engine.Input.Length)
            {
                yield return ParseStep.Fail(this, engine.State, engine.State);
                yield return ParseStep.Break(this);
                yield break;
            }

            if (Matches(engine.Input[engine.State.Index]))
            {
                var match = engine.Input.Substring(engine.State.Index, 1);
                
                var initialState = engine.State;
                engine.State = engine.State.Advance();

                yield return ParseStep.Pass(this, match, initialState, engine.State);
                yield return ParseStep.AdvanceIndex(this, engine.State);

                yield return ParseStep.Break(this);
            }
            else
            {
                yield return ParseStep.Fail(this, engine.State, engine.State);
                yield return ParseStep.Break(this);
            }
        }
    }
}