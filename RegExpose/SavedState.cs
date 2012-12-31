using System.Collections.Generic;

namespace RandomSkunk.RegularExpressions
{
    internal class SavedState
    {
        public SavedState()
        {
            CaptureNumbers = new List<int>();
        }

        public State State { get; set; }
        public string MatchedText { get; set; }

        public IEnumerator<ParseStep> Enumerator { get; set; }
        public LinkedListNode<RegexNode> Item { get; set; }
        public IList<int> CaptureNumbers { get; private set; }
    }
}