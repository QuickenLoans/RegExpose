using System.Collections.Generic;

namespace RegExpose.Nodes
{
    /// <summary>
    ///   A node that contains one or many child nodes.
    /// </summary>
    public abstract class ContainerNode : RegexNode
    {
        internal ContainerNode(IEnumerable<RegexNode> children, int index, string pattern)
            : base(index, pattern)
        {
            Children = new RegexNodeCollection(children);
        }

        public RegexNodeCollection Children { get; private set; }
    }
}