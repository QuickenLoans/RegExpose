namespace RegExpose.Nodes.Quantifiers
{
    public class PossessiveQuestionMark : PossessiveQuantifier
    {
        public PossessiveQuestionMark(RegexNode child, int index, string pattern)
            : base(0, 1, child, index, pattern)
        {
        }
    }
}