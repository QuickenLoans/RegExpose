using System.Diagnostics;

namespace RegExpose.Nodes.Parens
{
    public class ParenCapture
    {
        private readonly int _number;
        private readonly int _index;
        private readonly string _value;

        private ParenCapture(int number, int index, string value)
        {
            _number = number;
            _value = value;
            _index = index;
        }

        public static ParenCapture Pass(int number, int index, string value)
        {
            Debug.Assert(index >= 0);

            return new ParenCapture(number, index, value);
        }

        public static ParenCapture Fail(int number)
        {
            return new ParenCapture(number, -1, "");
        }

        public int Number
        {
            get { return _number; }
        }

        public int Index
        {
            get { return _index; }
        }

        public string Value
        {
            get { return _value; }
        }

        public bool Success
        {
            get { return Index >= 0; }
        }
    }
}