using System.Collections;
using System.Collections.Generic;

namespace RegExpose
{
    public class CaptureCollection : IEnumerable<Capture>
    {
        private readonly List<Capture> _captures = new List<Capture>();

        internal void Prepend(Capture capture)
        {
            _captures.Insert(0, capture);
        }

        public Capture this[int index]
        {
            get { return _captures[index]; }
        }

        public IEnumerator<Capture> GetEnumerator()
        {
            return _captures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _captures.Count; }
        }
    }
}