namespace RegExpose.Nodes.Quantifiers
{
    public class LazyPlus : LazyQuantifier
    {
        public LazyPlus(RegexNode child, int index, string pattern)
            : base(1, null, child, index, pattern)
        {
        }
    }
}