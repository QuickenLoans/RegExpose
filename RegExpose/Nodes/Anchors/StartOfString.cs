namespace RegExpose.Nodes.Anchors
{
    public class StartOfString : ZeroWidthAssertion
    {
        public StartOfString(int index, string pattern)
            : base(index, pattern)
        {
        }

        public override string NodeType
        {
            get { return "Start of String"; }
        }

        protected override bool Matches(State state)
        {
            return IsAtBeginningOfInput(state);
        }
    }
}