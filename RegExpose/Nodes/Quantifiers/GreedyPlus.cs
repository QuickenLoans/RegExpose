namespace RegExpose.Nodes.Quantifiers
{
    public class GreedyPlus : GreedyQuantifier
    {
        public GreedyPlus(RegexNode child, int index, string pattern)
            : base(1, null, child, index, pattern)
        {
        }

        public override string NodeType
        {
            get { return "Greedy Quantifier - Plus"; }
        }
    }
}