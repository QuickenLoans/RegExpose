namespace RegExpose.Nodes.Character
{
    public class CharacterLiteral : CharacterNode
    {
        private readonly char _characterForMatching;

        internal CharacterLiteral(char value, bool ignoreCase, int index, string pattern)
            : base(ignoreCase, index, pattern)
        {
            Value = new string(new[] { value });
            _characterForMatching = this.FixCase(value);
        }

        internal CharacterLiteral(string escapedSpecialCharacter, bool ignoreCase, int index, string pattern)
            : base(ignoreCase, index, pattern)
        {
            Value = escapedSpecialCharacter;
            _characterForMatching = this.FixCase(escapedSpecialCharacter[1]);
        }

        public string Value { get; private set; }

        public override bool Matches(char input)
        {
            return _characterForMatching == this.FixCase(input);
        }

        public override string NodeType
        {
            get { return "Character Literal"; }
        }

        internal override string GetPassMessage(string match, State initialState)
        {
            return string.Format("{0}, /{1}/{2}, matched '{3}' at index {4}",
                                 NodeType,
                                 Pattern,
                                 IgnoreCase ? " (IgnoreCase:true)" : "",
                                 match,
                                 initialState.Index);
        }

        internal override string GetFailMessage(State initialState)
        {
            return string.Format("{0}, /{1}/{2}, failed to match '{3}' at index {4}",
                                 NodeType,
                                 Pattern,
                                 IgnoreCase ? " (IgnoreCase:true)" : "",
                                 initialState.Current,
                                 initialState.Index);
        }
    }
}