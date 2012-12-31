namespace RegExpose.Nodes.Quantifiers
{
    public class GreedyStar : GreedyQuantifier
    {
        public GreedyStar(RegexNode child, int index, string pattern)
            : base(0, null, child, index, pattern)
        {
        }

        public override string NodeType
        {
            get { return "Greedy Quantifier - Star"; }
        }
    }
}