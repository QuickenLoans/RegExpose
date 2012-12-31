using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace RegExpose.PatternParsing
{
    internal class Failure<T> : IFailure<T>
    {
        private readonly Func<IEnumerable<string>> _expectations;
        private readonly Input _input;
        private readonly Func<string> _message;

        public Failure(Input input, Func<string> message, Func<IEnumerable<string>> expectations)
        {
            _input = input;
            _message = message;
            _expectations = expectations;
        }

        #region IFailure<T> Members

        public string Message
        {
            get { return _message(); }
        }

        public IEnumerable<string> Expectations
        {
            get { return _expectations(); }
        }

        public Input FailedInput
        {
            get { return _input; }
        }

        #endregion

        public override string ToString()
        {
            var expMsg = "";

            if (Expectations.Any())
            {
                expMsg = " expected " + Expectations.Aggregate((e1, e2) => e1 + " or " + e2);
            }

            return string.Format("Parsing failure: {0};{1} ({2}).", Message, expMsg, FailedInput);
        }
    }
}