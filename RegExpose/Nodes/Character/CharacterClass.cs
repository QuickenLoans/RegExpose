using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RegExpose.Nodes.Character
{
    public class CharacterClass : CharacterNode
    {
        private readonly IList<ICharacterClassPart> _characterClassParts;

        internal CharacterClass(IEnumerable<ICharacterClassPart> characterClassParts,
                                bool negated,
                                bool ignoreCase,
                                int index,
                                string pattern)
            : base(ignoreCase, index, pattern)
        {
            _characterClassParts = new ReadOnlyCollection<ICharacterClassPart>(characterClassParts.ToList());
            Negated = negated;
        }

        public bool Negated { get; private set; }

        public IList<ICharacterClassPart> Parts
        {
            get { return _characterClassParts; }
        }

        public override bool Matches(char input)
        {
            if (Negated)
            {
                return _characterClassParts.All(x => !x.Matches(input));
            }

            return _characterClassParts.Any(x => x.Matches(input));
        }

        public override string NodeType
        {
            get { return Negated ? "Negated " : "" + "Character Class"; }
        }

        internal override string GetPassMessage(string match, State initialState)
        {
            return string.Format("{0}, /{1}/{2}, matched '{3}' at index {4}",
                                 NodeType,
                                 Pattern,
                                 IgnoreCase ? " (IgnoreCase:true)" : "",
                                 match,
                                 initialState.Index);
        }

        internal override string GetFailMessage(State initialState)
        {
            return string.Format("{0}, /{1}/{2}, failed to match '{3}' at index {4}",
                                 NodeType,
                                 Pattern,
                                 IgnoreCase ? " (IgnoreCase:true)" : "",
                                 initialState.Current,
                                 initialState.Index);
        }
    }
}