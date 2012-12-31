namespace RegExpose
{
    public class State
    {
        private readonly int _index;
        private readonly string _input;

        internal State(string input, int index)
        {
            _input = input;
            _index = index;
        }

        public int Index
        {
            get { return _index; }
        }
        
        public string Input
        {
            get { return _input; }
        }

        public string Current
        {
            get { return Index >= Input.Length ? "<end of string>" : new string(new[] { Input[Index] }); }
        }

        internal State Advance()
        {
            return new State(Input, Index + 1);
        }

        internal State Plus(int n)
        {
            return n == 0 ? this : new State(Input, Index + n);
        }

        public override string ToString()
        {
            return string.Format("'{0}' - Index {1} in '{2}'", Current, Index, Input);
        }
    }
}