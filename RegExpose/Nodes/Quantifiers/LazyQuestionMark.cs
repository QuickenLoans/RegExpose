namespace RegExpose.Nodes.Quantifiers
{
    public class LazyQuestionMark : LazyQuantifier
    {
        public LazyQuestionMark(RegexNode child, int index, string pattern)
            : base(0, 1, child, index, pattern)
        {
        }
    }
}