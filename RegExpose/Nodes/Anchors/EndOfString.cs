namespace RegExpose.Nodes.Anchors
{
    public class EndOfString : ZeroWidthAssertion
    {
        public EndOfString(bool matchesFinalNewLine, int index, string pattern)
            : base(index, pattern)
        {
            MatchesFinalNewLine = matchesFinalNewLine;
        }

        public bool MatchesFinalNewLine { get; private set; }

        public override string NodeType
        {
            get { return "End of String"; }
        }

        protected override bool Matches(State state)
        {
            if (IsAtEndOfInput(state))
            {
                return true;
            }

            if (MatchesFinalNewLine)
            {
                return IsAtFinalNewLine(state);
            }

            return false;
        }

        protected override string AdditionalSettingsString
        {
            get { return string.Format("(MatchesFinalNewLine:{0})", MatchesFinalNewLine); }
        }
    }
}