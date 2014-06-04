using System;
using System.Collections.Generic;
using System.Linq;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Markers;

namespace Stratageme15.Core.Tools.JavascriptParser
{
    public static class LinkedListExtensions
    {
        public static void Replace<T>(this LinkedList<T> list, T oldNode, T newNode)
        {
            var idx = list.Find(oldNode);
            LinkedListNode<T> ndi = new LinkedListNode<T>(newNode);
            list.AddAfter(idx, ndi);
            list.Remove(idx);
        }
    }

    public class NodeInfo : IHierarchical
    {
        public int Line { get; private set; }
        public int Column { get; private set; }

        public Type NodeType { get; private set; }

        public bool IsScolonNeededAsTheEnd { get; set; }

        public NodeInfo Parent { get; set; }

        public List<Enum> Operators { get; private set; }

        public LinkedList<NodeInfo> Children { get; private set; }

        public LinkedList<SyntaxTreeNodeBase> Terminals { get; private set; }

        public LinkedList<IHierarchical> Order { get; private set; }

        public void AddChild(NodeInfo ni)
        {
            Children.AddLast(ni);
            Order.AddLast(ni);
        }

        public void AddTerminal(SyntaxTreeNodeBase t)
        {
            Terminals.AddLast(t);
            Order.AddLast(t);
        }

        public void RemoveChild(IHierarchical h)
        {
            Children.Remove((NodeInfo)h);
            Order.Remove(h);
        }

        public void ReplaceChild(IHierarchical old, IHierarchical newChild)
        {
            Children.Replace((NodeInfo)old, (NodeInfo)newChild);
            Order.Replace(old, newChild);
        }

        public NodeInfo(int line, int column, Type nodeType)
        {
            Line = line;
            Column = column;
            Order = new LinkedList<IHierarchical>();
            NodeType = nodeType;
            Terminals = new LinkedList<SyntaxTreeNodeBase>();
            Children = new LinkedList<NodeInfo>();
            IsScolonNeededAsTheEnd = false;
            Operators = new List<Enum>();
        }

        public void ClarifyType(Type nodeType)
        {
            NodeType = nodeType;
        }

        public override string ToString()
        {
            return string.Format("{0} {3} [chld:{1}] [term:{2}]", NodeType.Name, Children.Count, Terminals.Count, Operators.Select(c=>c.ToString()).Aggregate((a,v)=>string.Format("{0}{1}",a,v)));
        }
    }
}
