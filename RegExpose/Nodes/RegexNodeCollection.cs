using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RegExpose.Nodes
{
    public class RegexNodeCollection : RegexNodeCollection<RegexNode>
    {
        public RegexNodeCollection(IEnumerable<RegexNode> nodes)
            : base(nodes)
        {
        }

        internal LinkedListNode<RegexNode> FindLinkedListNode(RegexNode regexNode)
        {
            for (var item = _nodes.First; item != null; item = item.Next)
            {
                if (item.Value == regexNode)
                {
                    return item;
                }
            }

            return null;
        }
    }

    public class RegexNodeCollection<TRegexNode> : IEnumerable<TRegexNode>
        where TRegexNode : RegexNode
    {
        protected readonly LinkedList<TRegexNode> _nodes;

        public RegexNodeCollection(IEnumerable<TRegexNode> nodes)
        {
            _nodes = new LinkedList<TRegexNode>(nodes);
        }

        public LinkedListNode<TRegexNode> First
        {
            get { return _nodes.First; }
        }

        public int Count
        {
            get { return _nodes.Count; }
        }

        internal TRegexNode this[int index]
        {
            get { return _nodes.Skip(index).Take(1).SingleOrDefault(); }
        }

        #region IEnumerable<TRegexNode> Members

        public IEnumerator<TRegexNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        #endregion

        public RegexNode FindById(int id)
        {
            return FindBy(node => node.Id == id).FirstOrDefault();
        }

        public IEnumerable<RegexNode> FindBy(Func<RegexNode, bool> predicate)
        {
            return _nodes.Select(node => FindNodeBy(node, predicate)).SelectMany(x => x);
        }

        private static IEnumerable<RegexNode> FindNodeBy(RegexNode node, Func<RegexNode, bool> predicate)
        {
            if (predicate(node))
            {
                yield return node;
            }

            var containerNode = node as ContainerNode;
            if (containerNode != null)
            {
                foreach (var foundChild in containerNode.Children.Select(child => FindNodeBy(child, predicate)).SelectMany(x => x))
                {
                    yield return foundChild;
                }
            }

            var wrapperNode = node as WrapperNode;
            if (wrapperNode != null)
            {
                foreach (var foundChild in FindNodeBy(wrapperNode.Child, predicate))
                {
                    yield return foundChild;
                }
            }

            var alternation = node as Alternation.Alternation;
            if (alternation != null)
            {
                foreach (var foundChild in alternation.Choices.Select(choice => FindNodeBy(choice, predicate)).SelectMany(x => x))
                {
                    yield return foundChild;
                }
            }
        }
    }
}