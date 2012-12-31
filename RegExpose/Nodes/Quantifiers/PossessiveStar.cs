namespace RegExpose.Nodes.Quantifiers
{
    public class PossessiveStar : PossessiveQuantifier
    {
        public PossessiveStar(RegexNode child, int index, string pattern)
            : base(0, null, child, index, pattern)
        {
        }
    }
}