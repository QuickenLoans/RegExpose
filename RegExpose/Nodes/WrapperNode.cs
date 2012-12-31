namespace RegExpose.Nodes
{
    /// <summary>
    ///   A node that contains one child node.
    /// </summary>
    public abstract class WrapperNode : RegexNode
    {
        internal WrapperNode(RegexNode child, int index, string pattern)
            : base(index, pattern)
        {
            Child = child;
        }

        public RegexNode Child { get; private set; }
    }
}