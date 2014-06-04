using System;
using System.IO;
using System.Linq;
using System.Text;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;

namespace Stratageme15.Core.Tools.JavascriptParser
{
    public class NodeInfoTree
    {
        public NodeInfo Root { get; private set; }

        public NodeInfo CurrentNode { get; private set; }
        private int _tabCount = 0;
        private string Tabs
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < _tabCount; i++)
                {
                    sb.Append(" ");
                }
                return sb.ToString();
            }
        }


        public NodeInfoTree(NodeInfo root)
        {
            Root = root;
            CurrentNode = Root;
        }

        public void Clarify<T>()
        {
            CurrentNode.ClarifyType(typeof(T));
#if DEBUG
            DbgClarify(typeof(T));
#endif
        }

        public void Push<T>(int line, int col)
        {
            NodeInfo ni = new NodeInfo(line,col,typeof(T));
            ni.Parent = CurrentNode;
            CurrentNode = ni;
#if DEBUG
            DbgPush(typeof(T));
#endif
        }

        public void Converge<TConvergedNode>(int line, int col)
        {
            NodeInfo ni = new NodeInfo(line, col,typeof(TConvergedNode));
            ni.Parent = CurrentNode.Parent;
            ni.AddChild(CurrentNode);
#if DEBUG
            DbgConverge(typeof(TConvergedNode));
#endif
            if (Root == CurrentNode) Root = ni;
            CurrentNode = ni;
        }

        public void Pop()
        {
            CurrentNode.Parent.AddChild(CurrentNode);
#if DEBUG
            DbgPop();
#endif
            CurrentNode = CurrentNode.Parent;
            
            
        }

        public void PopDropIfEmpty()
        {
            bool needToDrop = !CurrentNode.Children.Any() && !CurrentNode.Terminals.Any();
            if (!needToDrop)
            {
                CurrentNode.Parent.AddChild(CurrentNode);
            }
#if DEBUG
            DbgPop();
#endif
            CurrentNode = CurrentNode.Parent;
        }

        public void Operator(Enum e)
        {
            CurrentNode.Operators.Add(e);
        }

        public void Terminal<T>(object argument = null) where T : SyntaxTreeNodeBase
        {
            var node = argument == null ? (SyntaxTreeNodeBase)Activator.CreateInstance(typeof(T)) : (SyntaxTreeNodeBase)Activator.CreateInstance(typeof(T), new[] { argument });
            CurrentNode.AddTerminal(node);
            _tabCount++;
#if DEBUG
            Dbg(node.ToString());
#endif
            _tabCount--;
        }

#if DEBUG
        #region debug
        public static TextWriter _debugWriter;
        private void Dbg(string s)
        {
            if (_debugWriter == null) return;
            _debugWriter.Write(Tabs);
            _debugWriter.Write(s);
            _debugWriter.WriteLine();
            _debugWriter.Flush();
        }
        private void DbgPush(Type t)
        {
            _tabCount++;
            if (t == typeof(UnaryExpression)) return;
            Dbg(string.Format("{0} {1}", t.Name, Tabs));
        }

        private void DbgClarify(Type t)
        {
            Dbg(string.Format("clarified: {0}", t.Name));
        }

        private void DbgConverge(Type t)
        {
            _tabCount++;
            Dbg(string.Format("converged: {0}", t.Name));
        }
        private void DbgPop()
        {
            _tabCount--;
            if (CurrentNode.GetType() == typeof(UnaryExpression)) return;
            //Dbg("}");

        }
        public void DebugPrint()
        {
            _tabCount = 0;
            PrintNode(Root);
        }

        private void PrintNode(NodeInfo ni)
        {
            if (ni.NodeType != typeof(UnaryExpression))
            {
                _tabCount++;
                Dbg(string.Format("{0} \r\n{1}{{",ni.NodeType.Name,Tabs));
            }
            foreach (var ord in ni.Order)
            {
                if (ord is SyntaxTreeNodeBase) Dbg(ord.ToString());
                else
                {
                    PrintNode((NodeInfo) ord);
                }
            }
            if (ni.NodeType != typeof(UnaryExpression))
            {
                Dbg("}");
                _tabCount--;
            }

        }
        #endregion

#endif
    }
}
