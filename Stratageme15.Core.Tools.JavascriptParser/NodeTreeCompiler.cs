using System;
using System.Linq;
using Stratageme15.Core.JavascriptCodeDom;
using Stratageme15.Core.JavascriptCodeDom.Expressions;
using Stratageme15.Core.JavascriptCodeDom.Expressions.Primary;
using Stratageme15.Core.JavascriptCodeDom.Statements;

namespace Stratageme15.Core.Tools.JavascriptParser
{
    public static class NodeTreeCompiler
    {
        private static Type _nodebaseType = typeof(SyntaxTreeNodeBase);

        public static JsProgram Compile(NodeInfo root)
        {
            return (JsProgram)CompileNode(root, null,null);
        }

        private static SyntaxTreeNodeBase CompileNode(NodeInfo info, SyntaxTreeNodeBase parent, NodeInfo parentNodeInfo)
        {
            SyntaxTreeNodeBase node = (SyntaxTreeNodeBase)Activator.CreateInstance(info.NodeType);
            node.IsScolonNeeded = info.IsScolonNeededAsTheEnd;

            bool isExpression = typeof(Expression).IsAssignableFrom(info.NodeType);
            foreach (var o in info.Order)
            {
                if (o is SyntaxTreeNodeBase)
                {
                    var b = ((SyntaxTreeNodeBase)o);
                    b.Parent = node;
                    node.CollectSymbol(b);
                }
                else
                {
                    var ni = (NodeInfo)o;
                    var compiled = CompileNode(ni, node,info);
                    compiled.Parent = node;
                    node.CollectSymbol(compiled);
                }
            }
            if (info.Operators.Any())
            {
                foreach (var oprtr in info.Operators)
                {
                    var optype = oprtr.GetType();
                    var method = _nodebaseType.GetMethod("CollectOperator", new[] { optype });
                    method.Invoke(node, new[] { oprtr });    
                }
            }

            node = NormalizeNode(node,parent);
            node.Parent = parent;
            return node;
        }
       
        private static SyntaxTreeNodeBase NormalizeNode(SyntaxTreeNodeBase node,SyntaxTreeNodeBase parent)
        {
            if (!(node is UnaryExpression)) return node;
            var uex = (UnaryExpression) node;
            if (uex.FirstOperator != null) return node;
            
            while (uex.Operand is UnaryExpression && uex.FirstOperator == null)
            {
                uex = (UnaryExpression) uex.Operand;
            }

            Expression result = uex;
            if (uex.FirstOperator == null)
            {
                result = uex.Operand;
            }
            
            if (parent is CodeBlock && result is CallExpression)
            {
                CallStatement cs = new CallStatement();
                cs.CollectSymbol(result);
                return cs;
            }
            return result;
        }
    }
}
