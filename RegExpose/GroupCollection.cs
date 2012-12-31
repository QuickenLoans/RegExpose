using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RegExpose
{
    public class GroupCollection : IEnumerable<Group>
    {
        private readonly List<Group> _groups = new List<Group>();

        internal void Append(Group group)
        {
            _groups.Add(group);
        }
        
        public IEnumerator<Group> GetEnumerator()
        {
            return _groups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Group this[int number]
        {
            get { return _groups.SingleOrDefault(g => g.Number == number); }
        }

        public int Count
        {
            get { return _groups.Count; }
        }
    }
}