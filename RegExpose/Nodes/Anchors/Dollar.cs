namespace RegExpose.Nodes.Anchors
{
    public class Dollar : ZeroWidthAssertion
    {
        public Dollar(bool multiLine, int index, string pattern)
            : base(index, pattern)
        {
            MultiLine = multiLine;
        }

        public bool MultiLine { get; private set; }

        public override string NodeType
        {
            get { return "Dollar"; }
        }

        protected override bool Matches(State state)
        {
            if (IsAtEndOfInput(state))
            {
                return true;
            }

            if (MultiLine)
            {
                return IsAtEndOfLine(state);
            }

            return false;
        }

        protected override string AdditionalSettingsString
        {
            get { return MultiLine ? "(MultiLine:true)" : ""; }
        }
    }
}