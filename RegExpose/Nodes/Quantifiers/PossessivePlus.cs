namespace RegExpose.Nodes.Quantifiers
{
    public class PossessivePlus : PossessiveQuantifier
    {
        public PossessivePlus(RegexNode child, int index, string pattern)
            : base(1, null, child, index, pattern)
        {
        }
    }
}