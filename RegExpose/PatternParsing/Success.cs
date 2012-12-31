using Sprache;

namespace RegExpose.PatternParsing
{
    internal sealed class Success<T> : ISuccess<T>
    {
        private readonly Input _remainder;
        private readonly T _result;

        public Success(T result, Input remainder)
        {
            _result = result;
            _remainder = remainder;
        }

        #region ISuccess<T> Members

        public T Result
        {
            get { return _result; }
        }

        public Input Remainder
        {
            get { return _remainder; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Successful parsing of {0}.", Result);
        }
    }
}