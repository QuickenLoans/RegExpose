namespace RegExpose
{
    public class Group : Capture
    {
        private readonly int _number;
        private readonly CaptureCollection _captures = new CaptureCollection();
        private readonly bool _success;

        internal Group(int number, int index, int length, string value, bool success)
            : base(index, length, value)
        {
            _success = success;
            _number = number;
        }

        public int Number
        {
            get { return _number; }
        }

        public CaptureCollection Captures
        {
            get { return _captures; }
        }

        public bool Success
        {
            get { return _success; }
        }
    }
}