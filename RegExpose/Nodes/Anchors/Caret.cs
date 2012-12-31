namespace RegExpose.Nodes.Anchors
{
    public class Caret : ZeroWidthAssertion
    {
        public Caret(bool multiLine, int index, string pattern)
            : base(index, pattern)
        {
            MultiLine = multiLine;
        }

        public bool MultiLine { get; private set; }

        public override string NodeType
        {
            get { return "Caret"; }
        }

        protected override bool Matches(State state)
        {
            if (IsAtBeginningOfInput(state))
            {
                return true;
            }

            if (MultiLine)
            {
                return IsAtBeginningOfLine(state);
            }

            return false;
        }

        protected override string AdditionalSettingsString
        {
            get { return MultiLine ? "(MultiLine:true)" : ""; }
        }
    }
}