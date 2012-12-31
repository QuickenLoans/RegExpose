using System.Collections.Generic;

namespace RegExpose.Nodes
{
    public abstract class RegexNode
    {
        protected RegexNode(int index, string pattern)
        {
            Index = index;
            Pattern = pattern;
        }

        /// <summary>
        /// Gets a value, unique across all nodes from the entire regular expression, that identifies the node.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Gets the string index, relative to the entire regular expression, where <see cref="Pattern"/> began.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the pattern that was responsible for the creation of the node.
        /// </summary>
        public string Pattern { get; private set; }

        /// <summary>
        /// Gets the human-readable type of the node.
        /// </summary>
        public abstract string NodeType { get; }

        internal abstract IEnumerable<ParseStep> Parse(IRegexEngine engine);

        internal virtual string GetPassMessage(string match, State initialState)
        {
            return string.Format("{0}, /{1}/, matched '{2}' starting at index {3}",
                                 NodeType,
                                 Pattern,
                                 match,
                                 initialState.Index);
        }

        internal virtual string GetFailMessage(State initialState)
        {
            return string.Format("{0}, /{1}/, failed to match starting at index {2}",
                                 NodeType,
                                 Pattern,
                                 initialState.Index);
        }

        public override string ToString()
        {
            return string.Format("{0}    {1}", Pattern, GetType().Name);
        }
    }
}