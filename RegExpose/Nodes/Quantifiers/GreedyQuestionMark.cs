namespace RegExpose.Nodes.Quantifiers
{
    public class GreedyQuestionMark : GreedyQuantifier
    {
        public GreedyQuestionMark(RegexNode child, int index, string pattern)
            : base(0, 1, child, index, pattern)
        {
        }

        public override string NodeType
        {
            get { return "Greedy Quantifier - Question Mark"; }
        }
    }
}