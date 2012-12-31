using System.Collections.Generic;

namespace RegExpose.Nodes
{
    public abstract class ZeroWidthAssertion : LeafNode
    {
        internal ZeroWidthAssertion(int index, string pattern)
            : base(index, pattern)
        {
        }

        internal override sealed IEnumerable<ParseStep> Parse(IRegexEngine engine)
        {
            yield return ParseStep.BeginParse(this, engine.State);

            if (Matches(engine.State))
            {
                yield return ParseStep.Pass(this, "", engine.State, engine.State);
            }
            else
            {
                yield return ParseStep.Fail(this, engine.State, engine.State);
            }

            yield return ParseStep.Break(this);
        }

        protected static bool IsAtBeginningOfInput(State state)
        {
            return state.Index == 0;
        }

        protected static bool IsAtBeginningOfLine(State state)
        {
            return state.Input.Length > 0 && state.Input[state.Index - 1] == '\n';
        }

        protected static bool IsAtEndOfInput(State state)
        {
            return state.Index == state.Input.Length;
        }

        protected static bool IsAtEndOfLine(State state)
        {
            return state.Index < state.Input.Length - 1 && state.Input[state.Index] == '\n';
        }

        protected static bool IsAtFinalNewLine(State state)
        {
            return state.Index == state.Input.Length - 1 && state.Input[state.Index] == '\n';
        }

        protected abstract bool Matches(State state);

        internal override string GetPassMessage(string match, State initialState)
        {
            return string.Format("{0}, /{1}/{2}, matched starting at index {3}",
                                 NodeType,
                                 Pattern,
                                 !string.IsNullOrEmpty(AdditionalSettingsString) ? " " + AdditionalSettingsString : AdditionalSettingsString,
                                 initialState.Index);
        }

        internal override string GetFailMessage(State initialState)
        {
            return string.Format("{0}, /{1}/{2}, failed to match starting at index {3}",
                                 NodeType,
                                 Pattern,
                                 !string.IsNullOrEmpty(AdditionalSettingsString) ? " " + AdditionalSettingsString : AdditionalSettingsString,
                                 initialState.Index);
        }

        protected virtual string AdditionalSettingsString
        {
            get { return ""; }
        }
    }
}