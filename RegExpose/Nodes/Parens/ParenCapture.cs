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

        public bool Equals(ParenCapture other)
        {
            return _number == other._number && _index == other._index && string.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((ParenCapture)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _number;
                hashCode = (hashCode * 397) ^ _index;
                hashCode = (hashCode * 397) ^ (_value != null ? _value.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}