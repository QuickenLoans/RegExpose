namespace RegExpose
{
    public class Capture
    {
        internal Capture(int index, int length, string value)
        {
            Index = index;
            Length = length;
            Value = value;
        }

        public int Index { get; private set; }
        public int Length { get; private set; }
        public string Value { get; private set; }

        public override string ToString()
        {
            return Value;
        }
    }
}