namespace RegExpose.Nodes.Quantifiers
{
    public class LazyStar : LazyQuantifier
    {
        public LazyStar(RegexNode child, int index, string pattern)
            : base(0, null, child, index, pattern)
        {
        }
    }
}