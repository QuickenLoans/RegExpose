namespace RegExpose.Nodes
{
    /// <summary>
    ///   A node that contains no child nodes.
    /// </summary>
    public abstract class LeafNode : RegexNode
    {
        internal LeafNode(int index, string pattern)
            : base(index, pattern)
        {
        }
    }
}