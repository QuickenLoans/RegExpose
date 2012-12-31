namespace RegExpose.Nodes.Character
{
    public class Dot : CharacterNode
    {
        internal Dot(bool singleLine, int index, string pattern)
            : base(false, index, pattern)
        {
            SingleLine = singleLine;
        }

        public bool SingleLine { get; private set; }

        public override bool Matches(char input)
        {
            if (SingleLine)
            {
                return true;
            }

            return input != '\n';
        }

        public override string NodeType
        {
            get { return "Dot"; }
        }

        internal override string GetPassMessage(string match, State initialState)
        {
            return string.Format("{0}{1} matched '{2}' at index {3}",
                                 NodeType,
                                 SingleLine ? " (SingleLine:true)" : "",
                                 match,
                                 initialState.Index);
        }

        internal override string GetFailMessage(State initialState)
        {
            return string.Format("{0}{1} failed to match '{2}' at index {3}",
                                 NodeType,
                                 SingleLine ? " (SingleLine:true)" : "",
                                 initialState.Current,
                                 initialState.Index);
        }
    }
}